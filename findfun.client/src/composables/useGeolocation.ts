import { ref } from 'vue'

export function useGeolocation() {
  const isLocationLoading = ref(false)
  const locationError = ref<string | null>(null)

  const getUserLocationWithWatch = (
    onSuccess: (position: GeolocationPosition) => void,
    onError: (error: any) => void,
  ) => {
    isLocationLoading.value = true
    locationError.value = null
    if (!navigator.geolocation) {
      onError('Geolocation is not supported by your browser.')
      isLocationLoading.value = false
      return
    }
    const options = {
      enableHighAccuracy: false,
      timeout: 8000,
      maximumAge: 60000,
    }
    const watchId = navigator.geolocation.watchPosition(
      (position) => {
        onSuccess(position)
        navigator.geolocation.clearWatch(watchId)
        isLocationLoading.value = false
      },
      (error) => {
        onError(error)
        navigator.geolocation.clearWatch(watchId)
        isLocationLoading.value = false
      },
      options,
    )
  }

  const getUserLocationFromIP = async (
    onSuccess: (data: any) => void,
    onError: (error: any) => void,
  ) => {
    isLocationLoading.value = true
    locationError.value = null
    try {
      const res = await fetch('/ipapi/json/')
      if (!res.ok) throw new Error('IP geolocation failed')
      const data = await res.json()
      onSuccess(data)
      isLocationLoading.value = false
    } catch (err: any) {
      onError(err)
      isLocationLoading.value = false
    }
  }

  return {
    isLocationLoading,
    locationError,
    getUserLocationWithWatch,
    getUserLocationFromIP,
  }
}
