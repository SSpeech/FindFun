import type { AirQualityResponse, WeatherResponse } from '@/types/park'

export async function getWeatherData(
  latitude: number,
  longitude: number,
): Promise<[WeatherResponse, AirQualityResponse]> {
  const paramsAirCore = new URLSearchParams({
    latitude: latitude.toString(),
    longitude: longitude.toString(),
    hourly:
      'pm10,pm2_5,alder_pollen,birch_pollen,grass_pollen,mugwort_pollen,olive_pollen,ragweed_pollen,european_aqi',
  })

  const params = new URLSearchParams({
    latitude: latitude.toString(),
    longitude: longitude.toString(),
    current:
      'temperature_2m,relative_humidity_2m,wind_speed_10m,precipitation,uv_index,cloudcover,rain',
    daily:
      'temperature_2m_max,temperature_2m_min,apparent_temperature_max,apparent_temperature_min,sunrise,sunset,uv_index_max,uv_index_clear_sky_max,wind_speed_10m_max',
  })

  const [coreRes, response] = await Promise.all([
    fetch(`https://air-quality-api.open-meteo.com/v1/air-quality?${paramsAirCore}`),
    fetch(`https://api.open-meteo.com/v1/forecast?${params.toString()}`),
  ])
  if (!response.ok) throw new Error('Failed to fetch weather data')
  return [await response.json(), await coreRes.json()]
}
