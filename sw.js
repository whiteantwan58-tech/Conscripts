/* ════════════════════════════════════════════════════════════════
   CEC‑WAM‑HOT‑CORE — Service Worker
   Offline‑First: cache-first for assets, network-first for CSV.
   ════════════════════════════════════════════════════════════════ */

'use strict';

const CACHE_NAME    = 'cec-wam-holo-v2';
const CDN_CACHE     = 'cec-wam-cdn-v2';

/* Assets to pre-cache on install */
const PRECACHE_URLS = [
  './',
  './index.html',
  './manifest.json',
];

/* CDN resources to cache lazily */
const CDN_HOSTS = [
  'cdn.sheetjs.com',
  'cdn.jsdelivr.net',
];

/* ── Install: pre‑cache shell ─────────────────────────────────── */
self.addEventListener('install', event => {
  event.waitUntil(
    caches.open(CACHE_NAME)
      .then(cache => cache.addAll(PRECACHE_URLS))
      .then(() => self.skipWaiting())
  );
});

/* ── Activate: remove old caches ─────────────────────────────── */
self.addEventListener('activate', event => {
  event.waitUntil(
    caches.keys().then(keys =>
      Promise.all(
        keys
          .filter(k => k !== CACHE_NAME && k !== CDN_CACHE)
          .map(k => caches.delete(k))
      )
    ).then(() => self.clients.claim())
  );
});

/* ── Fetch: offline‑first strategy ───────────────────────────── */
self.addEventListener('fetch', event => {
  const url = new URL(event.request.url);

  /* CDN resources: cache‑first, fall back to network, then cache */
  if (CDN_HOSTS.includes(url.hostname)) {
    event.respondWith(
      caches.open(CDN_CACHE).then(cache =>
        cache.match(event.request).then(cached => {
          if (cached) return cached;
          return fetch(event.request)
            .then(res => {
              if (res && res.status === 200) cache.put(event.request, res.clone());
              return res;
            })
            .catch(() => new Response(
              `/* CDN unavailable offline: ${url.hostname} — load the app while online first so this resource is cached. */`,
              { status: 503, headers: { 'Content-Type': 'application/javascript' } }
            ));
        })
      )
    );
    return;
  }

  /* Live Google Sheets CSV: network‑first, fall back to cache */
  if (url.hostname === 'docs.google.com' && url.pathname.includes('/pub')) {
    event.respondWith(
      fetch(event.request)
        .then(res => {
          if (res && res.status === 200) {
            caches.open(CACHE_NAME).then(c => c.put(event.request, res.clone()));
          }
          return res;
        })
        .catch(() => caches.match(event.request))
    );
    return;
  }

  /* App shell & local assets: cache‑first */
  if (url.origin === self.location.origin) {
    event.respondWith(
      caches.match(event.request).then(cached =>
        cached || fetch(event.request).then(res => {
          if (res && res.status === 200) {
            caches.open(CACHE_NAME).then(c => c.put(event.request, res.clone()));
          }
          return res;
        })
      )
    );
  }
});
