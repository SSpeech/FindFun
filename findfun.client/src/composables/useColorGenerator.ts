/**
 * Composable for generating unique, deterministic colors from strings/icons
 * Uses hash-based color generation to ensure consistent colors across the application
 */

/**
 * Generate a unique, deterministic color for any icon or string using hash-based color generation
 * @param {string} identifier - The icon name or any string to generate a color from
 * @returns {string} A consistent HSL color for the identifier
 */
export const generateColorFromHash = (identifier: string): string =>
{
    // Create a simple hash from the identifier string
    let hash = 0
    for (let i = 0; i < identifier.length; i++)
    {
        hash = ((hash << 5) - hash) + (identifier.codePointAt(i) ?? 0)
        hash = hash & hash // Convert to 32bit integer
    }

    // Convert hash to HSL for better color distribution
    const hue = Math.abs(hash % 360)
    const saturation = 65 + (Math.abs(hash) % 20) // 65-85%
    const lightness = 45 + (Math.abs(hash >> 8) % 20) // 45-65%

    return `hsl(${hue}, ${saturation}%, ${lightness}%)`
}

/**
 * Get a unique color for a weather icon
 * @param {string} icon - The icon name
 * @returns {string} A consistent HSL color for the icon
 */
export const useColorForIcon = (icon: string): string =>
{
    return generateColorFromHash(icon)
}

/**
 * Convert HSL color to HEX (if needed for legacy support)
 * @param {string} hslColor - The HSL color string
 * @returns {string} A HEX color string
 */
export const hslToHex = (hslColor: string): string =>
{
    const match = new RegExp(/hsl\((\d+),\s*(\d+)%,\s*(\d+)%\)/).exec(hslColor)
    if (!match) return '#BDBDBD'

    const h = Number.parseInt(match[1])
    const s = Number.parseInt(match[2])
    let l = Number.parseInt(match[3])

    l /= 100
    const a = (s * Math.min(l, 1 - l)) / 100
    const f = (n: number) =>
    {
        const k = (n + h / 30) % 12
        const color = l - a * Math.max(Math.min(k - 3, 9 - k, 1), -1)
        return Math.round(255 * color)
            .toString(16)
            .padStart(2, '0')
    }
    return `#${f(0)}${f(8)}${f(4)}`.toUpperCase()
}
