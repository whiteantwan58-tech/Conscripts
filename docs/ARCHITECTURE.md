# CEC‑WAM‑HOT‑CORE — Architecture

## Overview

```
┌──────────────────────────────────────────────────────────┐
│                     Browser / PWA Shell                  │
│  ┌────────────┐  ┌──────────────┐  ┌──────────────────┐  │
│  │  Dashboard │  │  Data Panel  │  │  Charts Panel    │  │
│  │ (stats +   │  │ • Sheets CSV │  │ • Bar / Line     │  │
│  │  preview)  │  │ • Local CSV  │  │ • Pie / Doughnut │  │
│  └────────────┘  │ • Local XLSX │  │ • Polar / Radar  │  │
│  ┌────────────┐  └──────────────┘  └──────────────────┘  │
│  │ EVE Voice  │  ┌──────────────┐                        │
│  │ Interface  │  │  Settings    │                        │
│  │ • Speech   │  │  + About     │                        │
│  │ • TTS      │  └──────────────┘                        │
│  └────────────┘                                          │
└──────────────────────────────────────────────────────────┘
           │ Service Worker (sw.js)
           │ Offline-first cache (Cache API)
           ▼
┌──────────────────────────────────────────────────────────┐
│  IndexedDB (persistence)         localStorage (settings) │
│  • datasets/main                 • cec_settings          │
│  • id, headers[], rows[][]       • cec_pin (SHA-256)     │
│  • savedAt                                               │
└──────────────────────────────────────────────────────────┘
           │
           ▼
┌──────────────────────────────────────────────────────────┐
│  External (optional, cached by SW)                       │
│  • Google Sheets CSV (docs.google.com/pub?output=csv)    │
│  • SheetJS CDN  (cdn.sheetjs.com)                        │
│  • Chart.js CDN (cdn.jsdelivr.net)                       │
└──────────────────────────────────────────────────────────┘
```

---

## Components

### `index.html`
Single‑file PWA entry point. Contains all HTML, CSS, and application
JavaScript inline. Tabs: Dashboard · Data · Charts · EVE · Settings.

### `manifest.json`
Web App Manifest enabling "Add to Home Screen" / desktop installation
(PWA). Defines name, icons, theme colours, and display mode.

### `sw.js`
Service Worker implementing three caching strategies:

| Resource type | Strategy |
|---|---|
| App shell (`index.html`, `manifest.json`) | Cache‑first |
| CDN libraries (SheetJS, Chart.js) | Cache‑first with lazy population |
| Google Sheets CSV | Network‑first with cache fallback |

### `Conscripts/` (WinUI 3 Desktop)
C# / WinUI 3 Windows application for managing BAT and PS1 script files
as rich icon cards. See the Visual Studio solution `Conscripts.sln`.

---

## Data Flow

```
User selects source
       │
       ├── Google Sheets URL  → fetch() → CSV text → parseCSV()
       │                          ↕ (cached by SW for offline use)
       ├── Local .csv file    → FileReader → CSV text → parseCSV()
       │
       └── Local .xlsx file   → FileReader → ArrayBuffer
                                    → SheetJS XLSX.read()
                                    → sheet_to_json()
                                         │
                               appData { headers, rows }
                                         │
                         ┌───────────────┼────────────────┐
                         ▼               ▼                ▼
                    DataTable        Charts          Dashboard
                   (HTML table)    (Chart.js)        (preview)
                         │
                         ▼
                    IndexedDB  ←→  Save / Load / Clear
```

---

## Security Model

- **No API keys** committed to the repository.
- **No server‑side authentication** — the app runs entirely client‑side.
- **Optional local PIN** is hashed with `SHA‑256` via the Web Crypto API
  before being stored in `localStorage`. The raw PIN is never persisted.
- **CORS note:** Google Sheets must be published publicly for the CSV
  fetch to succeed. No credentials are sent in the request.

---

## Deployment

### GitHub Pages (recommended)
1. Settings → Pages → Source: `main` branch, root `/`
2. Access at `https://whiteantwan58-tech.github.io/Conscripts/`
   *(URL is case-sensitive; adjust if your repository name differs in casing.)*

### Local development server
```bash
python -m http.server 8000
# or
npx serve .
```

### Flash drive / air‑gapped
Copy the repository folder to a USB drive. Open `index.html` directly
in a browser — no internet connection required after the first load
(CDN resources are cached by the Service Worker).

---

## Author

**Antwan White**  
GitHub: <https://github.com/whiteantwan58-tech>
