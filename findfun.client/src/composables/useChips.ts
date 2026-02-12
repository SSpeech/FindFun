import { ref, computed, onMounted, type Ref } from 'vue'

export interface UseProductChipsOptions {
  isMobileFn?: () => boolean
  showAllDefault?: boolean
  maxChips?: number
}

export function useProductChips(props: { chips: any[] }, options: UseProductChipsOptions = {}) {
  const showAll = ref(options.showAllDefault ?? false)
  const maxChips = options.maxChips ?? 2

  const displayedChips = computed(() =>
    showAll.value ? props.chips : props.chips.slice(0, maxChips),
  )

  // Allow DI for mobile detection
  const isMobile = ref(options.isMobileFn ? options.isMobileFn() : window.innerWidth < 640)
  onMounted(() => {
    if (!options.isMobileFn) {
      isMobile.value = window.innerWidth < 640
      window.addEventListener('resize', () => {
        isMobile.value = window.innerWidth < 640
      })
    }
  })

  function toggleShowAll() {
    showAll.value = !showAll.value
  }

  return {
    showAll,
    displayedChips,
    isMobile,
    toggleShowAll,
  }
}
