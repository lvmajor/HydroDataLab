"""Durées mesurées, sans ralentissement artificiel du pipeline."""
from contextlib import contextmanager
from time import perf_counter

from .extract import utc_now


class Execution:
    def __init__(self):
        self.steps = []

    @contextmanager
    def step(self, key, label):
        step = {"id": key, "label": label, "startedAtUtc": utc_now(), "status": "running"}
        self.steps.append(step)
        started = perf_counter()
        try:
            yield
            step["status"] = "succeeded"
        except Exception as error:
            step["status"] = "failed"
            step["error"] = str(error)
            error.execution = self.as_dict()
            raise
        finally:
            step["durationMs"] = round((perf_counter() - started) * 1000, 3)
            step["completedAtUtc"] = utc_now()

    def as_dict(self):
        return {"steps": self.steps}
