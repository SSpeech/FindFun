import { computed } from 'vue'

export interface UseProductStatusTagOptions {
  severityFn?: (status: string) => string | null
}

export function useProductStatusTag(
  props: { status: string },
  options: UseProductStatusTagOptions = {},
) {
  const getSeverity = (status: string) => {
    if (options.severityFn) return options.severityFn(status)
    switch (status) {
      case 'INSTOCK':
        return 'success'
      case 'LOWSTOCK':
        return 'warn'
      case 'OUTOFSTOCK':
        return 'danger'
      default:
        return null
    }
  }
  const severity = computed(() => getSeverity(props.status))
  return { severity }
}
