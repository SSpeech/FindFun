import { computed } from 'vue'
import { useWeatherStore, getWeatherIconColor, mapAirQualityDisplay } from '@/stores/weatherStore'
import type { AirQualityResponse, Park } from '@/types/park'

const airQualityKeys = [
  'pm10',
  'pm2_5',
  'alder_pollen',
  'birch_pollen',
  'grass_pollen',
  'mugwort_pollen',
  'olive_pollen',
  'ragweed_pollen',
  'european_aqi',
] as const

type AirQualityKey = (typeof airQualityKeys)[number]

export function useParkWeatherAirQuality(parkInfo: Park) {
  const weatherStore = useWeatherStore()
  const parkInfoRef = computed(() => ({
    id: parkInfo.id,
    latitude: parkInfo.latitude,
    longitude: parkInfo.longitude,
  }))
  const weather = weatherStore.useWeatherInfo(parkInfoRef)

  const weatherIconClass = computed(() =>
    weatherStore.getWeatherIcon(
      weather.value?.weatherResponse?.current,
      weather.value?.sunrise,
      weather.value?.sunset,
    ),
  )

  const airQualityData = computed(() => {
    const aq: AirQualityResponse | undefined = weather.value?.airQualityResponse
    if (!aq) return []
    const idx = aq.hourly.time.length - 1
    return airQualityKeys.map((key) => {
      const value =
        (aq.hourly as Record<AirQualityKey, (number | null)[] | undefined>)[key]?.[idx] ?? null
      return mapAirQualityDisplay(key, value)
    })
  })

  const mainAirMetric = computed(() =>
    airQualityData.value.find((aq) => aq.name === 'european_aqi'),
  )
  const otherAirMetrics = computed(() =>
    airQualityData.value.filter((aq) => aq.name !== 'european_aqi'),
  )

  return {
    weather,
    weatherIconClass,
    airQualityData,
    mainAirMetric,
    otherAirMetrics,
    getWeatherIconColor,
  }
}
