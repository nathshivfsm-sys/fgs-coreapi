#!/usr/bin/env python3
"""Convert FGS_EC2_SSM_MANUAL_DEPLOY_RUNBOOK.md to PDF using markdown + Edge/Chrome headless."""

from __future__ import annotations

import shutil
import subprocess
import sys
from pathlib import Path

ROOT = Path(__file__).resolve().parents[3]
MD_PATH = ROOT / "deployment" / "aws" / "manual-guide" / "FGS_EC2_SSM_MANUAL_DEPLOY_RUNBOOK.md"
HTML_PATH = MD_PATH.with_suffix(".html")
PDF_PATH = MD_PATH.with_suffix(".pdf")

CSS = """
@page { size: A4; margin: 14mm 12mm; }
body {
  font-family: "Segoe UI", Calibri, Arial, sans-serif;
  font-size: 10.5pt;
  line-height: 1.42;
  max-width: 920px;
  margin: 1.2em auto;
  padding: 0 1.2em 2em;
  color: #18181b;
}
h1 {
  font-size: 20pt;
  color: #0f172a;
  border-bottom: 3px solid #1d4ed8;
  padding-bottom: 0.35em;
  margin-top: 0;
}
h2 {
  font-size: 14pt;
  margin-top: 1.5em;
  color: #1e3a8a;
  border-bottom: 1px solid #bfdbfe;
  padding-bottom: 0.2em;
  page-break-after: avoid;
}
h3 {
  font-size: 11.5pt;
  margin-top: 1.1em;
  color: #1e40af;
  page-break-after: avoid;
}
p { margin: 0.55em 0; }
code, pre { font-family: Consolas, "Courier New", monospace; font-size: 8.5pt; }
code {
  background: #f1f5f9;
  padding: 0.1em 0.35em;
  border-radius: 3px;
}
pre {
  background: #0f172a;
  color: #e2e8f0;
  border: 1px solid #334155;
  border-radius: 6px;
  padding: 0.85em 1em;
  overflow-x: auto;
  white-space: pre-wrap;
  word-break: break-word;
  page-break-inside: avoid;
}
table {
  border-collapse: collapse;
  width: 100%;
  margin: 0.9em 0;
  font-size: 9.5pt;
  page-break-inside: avoid;
}
th, td {
  border: 1px solid #cbd5e1;
  padding: 0.4em 0.55em;
  text-align: left;
  vertical-align: top;
}
th { background: #eff6ff; color: #1e3a8a; }
tr:nth-child(even) td { background: #f8fafc; }
hr { border: none; border-top: 1px solid #cbd5e1; margin: 1.6em 0; }
ul, ol { padding-left: 1.35em; margin: 0.45em 0; }
li { margin: 0.2em 0; }
strong { color: #0f172a; }
@media print {
  body { margin: 0; max-width: none; padding: 0; }
  a { color: inherit; text-decoration: none; }
  h2, h3 { page-break-after: avoid; }
  pre, table { page-break-inside: avoid; }
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
        extensions=["tables", "fenced_code", "toc", "sane_lists"],
    )
    html = f"""<!DOCTYPE html>
<html lang="en">
<head>
  <meta charset="utf-8">
  <title>FGS DEV — Manual EC2 Deployment Runbook (SSM)</title>
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
    print(f"MD:  {MD_PATH}")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
