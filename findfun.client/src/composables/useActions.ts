import { computed, ref, onMounted } from 'vue'

export interface UseProductActionsOptions {
  chipsFn?: () => unknown[]
  wrapperClassFn?: (layout?: string) => string
  buttonWrapperClassFn?: (layout?: string) => string
  cartClassFn?: (layout?: string) => string
  isMobileFn?: () => boolean
}
export function useProductActions(
  props: { price: number; outOfStock: boolean; layout?: string },
  options: UseProductActionsOptions = {},
) {
  // Default layout to 'list' if not provided
  if (!props.layout) props.layout = 'list'
  const reverse = computed(() => props.layout === 'list')
  const wrapperClass = computed(() =>
    options.wrapperClassFn
      ? options.wrapperClassFn(props.layout)
      : props.layout === 'list'
        ? 'flex flex-col md:items-end gap-4'
        : 'flex flex-col gap-6 mt-6',
  )
  const buttonWrapperClass = computed(() =>
    options.buttonWrapperClassFn
      ? options.buttonWrapperClassFn(props.layout)
      : props.layout === 'list'
        ? 'flex flex-row-reverse md:flex-row gap-2'
        : 'flex gap-2',
  )
  const cartClass = computed(() =>
    options.cartClassFn
      ? options.cartClassFn(props.layout)
      : props.layout === 'list'
        ? 'flex-auto md:flex-initial whitespace-nowrap'
        : 'flex-auto whitespace-nowrap',
  )
  const chips = computed(() =>
    options.chipsFn
      ? options.chipsFn()
      : [
          {
            label: 'Amy',
            image: 'https://primefaces.org/cdn/primevue/images/avatar/amyelsner.png',
          },
          {
            label: 'Asiya',
            image: 'https://primefaces.org/cdn/primevue/images/avatar/asiyajavayant.png',
          },
          {
            label: 'Onyama',
            image: 'https://primefaces.org/cdn/primevue/images/avatar/onyamalimba.png',
          },
          {
            label: 'Xuxue Feng',
            image: 'https://primefaces.org/cdn/primevue/images/avatar/xuxuefeng.png',
            removable: true,
          },
        ],
  )
  const isMobile = ref(options.isMobileFn ? options.isMobileFn() : window.innerWidth < 640)
  onMounted(() => {
    if (!options.isMobileFn) {
      isMobile.value = window.innerWidth < 640
      window.addEventListener('resize', () => {
        isMobile.value = window.innerWidth < 640
      })
    }
  })
  return {
    reverse,
    wrapperClass,
    buttonWrapperClass,
    cartClass,
    chips,
    isMobile,
  }
}
