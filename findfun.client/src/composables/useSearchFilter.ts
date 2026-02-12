import { ref, watch, toRaw, type Ref } from 'vue'
import { useParksStore, type GetParksParams, type GetEventsParams } from '@/stores/data'

// Map to backend sort field names for parks
const parksSortFieldMap: Record<string, 'name' | 'location' | 'municipality' | 'province'> = {
  name: 'name',
  location: 'location',
  municipality: 'municipality',
  province: 'province',
}

// Map to backend sort field names for events
const eventsSortFieldMap: Record<string, 'title' | 'starttime' | 'endtime' | 'location'> = {
  title: 'title',
  name: 'title',
  location: 'location',
  starttime: 'starttime',
  endtime: 'endtime',
}

function addSortToParams(
  params: GetParksParams | GetEventsParams,
  sortKey?: string | null | { label?: string; value?: string },
  sortOrder?: 1 | -1,
  isEvent = false,
) {
  if (sortKey && typeof sortKey === 'object') {
    const rawData = toRaw(sortKey)

    if (rawData.value && typeof rawData.value === 'object') {
      const map = isEvent ? eventsSortFieldMap : parksSortFieldMap
      const mapped = map[rawData.value.field as keyof typeof map]

      if (mapped) {
        params.sortBy = mapped
        params.sortDirection = rawData.value.order
        console.log('Params after cascade sort:', params)
      }
      return
    }
  }

  let extracted: unknown = sortKey

  if (
    extracted &&
    typeof extracted === 'object' &&
    'value' in extracted &&
    !Array.isArray(extracted)
  ) {
    extracted = (extracted as { value?: unknown }).value
    if (
      extracted &&
      typeof extracted === 'object' &&
      'value' in extracted &&
      !Array.isArray(extracted)
    ) {
      extracted = (extracted as { value?: unknown }).value
    }
  }

  if (!extracted || typeof extracted !== 'string') {
    return
  }

  const map = isEvent ? eventsSortFieldMap : parksSortFieldMap
  const mapped = map[extracted.toLowerCase().trim() as keyof typeof map]

  if (mapped) {
    params.sortBy = mapped
    params.sortDirection = sortOrder === 1 ? 'asc' : 'desc'
  }
}

// Process cascade filter data and add to params
function addCascadeFilterToParams(params: GetParksParams | GetEventsParams, filterData: any) {
  if (!filterData || typeof filterData !== 'object') return

  const rawData = toRaw(filterData)
  const { value } = rawData
  console.log('Params after cascade filter:', value)
  switch (value.type) {
    case 'parkType':
      ;(params as any).parkType = value.type
      if (value.order) params.sortDirection = value.order
      break
    case 'distance':
      ;(params as any).radiusKm = value.distance
      if (value.order) params.sortDirection = value.order
      break
    case 'rating':
      ;(params as any).sortBy = 'rating'
      if (value.order) params.sortDirection = value.order
      break
    case 'amenities':
      if (Array.isArray(value.amenities) && value.amenities.length > 0) {
        ;(params as any).amenities = value.amenities
      }
      break
  }
}

interface UseSearchFilterOptions {
  isViewingEvents: Ref<boolean>
  searchTerm: Ref<string>
  sortKey: Ref<any>
  sortOrder: Ref<1 | -1>
  selectedFilters: Ref<any>
  dateRange?: Ref<Array<Date | null>>
  upcomingOnly?: Ref<boolean>
  ongoingOnly?: Ref<boolean>
  pageSize?: number
}

export function useSearchFilter(options: UseSearchFilterOptions) {
  const {
    isViewingEvents,
    searchTerm,
    sortKey,
    sortOrder,
    selectedFilters,
    dateRange,
    upcomingOnly,
    ongoingOnly,
    pageSize = 6,
  } = options

  const page = ref(1)

  // Trigger API call when filters, sort, or pagination changes
  const applyFiltersAndSort = async () => {
    // Build params from reactive sources and delegate to shared executor
    if (isViewingEvents.value) {
      const params: GetEventsParams = {
        page: page.value,
        pageSize,
      }

      if (searchTerm.value.trim()) params.search = searchTerm.value.trim()

      addSortToParams(params, sortKey.value, sortOrder.value, true)
      if (dateRange?.value?.[0]) params.startDate = dateRange.value[0].toISOString().split('T')[0]
      if (dateRange?.value?.[1]) params.endDate = dateRange.value[1].toISOString().split('T')[0]
      if (upcomingOnly?.value) params.upcomingOnly = true
      if (ongoingOnly?.value) params.ongoingOnly = true

      await executeSearch(true, params)
    } else {
      const params: GetParksParams = {
        page: page.value,
        pageSize,
      }

      if (searchTerm.value.trim()) params.search = searchTerm.value.trim()

      addSortToParams(params, sortKey.value, sortOrder.value, false)

      // Process cascade filter if provided
      if (selectedFilters.value) {
        addCascadeFilterToParams(params, selectedFilters.value)
      }

      await executeSearch(false, params)
    }
  }

  // Watch for changes and trigger API calls
  watch(
    [
      sortKey,
      sortOrder,
      selectedFilters,
      ...(dateRange ? [dateRange] : []),
      ...(upcomingOnly ? [upcomingOnly] : []),
      ...(ongoingOnly ? [ongoingOnly] : []),
    ],
    () => {
      page.value = 1
      applyFiltersAndSort()
    },
    { deep: true },
  )

  watch(page, () => {
    applyFiltersAndSort()
  })

  const nextPage = (totalPages: number) => {
    if (page.value < totalPages) {
      page.value++
    }
  }

  const prevPage = () => {
    if (page.value > 1) {
      page.value--
    }
  }

  return {
    page,
    applyFiltersAndSort,
    nextPage,
    prevPage,
  }
}

export interface TriggerSearchOptions {
  isViewingEvents?: boolean
  sortKey?: string | null
  sortOrder?: 1 | -1
  pageSize?: number
}

export function triggerSearch(searchTerm: string, opts: TriggerSearchOptions = {}) {
  const { isViewingEvents = false, sortKey, sortOrder, pageSize = 6 } = opts
  const params: GetEventsParams | GetParksParams = isViewingEvents
    ? ({ page: 1, pageSize, search: searchTerm.trim() || '' } as GetEventsParams)
    : ({ page: 1, pageSize, search: searchTerm.trim() || '' } as GetParksParams)

  addSortToParams(params, sortKey ?? null, sortOrder ?? 1, isViewingEvents)
  executeSearch(isViewingEvents, params)
}

export async function executeSearch(
  isViewingEvents: boolean,
  params: GetParksParams | GetEventsParams,
) {
  const parksStore = useParksStore()
  if (isViewingEvents) {
    await parksStore.fetchEvents(params as GetEventsParams)
  } else {
    await parksStore.fetchParks(params as GetParksParams)
  }
}
