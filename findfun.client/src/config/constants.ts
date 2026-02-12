// Centralized application constants and env-based defaults
// Keep non-sensitive defaults here. Secrets must use runtime/CI secrets and .env files.

export const API_BASE_PATH: string = import.meta.env.VITE_API_BASE_URL ?? '/api'

// Default radius used for park searches (units: meters). Can be overridden with VITE_DEFAULT_PARK_RADIUS_METERS
export const DEFAULT_PARK_RADIUS_METERS: number = Number(
  import.meta.env.VITE_DEFAULT_PARK_RADIUS_METERS ?? '2000',
)

export const DEFAULT_PAGE_SIZE: number = Number(import.meta.env.VITE_DEFAULT_PAGE_SIZE ?? '20')
