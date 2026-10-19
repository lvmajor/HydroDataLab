"""Présentation statique locale : Python standard library, sans npm."""
import argparse
import mimetypes
from functools import partial
from http.server import SimpleHTTPRequestHandler, ThreadingHTTPServer
from pathlib import Path
from urllib.parse import unquote, urlsplit


class PresentationHandler(SimpleHTTPRequestHandler):
    def prepare_path(self):
        path = unquote(urlsplit(self.path).path)
        # Compatibilité avec les anciens builds Slidev à chemins relatifs.
        marker = "/assets/"
        if marker in path:
            self.path = marker + path.split(marker, 1)[1]
        target = Path(self.translate_path(self.path))
        if not target.exists() and not Path(path).suffix and marker not in path:
            self.path = "/index.html"

    def do_GET(self):
        self.prepare_path()
        super().do_GET()

    def do_HEAD(self):
        self.prepare_path()
        super().do_HEAD()


if __name__ == "__main__":
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--port", type=int, default=53173)
    args = parser.parse_args()
    root = Path(__file__).resolve().parent / "presentation"
    if not (root / "index.html").is_file():
        parser.error("Le dossier presentation/index.html est manquant")
    mimetypes.add_type("application/javascript", ".js")
    mimetypes.add_type("application/javascript", ".mjs")
    server = ThreadingHTTPServer(("127.0.0.1", args.port), partial(PresentationHandler, directory=str(root)))
    print(f"Présentation : http://localhost:{args.port}/1", flush=True)
    print("Arrêt : Ctrl+C", flush=True)
    try:
        server.serve_forever()
    except KeyboardInterrupt:
        pass
    finally:
        server.server_close()
