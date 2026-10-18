FROM ghcr.io/astral-sh/uv:0.11.28 AS uv
FROM python:3.14.2-slim
COPY --from=uv /uv /usr/local/bin/uv
WORKDIR /app
ENV UV_LINK_MODE=copy PYTHONUNBUFFERED=1 PYTHONDONTWRITEBYTECODE=1
COPY etl/pyproject.toml etl/uv.lock ./
COPY etl/hydrodatalab ./hydrodatalab
RUN uv sync --frozen --no-dev
COPY data/fixtures /app/data/fixtures
ENV PATH="/app/.venv/bin:$PATH" HYDRO_FIXTURE_DIR=/app/data/fixtures HYDRO_OUTPUT_DIR=/app/outputs
EXPOSE 8001
HEALTHCHECK --interval=5s --timeout=3s --start-period=15s --retries=3 CMD ["python", "-c", "import urllib.request; urllib.request.urlopen('http://127.0.0.1:8001/health', timeout=2).read()"]
CMD ["uvicorn", "hydrodatalab.api:app", "--host", "0.0.0.0", "--port", "8001", "--workers", "1"]
