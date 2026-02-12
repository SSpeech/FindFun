// Open-Meteo Air Quality API response type
export type AirQualityResponse = {
  latitude: number
  longitude: number
  generationtime_ms: number
  utc_offset_seconds: number
  timezone: string
  timezone_abbreviation: string
  elevation: number
  hourly_units: {
    time: string
    pm10: string
    pm2_5: string
    carbon_monoxide?: string
    nitrogen_dioxide?: string
    sulphur_dioxide?: string
    ozone?: string
    european_aqi?: string
    alder_pollen?: string
    birch_pollen?: string
    grass_pollen?: string
    mugwort_pollen?: string
    olive_pollen?: string
    ragweed_pollen?: string
  }
  hourly: {
    time: string[]
    pm10: number[]
    pm2_5: number[]
    carbon_monoxide?: (number | null)[]
    nitrogen_dioxide?: (number | null)[]
    sulphur_dioxide?: (number | null)[]
    ozone?: (number | null)[]
    european_aqi?: (number | null)[]
    alder_pollen?: (number | null)[]
    birch_pollen?: (number | null)[]
    grass_pollen?: (number | null)[]
    mugwort_pollen?: (number | null)[]
    olive_pollen?: (number | null)[]
    ragweed_pollen?: (number | null)[]
  }
}
// WeatherInfo type for use across the app
export type WeatherInfo = {
  temperature: number | null
  sunrise?: string
  sunset?: string
  currentTime?: string
  weatherResponse?: WeatherResponse
  airQualityResponse?: AirQualityResponse
}

export type Location = {
  id: string | number
  latitude: number
  longitude: number
  title?: string
  name?: string
}
// WeatherResponse type for Open-Meteo API
export type WeatherResponse = {
  latitude: number
  longitude: number
  generationtime_ms: number
  utc_offset_seconds: number
  timezone: string
  timezone_abbreviation: string
  elevation: number
  current_units: {
    time: string
    interval: string
    temperature_2m: string
    relative_humidity_2m: string
    wind_speed_10m: string
    precipitation: string
    uv_index: string
    cloudcover: string
    rain: string
  }
  current: {
    time: string
    interval: number
    temperature_2m: number
    relative_humidity_2m: number
    wind_speed_10m: number
    precipitation: number
    uv_index: number
    cloudcover: number
    rain: number
  }
  daily_units: {
    time: string
    temperature_2m_max: string
    temperature_2m_min: string
    apparent_temperature_max: string
    apparent_temperature_min: string
    sunrise: string
    sunset: string
    uv_index_max: string
    uv_index_clear_sky_max: string
    wind_speed_10m_max: string
  }
  daily: {
    time: string[]
    temperature_2m_max: number[]
    temperature_2m_min: number[]
    apparent_temperature_max: number[]
    apparent_temperature_min: number[]
    sunrise: string[]
    sunset: string[]
    uv_index_max: number[]
    uv_index_clear_sky_max: number[]
    wind_speed_10m_max: number[]
  }
}
export interface ParkInfo {
  name: string
  status?: { text: string; severity: string } | null
  rating?: number | null
  description?: string
  location?: string
  infoRows?: Array<ParkInfoRow>
}
export type Items = Array<Events | Park>

export interface Events extends Park {
  startTime: string
  endTime: string
  eventType: string
}

export interface Park {
  id: string
  name: string
  description: string
  locationId: string
  locationName: string
  latitude: number
  longitude: number
  municipalityName: string
  provinceName: string
  autonomousCommunityName: string
  reviews: Array<Review>
  amenities: Array<string>
  parkType: string
  parkImages: Array<string>
  street: string
  averageRating: number
}

export interface ParkInfoRow {
  icon: string
  label: string
  value?: string
  iconColor?: string
  tag?: { text: string; severity: string }
}

export interface ParkImage {
  itemImageSrc: string
  thumbnailImageSrc: string
  alt?: string
}

export interface Review {
  id: string
  userName: string
  content: string
  rating: number
  createdAt: string
}
export interface ParkRelated {
  image: string
  name: string
  location: string
  rating: number
  [key: string]: string | number | Array | null
}

export type SearchableField = (typeof SEARCHABLE_FIELDS)[number]
export type FilterableField = (typeof FILTERABLE_FIELDS)[number]
