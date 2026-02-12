import { ref } from 'vue'

type ShareData = {
  title: string
  text?: string
  url?: string
}

export function useShare() {
  const isSupported = ref(
    typeof navigator !== 'undefined' &&
      typeof navigator.share === 'function' &&
      typeof navigator.canShare === 'function',
  )

  const isSharing = ref(false)
  const error = ref<string | null>(null)

  const share = async ({ title, text, url }: ShareData) => {
    isSharing.value = true
    error.value = null
    const payload: ShareData = { title, text, url }
    try {
      if (!isSupported.value) {
        error.value = 'Web Share API not supported on this device.'
        return false
      }
      if (!navigator?.canShare(payload)) {
        error.value = 'This content cannot be shared.'
        return false
      }
      await navigator.share(payload)
      return true
    } catch (exception) {
      error.value = exception instanceof Error ? exception.message : 'Sharing failed.'
      return false
    } finally {
      isSharing.value = false
    }
  }
  return {
    isSupported,
    isSharing,
    error,
    share,
  }
}
