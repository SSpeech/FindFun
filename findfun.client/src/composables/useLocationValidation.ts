import { ref } from 'vue'

export function useLocationValidation(form: any) {
  const placeDetails = ref({
    placeId: '',
    lat: null,
    lng: null,
    formattedAddress: '',
    street: '',
    streetNumber: '',
    neighborhood: '',
    sublocality: '',
    locality: '',
    district: '',
    province: '',
    region: '',
    country: '',
    postalCode: '',
  })

  function extractAdminDetails(components: any[]) {
    const get = (type: string) => components.find((c) => c.types.includes(type))?.long_name || ''
    return {
      street: get('route'),
      streetNumber: get('street_number'),
      neighborhood: get('neighborhood'),
      sublocality: get('sublocality'),
      locality: get('locality'),
      district: get('administrative_area_level_3'),
      province: get('administrative_area_level_2'),
      region: get('administrative_area_level_1'),
      country: get('country'),
      postalCode: get('postal_code'),
    }
  }

  function waitForGoogleMaps(): Promise<void> {
    let timeoutId: ReturnType<typeof setTimeout> | null = null
    const cleanup = () => {
      if (timeoutId) clearTimeout(timeoutId)
    }
    const promise: Promise<void> = new Promise((resolve) => {
      const check = () => {
        if ((window as any).google?.maps?.Geocoder) resolve(undefined)
        else timeoutId = setTimeout(check, 100)
      }
      check()
    })
    ;(promise as any).cleanup = cleanup
    return promise
  }

  async function validateLocation(): Promise<boolean> {
    const input = form.value.location.trim()
    if (!input) return false

    await waitForGoogleMaps()
    const geocoder = new (window as any).google.maps.Geocoder()

    return new Promise((resolve) => {
      geocoder.geocode({ address: input }, (results: any, status: any) => {
        if (status === 'OK' && results.length > 0) {
          const result = results[0]
          const components = result.address_components
          const geometry = result.geometry.location

          const admin = extractAdminDetails(components)
          placeDetails.value = {
            placeId: result.place_id || '',
            lat: geometry.lat(),
            lng: geometry.lng(),
            formattedAddress: result.formatted_address || '',
            ...admin,
          }

          form.value.location = result.formatted_address || ''
          resolve(true)
        } else {
          alert('Invalid location. Please enter a valid place.')
          resolve(false)
        }
      })
    })
  }

  return { placeDetails, validateLocation }
}
