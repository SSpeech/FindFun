import type { Ref } from 'vue'
import { ref, onMounted } from 'vue'

export interface ServiceWorkerState
{
    isSupported: boolean
    isRegistered: Ref<boolean>
    registration: Ref<ServiceWorkerRegistration | null>
    error: Ref<Error | null>
}

/**
 * Composable for managing service worker registration and lifecycle
 * Handles PWA functionality including offline support and caching
 */
export function useServiceWorker(): ServiceWorkerState
{
    const isSupported = 'serviceWorker' in navigator
    const isRegistered = ref(false)
    const registration = ref<ServiceWorkerRegistration | null>(null)
    const error = ref<Error | null>(null)

    const registerServiceWorker = async (): Promise<void> =>
    {
        if (!isSupported)
        {
            return
        }

        try
        {
            const reg = await navigator.serviceWorker.register('/sw.js', {
                scope: '/',
            })
            registration.value = reg
            isRegistered.value = true

            // Handle service worker updates
            reg.addEventListener('updatefound', () =>
            {
                const newWorker = reg.installing
                if (newWorker)
                {
                    newWorker.addEventListener('statechange', () =>
                    {
                        if (
                            newWorker.state === 'installed' &&
                            navigator.serviceWorker.controller
                        )
                        {
                            // Optionally notify user about update
                        }
                    })
                }
            })
        }
        catch (err)
        {
            const serviceWorkerError
                = err instanceof Error
                    ? err
                    : new Error('Failed to register service worker')
            error.value = serviceWorkerError
        }
    }

    onMounted(() =>
    {
        registerServiceWorker()
    })

    return {
        isSupported,
        isRegistered,
        registration,
        error,
    }
}
