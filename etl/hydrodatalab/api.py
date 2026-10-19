"""Adaptateur HTTP mince; le calcul appartient au pipeline Python."""
import logging
from threading import Lock
from typing import Literal
from uuid import UUID

from fastapi import FastAPI, HTTPException
from fastapi.responses import FileResponse
from pydantic import BaseModel

from .pipeline import artifact_path, run_pipeline

app = FastAPI(title="HydroDataLab — ETL Python", version="0.1.0")
logger = logging.getLogger("hydrodatalab")
_run_lock = Lock()


class RunRequest(BaseModel):
    runId: UUID
    source: Literal["fixture", "live"] = "fixture"
    simulateFailure: bool = False
    qualityScenario: Literal["complete", "missing"] = "complete"


@app.get("/health")
def health():
    return {"status": "ok"}


@app.post("/run")
def run(request: RunRequest):
    if not _run_lock.acquire(blocking=False):
        raise HTTPException(status_code=409, detail="Un pipeline Python est déjà en cours. Réessayez après sa fin.")
    logger.info("pipeline start run_id=%s source=%s", request.runId, request.source)
    try:
        try:
            report = run_pipeline(request.source, run_id=str(request.runId), simulate_failure=request.simulateFailure,
                                  quality_scenario=request.qualityScenario)
        except ValueError as error:
            raise HTTPException(status_code=422, detail={"message": str(error), "execution": getattr(error, "execution", None)}) from error
        except (RuntimeError, OSError) as error:
            logger.exception("pipeline failed run_id=%s", request.runId)
            raise HTTPException(status_code=503, detail={"message": str(error), "execution": getattr(error, "execution", None)}) from error
        logger.info("pipeline complete run_id=%s observations=%s", request.runId, report["summary"]["observationCount"])
        return report
    finally:
        _run_lock.release()


@app.get("/artifacts/{run_id}/{filename}")
def artifact(run_id: UUID, filename: str):
    try:
        path = artifact_path(str(run_id), filename)
    except ValueError as error:
        raise HTTPException(status_code=400, detail={"message": str(error), "execution": getattr(error, "execution", None)}) from error
    if not path.is_file():
        raise HTTPException(status_code=404, detail="Export introuvable pour cette exécution")
    media_type = {"observations.csv": "text/csv", "hourly.csv": "text/csv", "observations.parquet": "application/vnd.apache.parquet", "report.json": "application/json"}[filename]
    return FileResponse(path, media_type=media_type, filename=filename)
