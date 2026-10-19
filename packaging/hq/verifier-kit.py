"""Vérifier les fichiers livrés, sans téléchargement ni modification."""
import hashlib
import json
import sys
from pathlib import Path, PurePosixPath


def verify(root):
    manifest = json.loads((root / "SHA256SUMS.json").read_text(encoding="utf-8"))
    failures = []
    seen = set()
    for entry in manifest["files"]:
        name = entry["path"]
        relative = PurePosixPath(name)
        if relative.is_absolute() or ".." in relative.parts or "\\" in name or ":" in name or name.casefold() in seen:
            raise ValueError(f"Chemin incorrect ou duplique : {name}")
        seen.add(name.casefold())
        target = (root / Path(*relative.parts)).resolve()
        if not target.is_relative_to(root) or not target.is_file():
            failures.append(f"Absent ou hors dossier : {name}")
            continue
        if target.stat().st_size != entry["bytes"]:
            failures.append(f"Taille incorrecte : {name}")
            continue
        if target.stat().st_size > 100_000_000:
            print(f"Verification : {name}...", flush=True)
        with target.open("rb") as stream:
            digest = hashlib.file_digest(stream, "sha256").hexdigest()
        if digest != entry["sha256"]:
            failures.append(f"Empreinte incorrecte : {name}")
    if failures:
        print("\n".join(failures), file=sys.stderr)
        return 1
    print(f"OK : {len(seen)} fichiers livres verifies (SHA-256).")
    print("Les fichiers de travail ajoutes ne sont pas modifies ni controles.")
    return 0


if __name__ == "__main__":
    try:
        sys.exit(verify(Path(__file__).resolve().parent))
    except (OSError, ValueError, KeyError, TypeError) as error:
        print(f"Verification impossible : {error}", file=sys.stderr)
        sys.exit(1)
