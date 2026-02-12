import { computed } from 'vue'

export interface ProductInfoOptions {
  chipsFn?: (props: any) => any[]
  wrapperClassFn?: (props: any) => string
}

export function useProductInfo(props: any, options: ProductInfoOptions = {}) {
  const wrapperClass = computed(() =>
    options.wrapperClassFn
      ? options.wrapperClassFn(props)
      : props.layout === 'list'
        ? 'flex flex-col md:items-end gap-8'
        : 'flex flex-col gap-6 mt-6',
  )

  const chips = computed(() =>
    options.chipsFn
      ? options.chipsFn(props)
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

  return {
    wrapperClass,
    chips,
  }
}
