import type { FilterableField } from '@/types/park'

export function useDynamicFilterSort<T extends Record<string, unknown>>(
  items: Ref<T[]>,
  selectedValues: Ref<unknown[]>,
  filterFields: readonly FilterableField[],
  sortOrder: Ref<1 | -1>,
  sortField: Ref<keyof T | null>,
)
{
  const filtered = computed(() =>
  {
    const values = selectedValues.value
    if (!values.length) return items.value

    return items.value.filter((item) => filterFields.some((field) => values.includes(item[field])))
  })

  const sorted = computed(() =>
  {
    const field = sortField.value
    if (!field) return filtered.value

    return filtered.value.slice().sort((a, b) =>
    {
      const aVal = a[field]
      const bVal = b[field]

      if (aVal == null || bVal == null) return 0
      if (typeof aVal === 'string' && typeof bVal === 'string')
      {
        return sortOrder.value * aVal.localeCompare(bVal)
      }
      if (typeof aVal === 'number' && typeof bVal === 'number')
      {
        return sortOrder.value * (aVal - bVal)
      }
      return 0
    })
  })

  return { filtered, sorted }
}
