import argparse
import json
import sys
from pathlib import Path


def main(argv=None):
    argv = list(sys.argv[1:] if argv is None else argv)
    if argv and argv[0] == "extract":
        from .extract import main as extract_main
        return extract_main(argv[1:])
    parser = argparse.ArgumentParser(description="HydroDataLab ETL — même moteur CLI/HTTP/Docker")
    parser.add_argument("command", choices=["run"])
    parser.add_argument("--source", choices=["fixture", "live"], default="fixture")
    parser.add_argument("--date", help="Journée civile terminée à Toronto, YYYY-MM-DD")
    parser.add_argument("--output", type=Path)
    parser.add_argument("--run-id")
    parser.add_argument("--quality-scenario", choices=["complete", "missing"], default="complete")
    parser.add_argument("--simulate-failure", action="store_true")
    args = parser.parse_args(argv)
    from .pipeline import run_pipeline
    try:
        result = run_pipeline(args.source, output=args.output, run_id=args.run_id,
                              quality_scenario=args.quality_scenario, day=args.date, simulate_failure=args.simulate_failure)
    except (ValueError, RuntimeError, OSError) as error:
        print(f"Échec : {error}", file=sys.stderr)
        return 1
    print(json.dumps({"runId": result["runId"], "summary": result["summary"], "warnings": result["warnings"]}, ensure_ascii=False))
    return 0


raise SystemExit(main())
