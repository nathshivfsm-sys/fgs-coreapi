#!/usr/bin/env python3
"""Convert EC2_FULL_SETUP_AND_CD.md to PDF using markdown + Edge/Chrome headless."""

from __future__ import annotations

import shutil
import subprocess
import sys
from pathlib import Path

ROOT = Path(__file__).resolve().parents[3]
MD_PATH = ROOT / "deployment" / "aws" / "manual-guide" / "EC2_FULL_SETUP_AND_CD.md"
HTML_PATH = MD_PATH.with_suffix(".html")
PDF_PATH = MD_PATH.with_suffix(".pdf")

CSS = """
body {
  font-family: Segoe UI, Arial, sans-serif;
  font-size: 11pt;
  line-height: 1.45;
  max-width: 900px;
  margin: 2em auto;
  padding: 0 1.5em;
  color: #1a1a1a;
}
h1 { font-size: 22pt; border-bottom: 2px solid #2563eb; padding-bottom: 0.3em; }
h2 { font-size: 16pt; margin-top: 1.4em; color: #1e40af; }
h3 { font-size: 13pt; margin-top: 1em; }
code, pre { font-family: Consolas, monospace; font-size: 9pt; }
pre {
  background: #f4f4f5;
  border: 1px solid #e4e4e7;
  border-radius: 4px;
  padding: 0.8em 1em;
  overflow-x: auto;
  white-space: pre-wrap;
  word-break: break-word;
}
table { border-collapse: collapse; width: 100%; margin: 1em 0; font-size: 10pt; }
th, td { border: 1px solid #d4d4d8; padding: 0.45em 0.6em; text-align: left; }
th { background: #eff6ff; }
hr { border: none; border-top: 1px solid #d4d4d8; margin: 2em 0; }
ul, ol { padding-left: 1.4em; }
li { margin: 0.25em 0; }
@media print {
  body { margin: 0; max-width: none; }
  pre { page-break-inside: avoid; }
  h2, h3 { page-break-after: avoid; }
}
"""


def find_browser() -> Path | None:
    candidates = [
        Path(r"C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe"),
        Path(r"C:\Program Files\Microsoft\Edge\Application\msedge.exe"),
        Path(r"C:\Program Files\Google\Chrome\Application\chrome.exe"),
        Path(r"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe"),
    ]
    for path in candidates:
        if path.is_file():
            return path
    for name in ("msedge", "chrome", "chromium"):
        found = shutil.which(name)
        if found:
            return Path(found)
    return None


def main() -> int:
    try:
        import markdown
    except ImportError:
        subprocess.check_call([sys.executable, "-m", "pip", "install", "markdown", "-q"])
        import markdown

    if not MD_PATH.is_file():
        print(f"Missing: {MD_PATH}", file=sys.stderr)
        return 1

    text = MD_PATH.read_text(encoding="utf-8")
    body = markdown.markdown(
        text,
        extensions=["tables", "fenced_code", "toc"],
    )
    html = f"""<!DOCTYPE html>
<html lang="en">
<head>
  <meta charset="utf-8">
  <title>FGS EC2 Full Setup and CD</title>
  <style>{CSS}</style>
</head>
<body>
{body}
</body>
</html>
"""
    HTML_PATH.write_text(html, encoding="utf-8")

    browser = find_browser()
    if not browser:
        print("No Edge/Chrome found. HTML written:", HTML_PATH, file=sys.stderr)
        return 1

    html_uri = HTML_PATH.resolve().as_uri()
    cmd = [
        str(browser),
        "--headless=new",
        "--disable-gpu",
        "--no-pdf-header-footer",
        f"--print-to-pdf={PDF_PATH.resolve()}",
        html_uri,
    ]
    result = subprocess.run(cmd, capture_output=True, text=True)
    if result.returncode != 0:
        print(result.stderr or result.stdout, file=sys.stderr)
        return result.returncode

    if not PDF_PATH.is_file():
        print("PDF was not created.", file=sys.stderr)
        return 1

    print(f"PDF: {PDF_PATH}")
    print(f"HTML: {HTML_PATH}")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
