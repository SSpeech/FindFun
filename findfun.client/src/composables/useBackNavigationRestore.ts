export function useBackNavigationRestore(key = 'lastFocusedId') {
  const isBrowser = typeof window !== 'undefined' && 'sessionStorage' in window
  function save(id: string) {
    if (!isBrowser) return
    try {
      sessionStorage.setItem(key, id)
    } catch {}
  }

  async function restore(handler: (id: string) => void | Promise<void>) {
    if (!isBrowser) return
    try {
      const id = sessionStorage.getItem(key)
      if (!id) return
      await handler(id)
      sessionStorage.removeItem(key)
    } catch {}
  }
  return { save, restore }
}
