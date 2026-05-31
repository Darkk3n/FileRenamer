FileRenamer
===========

Overview
--------
FileRenamer is a Windows desktop (WinForms) utility targeting .NET 10. It scans PDF files, extracts structured data from each page, presents the extracted data in an editable preview grid, and exports renamed single‑page PDFs using a deterministic filename format. The tool is optimized for batch processing of payment documents.

What changed
------------
- The app now parses PDF pages and extracts fields: amount, vendor name, reason/concept, and currency.
- Multi‑page PDFs are split into single‑page output files when appropriate; true multipage source documents are preserved while pre‑segmented single‑page sources can be removed after export.
- A progress dialog shows per‑file and per‑page processing status.
- Extraction includes heuristic cleaning (spacing fixes and split‑word healing) to improve vendor and reason text quality.
- iText (iText.Kernel) is used for robust PDF text extraction and page copying.

Key features
------------
- Per‑page PDF text extraction and field parsing (amount, vendor, concept/reason, currency)
- DataGridView preview where extracted rows can be reviewed and edited before export
- Deterministic filename generation using date, selected company, vendor, concept, amount, and currency
- Splits and writes single‑page PDF files using iText, preserves multipage source files, and optionally deletes pre‑segmented single‑page sources
- Progress UI with status messages and graceful error handling for corrupted PDFs

How it works (usage summary)
---------------------------
1. Select a company from the UI control.
2. Scan a folder of PDFs. Each page is parsed and added as a row in the preview grid.
3. Review and edit extracted values in the grid if needed.
4. Start processing: the app copies pages into new single‑page PDFs and names them using the configured pattern.

Filename format
---------------
Output files are named with the following pattern:

<date>-<company>-<vendor> <concept>-<amount> <currency>.pdf

Common tokens
-------------
- {name} — original filename without extension
- {ext} — file extension
- {counter} — sequential number (configurable start and padding)
- {date:format} — file timestamp formatted with .NET date format strings (e.g., {date:yyyyMMdd})

Dependencies
------------
- iText.Kernel for PDF parsing and page copying. Check the project file for the exact package/version.

Troubleshooting
---------------
- When a PDF fails to parse the app will add an error row to the grid describing the failure; inspect and skip or correct that row.
- Ensure the app has write/delete permissions for the target directory when export fails.
- Large folders or many multipage PDFs will increase processing time; the progress dialog reports ongoing status.

Author
------
Created by the repository owner.
