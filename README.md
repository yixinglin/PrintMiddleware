# Printer Middleware

A lightweight Windows-based print service middleware that listens for print jobs via WebSocket and silently prints PDF files using local printers.

## Features

- WebSocket server for receiving print jobs (PDF URLs + printer IDs)
- Auto-download and silent print (via SumatraPDF)
- Printer auto-discovery (must be named as #1, #2, etc.)
- Real-time display of connected clients (IP list)
- System tray support (minimize to tray, exit from tray icon)
- Auto-start on system boot (optional)
- Real-time logging with UI display
- Logs saved to disk and auto-cleaned
- Multi-NIC support: displays all available LAN IPs and WebSocket endpoints

## Build & Run (Visual Studio)

1. Clone the repository:

2. Open the solution file `PrinterMiddleware.sln` in Visual Studio.

3. Restore NuGet packages (right-click on solution > "Restore NuGet Packages")

4. Make sure these NuGet packages are installed:
    - `Fleck`
    - `Newtonsoft.Json`

5. Download **SumatraPDF.exe** and place it in the same directory as the executable:
    - [https://www.sumatrapdfreader.org/download-free-pdf-viewer.html](https://www.sumatrapdfreader.org/download-free-pdf-viewer.html)

6. Build and run the project.

## Usage

- Configure port, auto-start, and see available printers in the main UI.
- Only printers named like `#1`, `#2`, etc. will be used.
- Client should send WebSocket message in the following format:

    ```json
    {
      "files": [
        { "printer_id": 1, "url": "https://example.com/file1.pdf" },
        { "printer_id": 2, "url": "https://example.com/file2.pdf" }
      ]
    }
    ```

- The middleware will:
  - Download each file
  - Route to the correct printer by number
  - Print using SumatraPDF in silent mode
  - Delete the temporary file after printing

## Notes

- Only PDF files are supported.
- This tool is intended for LAN use only.
- Do not expose to the internet.
