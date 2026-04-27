# CEC‑WAM‑HOT‑CORE‑HEI — Architecture

## Overview

```
┌──────────────────────────────────────────────────────────────────┐
│                Browser / PWA Shell (Holographic UI)              │
│  ┌────────────┐  ┌──────────────┐  ┌────────────────────────┐   │
│  │  Dashboard │  │  Data Panel  │  │  Charts Panel          │   │
│  │ (stats +   │  │ • Sheets CSV │  │ • Bar / Line / Pie     │   │
│  │  preview)  │  │ • Local CSV  │  │ • Doughnut / Polar     │   │
│  └────────────┘  │ • Local XLSX │  │ • Radar                │   │
│  ┌────────────┐  └──────────────┘  └────────────────────────┘   │
│  │ Real-Time  │  ┌──────────────┐  ┌────────────────────────┐   │
│  │ Feeds      │  │ Camera Feed  │  │ EVE HEI Voice          │   │
│  │ • Weather  │  │ • Live Video │  │ • Enhanced AI          │   │
│  │ • News     │  │ • Screenshots│  │ • Speech Recognition   │   │
│  │ • Crime    │  │ • Traffic    │  │ • TTS Integration      │   │
│  │ • Space    │  └──────────────┘  │ • Command Control      │   │
│  └────────────┘  ┌──────────────┐  └────────────────────────┘   │
│  │ Settings   │  │ Sync Status  │                               │
│  │ + About    │  │ • Repository │                               │
│  └────────────┘  │ • LocalHost  │                               │
│                  │ • EVE Node   │                               │
│                  │ • Sovereign  │                               │
│                  └──────────────┘                               │
└──────────────────────────────────────────────────────────────────┘
           │ Service Worker (sw.js) — Cache v2 with HEI
           │ Offline-first cache (Cache API)
           ▼
┌──────────────────────────────────────────────────────────────────┐
│  IndexedDB (persistence)         localStorage (settings)         │
│  • datasets/main                 • cec_settings                  │
│  • id, headers[], rows[][]       • cec_pin (SHA-256)             │
│  • savedAt                       • cec_sync_backup               │
│  • screenshots[]                                                 │
└──────────────────────────────────────────────────────────────────┘
           │
           ▼
┌──────────────────────────────────────────────────────────────────┐
│  External (optional, cached by SW)                               │
│  • Google Sheets CSV (docs.google.com/pub?output=csv)            │
│  • SheetJS CDN  (cdn.sheetjs.com)                                │
│  • Chart.js CDN (cdn.jsdelivr.net)                               │
│  • Real-time data APIs (Weather, News, Crime, Space)             │
│  • MediaDevices API (Camera/Video)                               │
└──────────────────────────────────────────────────────────────────┘
           │
           ▼
┌──────────────────────────────────────────────────────────────────┐
│  Sync Nodes                                                       │
│  • GitHub Repository (whiteantwan58-tech/Conscripts)             │
│  • LocalHost (http://localhost:8000)                             │
│  • EVE Node 1010 (http://localhost:1010)                         │
│  • Sovereign MaxEffort (https://github.com/sovereign-maxeffort)  │
└──────────────────────────────────────────────────────────────────┘
```

---

## Components

### `index.html`
Single‑file PWA entry point with holographic HD visuals. Contains all HTML, CSS, and application
JavaScript inline. Features holographic color gradients, animated UI elements, and real-time data panels.
Tabs: Dashboard · Data · Charts · Real-Time Feeds · Camera Feed · EVE HEI · Settings.

### `manifest.json`
Web App Manifest enabling "Add to Home Screen" / desktop installation
(PWA). Defines name (CEC-WAM-HOT-CORE-HEI), icons, holographic theme colors, and display mode.

### `sw.js`
Service Worker implementing three caching strategies (v2 with HEI):

| Resource type | Strategy |
|---|---|
| App shell (`index.html`, `manifest.json`) | Cache‑first |
| CDN libraries (SheetJS, Chart.js) | Cache‑first with lazy population |
| Google Sheets CSV | Network‑first with cache fallback |
| Real-time APIs | Network‑first (weather, news, crime, space) |

### `Conscripts/` (WinUI 3 Desktop)
C# / WinUI 3 Windows application for managing BAT and PS1 script files
as rich icon cards. See the Visual Studio solution `Conscripts.sln`.

---

## New Features (HEI Update)

### Holographic Visuals
- **HD Color Gradients**: Dynamic holographic color scheme with cyan, magenta, purple, gold, and lime
- **Animated Elements**: Rotating holographic sweep effects, color-shifting headers, gradient borders
- **Enhanced UI**: Glowing cards, animated transitions, holographic status indicators

### Real-Time Data Feeds
- **Weather Updates**: Live weather data for multiple cities with temperature and conditions
- **National News**: Real-time news headlines with timestamps
- **Crime Alerts**: City safety alerts with severity levels (Low, Medium, High)
- **Space Updates**: ISS status, solar activity, GPS satellites, astronomical events
- **Auto-Refresh**: Configurable automatic refresh every 60 seconds
- **Manual Controls**: Refresh all feeds or pause/resume auto-refresh

### Camera Integration
- **Live Video Feed**: Access device cameras for traffic monitoring
- **Camera Selection**: Choose from available video input devices
- **Screenshot Capture**: Capture and save timestamped images
- **Gallery**: View all captured screenshots with timestamps
- **Visual Overlay**: "LIVE" indicator with holographic styling

### EVE HEI (Holographic Enhanced Intelligence)
- **Enhanced Voice Commands**: Control all new features via voice or text
- **Smart Navigation**: Auto-switch to relevant panels based on queries
- **Extended Vocabulary**: Understands weather, news, crime, space, camera, sync commands
- **Intelligence Mode**: Responds to holographic and HEI status queries

### Auto-Sync System
- **Multi-Node Sync**: Repository, LocalHost, EVE Node 1010, Sovereign MaxEffort
- **Status Indicators**: Real-time sync status with visual feedback
- **Backup System**: localStorage-based backup for offline resilience
- **Force Sync**: Manual trigger for immediate synchronization

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
- **Camera Permissions**: MediaDevices API requires user consent for camera access
- **Local-Only Storage**: All screenshots and sync data stored locally in browser
- **Sync Security**: Multi-node sync operates over standard web protocols (HTTP/HTTPS)

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
