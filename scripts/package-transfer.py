"""Créer le ZIP HQ allégé sans copier le TAR dans un dossier intermédiaire.

Prérequis : dépôt propre, build Slidev courant, guide HTML généré, images validées.
Usage depuis la racine : python scripts/package-transfer.py
"""
import hashlib
import io
import json
import subprocess
import tarfile
import zipfile
from datetime import datetime, timezone
from pathlib import Path

ROOT = Path(__file__).resolve().parent.parent
PREFIX = "HydroDataLab-HQ/"
EXERCISE = "9e1ecde478f4f5d70634a42c720fa10fd224eab7"
TAGS = ["00-socle", "01-instructions", "02-extraction", "03-transformation-bi",
        "04-pilotage", "05-docker", "06-specification", "07-solution"]


def git(*args):
    return subprocess.check_output(["git", *args], cwd=ROOT, text=True, encoding="utf-8").strip()


def digest_file(path):
    with path.open("rb") as stream:
        return hashlib.file_digest(stream, "sha256").hexdigest()


def main():
    if git("status", "--porcelain"):
        raise RuntimeError("Enregistrer les changements dans Git avant le transfert.")
    head = git("rev-parse", "HEAD")
    if git("rev-parse", "refs/heads/main") != head:
        raise RuntimeError("Le kit doit partir du HEAD de main.")
    if git("rev-parse", "refs/heads/codex/preparation-demo-06") != EXERCISE:
        raise RuntimeError("La branche pedagogique differe du guide.")
    stamp = datetime.now().strftime("%Y%m%d")
    target = ROOT / "artifacts" / f"HydroDataLab-HQ-{stamp}-{head[:7]}.zip"
    partial = target.with_suffix(".zip.partial")
    if target.exists() or partial.exists():
        raise FileExistsError(f"Sortie existante conservee : {target} ou {partial}")

    archive = ROOT / "artifacts/hydrodatalab-images.tar"
    proof = json.loads(archive.with_suffix(".tar.manifest.json").read_text(encoding="utf-8-sig"))
    declared = archive.with_suffix(".tar.sha256").read_text(encoding="utf-8-sig").split()[0].lower()
    print("1/6 Verification des images et des donnees...", flush=True)
    if not proof["manifestValidated"] or declared != proof["sha256"] or archive.stat().st_size != proof["bytes"]:
        raise RuntimeError("Metadonnees d'images incoherentes.")
    platforms = []
    with tarfile.open(archive) as tar:
        manifest = json.load(tar.extractfile("manifest.json"))
        if len(manifest) != 4 or len(proof["images"]) != 4:
            raise RuntimeError("Quatre images sont requises.")
        for entry in manifest:
            raw = tar.extractfile(entry["Config"]).read()
            config = json.loads(raw)
            known = [image for image in proof["images"] if image["Image"] in entry["RepoTags"]]
            if len(known) != 1 or hashlib.sha256(raw).hexdigest() != known[0]["Id"]:
                raise RuntimeError("Configuration d'image inattendue.")
            platform = f"{config['os']}/{config['architecture']}"
            if platform != "linux/amd64":
                raise RuntimeError("Plateforme differente du guide.")
            platforms.append({"tags": entry["RepoTags"], "platform": platform})
    fixture = json.loads((ROOT / "data/fixtures/manifest.json").read_text(encoding="utf-8-sig"))
    for entry in fixture["sources"]:
        if digest_file(ROOT / "data/fixtures" / entry["file"]) != entry["sha256"]:
            raise RuntimeError("Donnees fixture modifiees.")

    print("2/6 Bundle unique : main, exercice et huit tags...", flush=True)
    (ROOT / ".runtime").mkdir(exist_ok=True)
    bundle = ROOT / ".runtime" / f"transfer-source-{head[:12]}.bundle"
    refs = ["refs/heads/main", "refs/heads/codex/preparation-demo-06"] + [f"refs/tags/{tag}" for tag in TAGS]
    if not bundle.exists():
        subprocess.run(["git", "bundle", "create", str(bundle), *refs], cwd=ROOT, check=True)
    subprocess.run(["git", "bundle", "verify", str(bundle)], cwd=ROOT, check=True, capture_output=True)
    actual_refs = dict(line.split(maxsplit=1)[::-1] for line in git("bundle", "list-heads", str(bundle)).splitlines())
    expected_refs = {ref:git("rev-parse", ref) for ref in refs}
    if actual_refs != expected_refs:
        raise RuntimeError("Refs du bundle incorrectes.")

    files = {}
    def add(source, destination):
        path = (ROOT / source).resolve()
        if not path.is_relative_to(ROOT) or not path.is_file() or path.is_symlink():
            raise RuntimeError(f"Source invalide : {path}")
        if destination in files:
            raise RuntimeError(f"Destination dupliquee : {destination}")
        files[destination] = path
    def tree(source, destination):
        folder = ROOT / source
        for path in sorted(folder.rglob("*")):
            if path.is_file():
                add(path.relative_to(ROOT), f"{destination}/{path.relative_to(folder).as_posix()}")

    tree("presentation/dist", "presentation")
    tree("presentation/public", "presentation/public")
    tree("handouts", "handouts")
    tree("data", "data")
    tree("docs/screenshots", "docs/screenshots")
    for path in (ROOT / "docs").glob("*.md"):
        add(path.relative_to(ROOT), f"docs/{path.name}")
    for name in ["LIRE-MOI-HQ.md", "LIRE-MOI-HQ.html", "verifier-kit.py", "servir-presentation.py"]:
        add(f"packaging/hq/{name}", name)
    for name in ["docker-compose.yaml", "images.env"]:
        add(f"artifacts/compose/{name}", f"compose/{name}")
    for suffix in ["", ".sha256", ".manifest.json"]:
        add(f"artifacts/hydrodatalab-images.tar{suffix}", f"images/hydrodatalab-images.tar{suffix}")
    add("LICENSE", "LICENSE")
    add("presentation/exports/formation-hydrodatalab.pdf", "documents/formation-hydrodatalab.pdf")
    add(bundle.relative_to(ROOT), "developpement/HydroDataLab.bundle")
    if "presentation/index.html" not in files:
        raise RuntimeError("Build Slidev absent.")
    for name in files:
        if any(part in {"node_modules", ".venv", ".git", ".runtime", "bin", "obj"} for part in Path(name).parts):
            raise RuntimeError(f"Dependance ou cache non souhaites : {name}")

    revision = {
        "createdAtUtc": datetime.now(timezone.utc).isoformat(), "sourceCommit": head,
        "exerciseCommit": EXERCISE, "refs": expected_refs, "dockerArchiveSha256": declared,
        "images": platforms, "slideCount": 29, "explanationMinutes": 32, "demoMinutes": 13,
        "preparation": "LIRE-MOI-HQ.html", "nativeDependenciesIncluded": False,
        "hqLaptopRehearsal": "A effectuer sur le portable HQ",
    }
    records = []
    print(f"3/6 Compression de {len(files)} fichiers sans copie intermediaire du TAR...", flush=True)
    with zipfile.ZipFile(partial, "x", compression=zipfile.ZIP_DEFLATED, compresslevel=3, allowZip64=True) as output:
        for name, path in sorted(files.items()):
            sha = hashlib.sha256()
            size = 0
            if path.stat().st_size > 100_000_000:
                print(f"Compression : {name} ({path.stat().st_size / 1e9:.2f} Go)...", flush=True)
            with path.open("rb") as source, output.open(PREFIX + name, "w", force_zip64=True) as dest:
                while chunk := source.read(4 * 1024 * 1024):
                    dest.write(chunk)
                    sha.update(chunk)
                    size += len(chunk)
            record = {"path":name, "bytes":size, "sha256":sha.hexdigest()}
            records.append(record)
            if name == "images/hydrodatalab-images.tar" and record["sha256"] != declared:
                raise RuntimeError("Archive Docker differente de la preuve; ZIP partiel conserve.")
        raw = (json.dumps(revision, ensure_ascii=False, indent=2) + "\n").encode("utf-8")
        output.writestr(PREFIX + "REVISION.json", raw)
        records.append({"path":"REVISION.json", "bytes":len(raw), "sha256":hashlib.sha256(raw).hexdigest()})
        inventory = {"schemaVersion":1, "excludes":["SHA256SUMS.json"], "files":records}
        output.writestr(PREFIX + "SHA256SUMS.json", json.dumps(inventory, ensure_ascii=False, indent=2) + "\n")

    print("4/6 Relecture et SHA-256 de chaque fichier compresse...", flush=True)
    with zipfile.ZipFile(partial) as check:
        expected = {PREFIX + record["path"] for record in records} | {PREFIX + "SHA256SUMS.json"}
        if len(check.namelist()) != len(expected) or set(check.namelist()) != expected:
            raise RuntimeError("Inventaire ZIP incorrect.")
        for record in records:
            with check.open(PREFIX + record["path"]) as stream:
                if hashlib.file_digest(stream, "sha256").hexdigest() != record["sha256"]:
                    raise RuntimeError(f"Fichier ZIP invalide : {record['path']}")
        if json.loads(check.read(PREFIX + "SHA256SUMS.json"))["files"] != records:
            raise RuntimeError("Manifeste ZIP incorrect.")
    if git("rev-parse", "HEAD") != head or git("status", "--porcelain"):
        raise RuntimeError("Le depot a change pendant la generation; ZIP partiel conserve.")
    print("5/6 Publication du ZIP et de son empreinte...", flush=True)
    partial.rename(target)
    zip_hash = digest_file(target)
    target.with_suffix(".zip.sha256").write_text(f"{zip_hash}  {target.name}\n", encoding="utf-8")
    report = {
        "checkedAtUtc":datetime.now(timezone.utc).isoformat(), "zip":target.name,
        "sourceCommit":head, "sha256":zip_hash, "compressedBytes":target.stat().st_size,
        "uncompressedBytes":sum(record["bytes"] for record in records), "fileCount":len(records)+1,
        "allEntryHashesVerified":True, "bundleRefsVerified":True, "dockerManifestVerified":True,
        "dockerPlatform":"linux/amd64", "hqLaptopRehearsal":"A effectuer",
    }
    target.with_suffix(".zip.validation.json").write_text(json.dumps(report,indent=2)+"\n",encoding="utf-8")
    print("6/6 Termine : " + json.dumps(report), flush=True)


if __name__ == "__main__":
    main()
