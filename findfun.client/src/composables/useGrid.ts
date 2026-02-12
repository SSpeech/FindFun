import type { Events, Park } from '@/types/park'
import { ref, computed, type Ref } from 'vue'

export type ProductFilterFn<T = unknown> = (
  products: T[],
  selectedCategories: string[],
  searchTerm?: string,
) => T[]
export type ProductSortFn<T = unknown> = (
  products: T[],
  sortKey: keyof T | undefined,
  sortOrder: 1 | -1 | undefined,
  sortField: keyof T | undefined,
) => T[]

export function useProductGrid(
  products: Ref<Park[] | Events[]>,
  initialItemsPerPage = 6,
  filterFn?: ProductFilterFn,
  sortFn?: ProductSortFn,
) {
  const selectedCategories = ref<string[]>([])
  const sortKey = ref()
  const sortOrder = ref()
  const sortField = ref()
  const sortOptions = ref([
    { label: 'Price High to Low', value: '!name' },
    { label: 'Price Low to High', value: 'name' },
  ])
  const layout = ref('grid')
  const options = ref(['list', 'grid'])
  const itemsPerPage = ref(initialItemsPerPage)
  const page = ref(1)

  const categoryOptions = computed(() => {
    const names = products.value
      .map((p) => (p && typeof (p as any).name === 'string' ? (p as any).name : undefined))
      .filter(Boolean) as string[]
    const cats = Array.from(new Set(names))
    return cats.map((c) => ({ label: c, value: c }))
  })

  // Default filter logic
  const defaultFilter: ProductFilterFn = (prods, cats) => {
    let result = prods
    if (cats.length) {
      result = result.filter((p) =>
        p && typeof (p as any).name === 'string' ? cats.includes((p as any).name) : false,
      )
    }
    return result
  }

  // Default sort logic (no-op)
  const defaultSort: ProductSortFn = (prods) => prods

  const filteredProducts = computed(() => {
    return (filterFn || defaultFilter)(products.value, selectedCategories.value)
  })

  const sortedProducts = computed(() => {
    return (sortFn || defaultSort)(
      filteredProducts.value,
      sortKey.value,
      sortOrder.value,
      sortField.value,
    )
  })

  const totalPages = computed(() => Math.ceil(sortedProducts.value.length / itemsPerPage.value))
  const paginatedProducts = computed(() => {
    const start = (page.value - 1) * itemsPerPage.value
    return sortedProducts.value.slice(start, start + itemsPerPage.value)
  })

  const nextPage = () => {
    if (page.value < totalPages.value) page.value++
  }
  const prevPage = () => {
    if (page.value > 1) page.value--
  }

  function isSortEvent(event: unknown): event is { value: { value: string } } {
    return (
      typeof event === 'object' &&
      event !== null &&
      'value' in event &&
      typeof (event as any).value?.value === 'string'
    )
  }

  const onSortChange = (event: unknown) => {
    if (!isSortEvent(event)) return

    const raw = event.value.value
    const isDescending = raw.startsWith('!')
    const field = isDescending ? raw.slice(1) : raw

    sortOrder.value = isDescending ? -1 : 1
    sortField.value = field
    sortKey.value = event.value
  }

  return {
    selectedCategories,
    sortKey,
    sortOrder,
    sortField,
    sortOptions,
    layout,
    options,
    itemsPerPage,
    page,
    categoryOptions,
    filteredProducts,
    sortedProducts,
    totalPages,
    paginatedProducts,
    nextPage,
    prevPage,
    onSortChange,
  }
}
