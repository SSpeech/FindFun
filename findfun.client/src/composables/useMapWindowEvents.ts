import type { Ref } from 'vue'
import type { Location } from '../types/park.ts'
interface UseMapWindowEventsOptions
{
  map: Ref<HTMLElement | null>
  gmap: Ref<any>
  locationMarkers: Ref<any[]>
  filteredLocations: Location[]
}

export function useMapWindowEvents({
  map,
  gmap,
  locationMarkers,
  filteredLocations,
}: UseMapWindowEventsOptions)
{
  const timeoutIds: ReturnType<typeof setTimeout>[] = [];
  window.scrollToMapMarker = function (productId: string) {
    const el = document.getElementById(productId)
    if (el) {
      el.scrollIntoView({ behavior: 'smooth', block: 'center' })
      el.classList.add('ring-2', 'ring-blue-400')
      timeoutIds.push(setTimeout(() => el.classList.remove('ring-2', 'ring-blue-400'), 1200))
    }
    const mapDiv = map.value
    if (mapDiv) mapDiv.scrollIntoView({ behavior: 'smooth', block: 'center' })

    const idx = Array.isArray(locationMarkers.value)
      ? locationMarkers.value.findIndex((marker) => String((marker).id) === String(productId))
      : -1
    if (idx !== -1 && locationMarkers.value[idx] && gmap.value) {
      const marker = locationMarkers.value[idx]
      gmap.value?.panTo?.(marker?.getPosition?.())
      gmap.value?.setZoom?.(11)
      marker?.setIcon?.({
        url: 'https://maps.google.com/mapfiles/ms/icons/green-dot.png',
        scaledSize: new (window as any).google.maps.Size(40, 40),
      })

      if (!window._markerInfoWindow) window._markerInfoWindow = new (window as any).google.maps.InfoWindow()

      const markerTitle = marker?.getTitle?.() || marker?.getLabel?.() || 'Location'
      window._markerInfoWindow?.setContent?.(`<b style=\"color:black;\">${markerTitle}</b><br/>`)
      window._markerInfoWindow?.open?.(gmap.value, marker)

      timeoutIds.push(setTimeout(() => {
        marker?.setIcon?.(null)
        (window._markerInfoWindow as any)?.close?.()
      }, 5000))

      if (marker?.setAnimation) {
        marker?.setAnimation?.((window as any).google.maps.Animation.BOUNCE)
        timeoutIds.push(setTimeout(() => marker?.setAnimation?.(null), 1200))
      }
    }
  }

  // Cleanup function to clear all timeouts
  return {
    cleanup: () => timeoutIds.forEach(clearTimeout)
  }
}
