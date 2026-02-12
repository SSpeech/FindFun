/// <reference lib="webworker" />
declare const self: ServiceWorkerGlobalScope

import { precacheAndRoute, cleanupOutdatedCaches } from 'workbox-precaching'
import { registerRoute } from 'workbox-routing'
import { CacheFirst, StaleWhileRevalidate, NetworkFirst } from 'workbox-strategies'
import { ExpirationPlugin } from 'workbox-expiration'

// Clean up old caches
cleanupOutdatedCaches()

// Precache files
precacheAndRoute(self.__WB_MANIFEST)

// Cache images with CacheFirst strategy
registerRoute(
    ({ request }) => request.destination === 'image',
    new CacheFirst({
        cacheName: 'images',
        plugins: [
            new ExpirationPlugin({
                maxEntries: 60,
                maxAgeSeconds: 30 * 24 * 60 * 60, // 30 days
            }),
        ],
    })
)

// Cache API calls with StaleWhileRevalidate strategy
registerRoute(
    ({ url }) => url.pathname.startsWith('/api/'),
    new StaleWhileRevalidate({
        cacheName: 'api-cache',
        plugins: [
            new ExpirationPlugin({
                maxEntries: 50,
                maxAgeSeconds: 24 * 60 * 60, // 24 hours
            }),
        ],
    })
)

// Cache third-party APIs with NetworkFirst strategy
registerRoute(
    ({ url }) => url.origin !== location.origin,
    new NetworkFirst({
        cacheName: 'cross-origin',
        networkTimeoutSeconds: 5,
        plugins: [
            new ExpirationPlugin({
                maxEntries: 30,
                maxAgeSeconds: 7 * 24 * 60 * 60, // 7 days
            }),
        ],
    })
)

// Handle offline navigation
self.addEventListener('fetch', (event) =>
{
    const { request } = event
    if (request.method !== 'GET')
    {
        return
    }

    if (request.mode === 'navigate')
    {
        event.respondWith(
            fetch(request)
                .then((response) =>
                {
                    if (!response || response.status !== 200 || response.type === 'error')
                    {
                        return response
                    }
                    return response.clone()
                })
                .catch(() =>
                {
                    return caches.match('/index.html').then((response) =>
                    {
                        return response || fetch(request)
                    })
                })
        )
    }
})
