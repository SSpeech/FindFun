import { computed } from 'vue'

export interface UseProductImageOptions {
  statusFormatter?: (status: string) => string
  imgStyleFormatter?: (style?: string) => string
}

export function useProductImage(
  props: {
    src: string
    alt: string
    status: string
    imgStyle?: string
  },
  options: UseProductImageOptions = {},
) {
  // Allow DI for formatting status or image style
  const statusText = computed(() =>
    options.statusFormatter ? options.statusFormatter(props.status) : props.status,
  )
  const styleValue = computed(() =>
    options.imgStyleFormatter ? options.imgStyleFormatter(props.imgStyle) : props.imgStyle || '',
  )
  return {
    statusText,
    styleValue,
  }
}
