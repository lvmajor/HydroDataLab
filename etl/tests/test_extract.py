import io
import json
import shutil
import subprocess
import sys
from datetime import datetime, timezone
from urllib.error import HTTPError, URLError
from urllib.parse import parse_qs, urlparse

import pytest
from hydrodatalab.extract import _request_json, extract, fixture_dir


def test_snapshot_hash_tampering_rejected(tmp_path):
    shutil.copytree(fixture_dir(), tmp_path / "fixture")
    (tmp_path / "fixture" / "demand.json").write_text("{}", encoding="utf-8")
    with pytest.raises(ValueError, match="intégrité"):
        extract(directory=tmp_path / "fixture")


def test_live_uses_previous_local_day_and_pagination():
    calls = []
    def fetch(url):
        calls.append(url)
        query = parse_qs(urlparse(url).query)
        assert "2026-11-01T00:00:00-04:00" in query["where"][0]
        assert "2026-11-02T00:00:00-05:00" in query["where"][0]
        offset = int(query["offset"][0])
        count = 125 if "demande-electricite" in url else 75
        return {"total_count": count, "results": [{"index": i} for i in range(offset, min(offset + 100, count))]}
    bundle = extract("live", now=datetime(2026, 11, 2, 15, tzinfo=timezone.utc), fetch=fetch)
    assert len(calls) == 3
    assert len(bundle.demand) == 125 and len(bundle.hydrometeo) == 75
    assert bundle.period["start"] == "2026-11-01"
    assert all(item["license"] == "CC BY-NC 4.0" for item in bundle.provenance)


def test_live_empty_or_incomplete_pages_fail_without_fixture_fallback():
    now = datetime(2026, 10, 18, 20, tzinfo=timezone.utc)
    with pytest.raises(ValueError, match="Aucune donnée"):
        extract("live", now=now, fetch=lambda url: {"results": [], "total_count": 0})
    with pytest.raises(ValueError, match="incomplète"):
        extract("live", now=now, fetch=lambda url: {"results": [], "total_count": 5})


def test_network_transient_error_retries_and_permanent_error_does_not():
    attempts, waits = [], []
    def transient(request, timeout):
        attempts.append(request.full_url)
        assert timeout == 20
        if len(attempts) < 3:
            raise URLError("offline")
        return io.BytesIO(b'{"results": []}')
    assert _request_json("https://example.test", opener=transient, sleeper=waits.append) == {"results": []}
    assert waits == [0.5, 1.0]
    def permanent(request, timeout):
        raise HTTPError(request.full_url, 404, "missing", {}, None)
    with pytest.raises(RuntimeError, match="HTTP 404"):
        _request_json("https://example.test", opener=permanent, sleeper=lambda _: pytest.fail("must not retry"))
    def offline(request, timeout):
        raise URLError("offline")
    with pytest.raises(RuntimeError, match="mode fixture"):
        _request_json("https://example.test", opener=offline, sleeper=lambda _: None)


def test_standalone_extractor_imports_no_transform_modules(tmp_path):
    code = "import sys; from hydrodatalab.extract import main; main(['--source','fixture','--output',sys.argv[1]]); assert not any(x in sys.modules for x in ('pandas','hydrodatalab.pipeline','hydrodatalab.normalize','hydrodatalab.coverage'))"
    subprocess.run([sys.executable, "-c", code, str(tmp_path)], check=True, capture_output=True)
    assert len(extract(directory=tmp_path).demand) == 96
