"""Vérification explicite CLI native/conteneur, hors collecte pytest et sans réseau d'exécution."""
import argparse
import hashlib
import json
import subprocess
from pathlib import Path
from uuid import uuid4

import pandas as pd

from hydrodatalab.pipeline import run_pipeline


def main():
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--engine", choices=("docker", "podman"), default="docker")
    parser.add_argument("--docker-context")
    parser.add_argument("--image", default="hydrodatalab-etl:formation")
    parser.add_argument("--output", type=Path, default=Path("etl/.test-output/container-parity"))
    args = parser.parse_args()
    if args.engine == "podman" and args.docker_context:
        parser.error("--docker-context ne s'applique qu'au moteur docker")
    docker = [args.engine] + (["--context", args.docker_context] if args.docker_context else [])
    run_id = str(uuid4())
    root = args.output.resolve() / run_id
    local_root, container_root = root / "local", root / "container"
    local_report = run_pipeline(output=local_root, run_id=run_id)
    container_root.mkdir(parents=True)
    name = f"hydrodatalab-parity-{run_id}"
    subprocess.run(docker + ["create", "--name", name, "--network", "none", args.image,
                            "python", "-m", "hydrodatalab", "run", "--source", "fixture",
                            "--date", local_report["period"]["start"],
                            "--output", "/app/outputs/parity", "--run-id", run_id], check=True, capture_output=True)
    try:
        subprocess.run(docker + ["start", "--attach", name], check=True)
        status = subprocess.check_output(docker + ["inspect", "--format", "{{.State.ExitCode}}", name], text=True).strip()
        if status != "0":
            raise RuntimeError(f"Le conteneur a terminé avec le code {status}")
        subprocess.run(docker + ["cp", f"{name}:/app/outputs/parity/{run_id}/.", str(container_root)], check=True)
    finally:
        subprocess.run(docker + ["rm", name], check=True, capture_output=True)
    local = local_root / run_id
    local_csv = (local / "observations.csv").read_bytes()
    assert local_csv == (container_root / "observations.csv").read_bytes(), "Les octets CSV diffèrent"
    pd.testing.assert_frame_equal(pd.read_parquet(local / "observations.parquet"),
                                  pd.read_parquet(container_root / "observations.parquet"))
    container_report = json.loads((container_root / "report.json").read_text(encoding="utf-8"))
    for report in (local_report, container_report):
        report.pop("generatedAtUtc")
    assert local_report == container_report, "Les rapports métier diffèrent"
    evidence = {"runId": run_id, "image": args.image, "engine": args.engine, "context": args.docker_context,
                "network": "none", "csvBytesEqual": True, "parquetDataframesEqual": True,
                "businessReportsEqual": True, "csvSha256": hashlib.sha256(local_csv).hexdigest(),
                "summary": local_report["summary"]}
    (root / "verification.json").write_text(json.dumps(evidence, indent=2), encoding="utf-8")
    print(json.dumps(evidence, ensure_ascii=False))


if __name__ == "__main__":
    main()
