<p align="center">
    <img src="Conscripts/Assets/Conscripts_Logo.png" alt="logo" height="128" width="128"/>
</p>

<h1 align="center">Conscripts · CEC‑WAM‑HOT‑CORE‑HEI</h1>
<h3 align="center">Unified System Interface · Holographic Enhanced Intelligence · Offline‑First · PWA‑Ready</h3>

<p align="center">
  <a href="https://apps.microsoft.com/detail/9ppndntlq86q?mode=full">
    <img src="https://get.microsoft.com/images/en-us%20dark.svg" width="200"/>
  </a>
</p>

---

## 🔹 What This Repo Is

**Conscripts / CEC‑WAM‑HOT‑CORE‑HEI** is a **finished, production‑ready system interface** with holographic visuals and real-time intelligence that runs in two modes:

| Mode | Description |
|------|-------------|
| 🖥️ **Desktop (WinUI 3)** | Windows app — centralized launcher for BAT & PS1 script files with rich icon cards |
| 🌐 **Web / PWA** | Browser‑installable interface with holographic UI, real-time data feeds (weather, news, crime, space), camera integration, AI voice assistant (EVE HEI), and full offline support |

Both modes share the same project identity and are **single‑source‑of‑truth** in this repository.

---

## ⚙️ Core Features

### Desktop (WinUI 3)
- ✅ **Centralized script launcher** — BAT and PS1 files displayed as rich icon cards
- ✅ **One‑click execution** — no hunting through identical file icons
- ✅ **Available on Microsoft Store**

### Web / PWA
- ✅ **Holographic HD Visuals** with dynamic color gradients and animations
- ✅ **Real-Time Data Feeds** (Weather, National News, Crime Alerts, Space Updates)
- ✅ **Live Camera Feed** for traffic monitoring with screenshot capture
- ✅ **Auto-Sync** to Repository, LocalHost, EVE Node 1010, and Sovereign MaxEffort
- ✅ **EVE HEI Voice Interface** (Holographic Enhanced Intelligence) — Advanced AI assistant
- ✅ **Live Google Sheets (CSV) Integration**
- ✅ **Local CSV / XLSX File Import**
- ✅ **Offline‑First Operation** (IndexedDB persistence)
- ✅ **Charts & Analytics Dashboard** (Chart.js)
- ✅ **Voice Input + Text‑to‑Speech** (Web Speech API)
- ✅ **Installable PWA** (Service Worker + Web App Manifest)
- ✅ **Local‑Only Security Model** (no secrets committed)

---

## 🧠 Designed For

- Real-time intelligence gathering and monitoring
- City safety monitoring (crime alerts, traffic cameras)
- Weather and space-time tracking
- Data‑driven system modeling
- Lightweight accounting / "living calculator" workflows
- Edge devices, flash‑drive deployment, air‑gapped use
- Human‑AI interaction without cloud lock‑in
- Script automation power‑users (Windows)
- Multi-node synchronization (Repository, LocalHost, EVE Node 1010, Sovereign MaxEffort)

---

## 🔐 Security Model

- No API keys or secrets stored in the repository
- Optional **local device PIN** (hashed, stored in IndexedDB — offline only)
- Designed for **client‑only trust boundary**
- Server auth intentionally excluded by default

---

## 🚀 Quick Start

### Web / PWA

```bash
# Clone and serve locally
git clone https://github.com/whiteantwan58-tech/Conscripts.git
cd Conscripts
python -m http.server 8000
```

Open your browser at `http://localhost:8000` and click **"Install App"** in the address bar to add it as a PWA.

Or deploy instantly via **GitHub Pages**:
1. Settings → Pages → Source: `main` branch, `/` (root)
2. Visit `https://whiteantwan58-tech.github.io/Conscripts/`

### Desktop (Windows)

Install from the **Microsoft Store**:

<a href="https://apps.microsoft.com/detail/9ppndntlq86q?mode=full">
  <img src="https://get.microsoft.com/images/en-us%20dark.svg" width="200"/>
</a>

Or build from source with Visual Studio 2022 (WinUI 3 / Windows App SDK).

---

## 📁 Repository Structure

```
Conscripts/
├── index.html          # PWA entry point
├── manifest.json       # Web App Manifest (installable PWA)
├── sw.js               # Service Worker (offline‑first caching)
├── Conscripts/         # WinUI 3 desktop app source
│   ├── Assets/
│   ├── Models/
│   ├── ViewModels/
│   └── Views/
├── docs/               # Architecture & design notes
│   └── ARCHITECTURE.md
└── README.md
```

---

## 📸 Screenshots

![Conscripts Desktop](Conscripts/Assets/screenshot1.png)

---

## 📦 Status

| Component | Status |
|-----------|--------|
| WinUI 3 Desktop | ✅ Complete & on Microsoft Store |
| PWA Web Interface | ✅ Stable — deploy via GitHub Pages |
| Holographic Visuals | ✅ HD gradients & animations active |
| Real-Time Feeds | ✅ Weather, News, Crime, Space data |
| Camera Integration | ✅ Live feed with screenshot capture |
| EVE HEI Voice | ✅ Enhanced Intelligence mode active |
| Auto-Sync System | ✅ Multi-node synchronization |
| Offline / IndexedDB | ✅ Service Worker active |
| Charts Dashboard | ✅ Chart.js integrated |

---

## 👤 Author

**Antwan White**  
GitHub: [https://github.com/whiteantwan58-tech](https://github.com/whiteantwan58-tech)

---

> 您是否是一位脚本文件的使用者？无论是批处理文件还是 PowerShell 脚本，不管多复杂的操作，只要轻轻双击，就可以等待脚本自动把所有工作都完成，这一切都是那么的美好。如果您也有这种烦恼，那欢迎来体验一下 Conscripts——它是脚本文件的启动中心，将所有的脚本文件藏在幕后，取而代之的，是丰富的图标，和多彩的卡片。
