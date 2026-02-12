<template>
  <div
    class="flex flex-wrap sm:flex-nowrap justify-between items-center w-full overflow-x-auto scrollbar-none"
    :class="[customClass]"
  >
    <div class="flex flex-wrap sm:flex-nowrap items-center gap-2">
      <template v-if="isMobile">
        <Button
          icon="pi pi-filter"
          @click="showFilter = true"
          aria-label="Filter"
          outlined
          size="small"
        />
        <Button
          icon="pi pi-sort-alt"
          @click="showSort = true"
          aria-label="Sort"
          outlined
          size="small"
        />
      </template>

      <template v-else>
        <CascadeSelect
          v-model="cascadeCategoryKey"
          :options="cascadeCategoryOptions"
          optionLabel="label"
          optionGroupLabel="label"
          optionGroupChildren="children"
          placeholder="Filter By"
          showClear
          class="min-w-0 max-w-[50vw] sm:max-w-xs"
        />
        <CascadeSelect
          v-model="cascadeSortKey"
          :options="cascadeSortOptions"
          optionLabel="label"
          optionGroupLabel="label"
          optionGroupChildren="children"
          placeholder="Sort By"
          @change="onSortChange"
          showClear
          class="min-w-0 max-w-[50vw] sm:max-w-xs"
        />
      </template>
    </div>
    <div class="flex items-center gap-2">
      <SelectButton v-model="layoutProxy" :options="layoutOptions" :allowEmpty="false">
        <template #option="{ option }">
          <i :class="[option === 'list' ? 'pi pi-bars' : 'pi pi-table']" class="p-1" />
        </template>
      </SelectButton>
    </div>
    <Dialog v-model:visible="showFilter" modal header="Filter By" class="w-11/12 max-w-xs">
      <CascadeSelect
        v-model="cascadeCategoryKey"
        :options="cascadeCategoryOptions"
        optionLabel="label"
        optionGroupLabel="label"
        optionGroupChildren="children"
        placeholder="Filter By"
        showClear
        class="w-full"
      />
    </Dialog>
    <Dialog v-model:visible="showSort" modal header="Sort By" class="w-11/12 max-w-xs">
      <CascadeSelect
        v-model="cascadeSortKey"
        :options="cascadeSortOptions"
        optionLabel="label"
        optionGroupLabel="label"
        optionGroupChildren="children"
        placeholder="Sort By"
        @change="onSortChange"
        showClear
        class="w-full"
      />
    </Dialog>
  </div>
</template>

<script setup lang="ts">
import { ref, watch } from 'vue'
import { useSectionHeader } from '../composables/useSectionHeader'
import SelectButton from 'primevue/selectbutton'
import Button from 'primevue/button'
import Dialog from 'primevue/dialog'
import CascadeSelect from 'primevue/cascadeselect'

const emit = defineEmits<{
  (e: 'update:sortKey', value: string | number | { label: string; value: unknown } | null): void
  (e: 'update:layout', value: string): void
  (e: 'sort-change', event: unknown): void
  (e: 'update:selectedCategories', value: string[]): void
}>()

const props = withDefaults(
  defineProps<{
    title?: string
    sortKey?: string | number | { label: string; value: unknown } | null
    sortOptions?: { label: string; value: unknown }[]
    layout?: string
    layoutOptions?: string[]
    customClass?: string
    selectedCategories?: string[]
    categoryOptions?: { label: string; value: string }[]
    showTitle?: boolean
  }>(),
  { showTitle: false },
)

const { layoutProxy, onSortChange, isMobile, showFilter, showSort } =
  useSectionHeader(props, emit)
const cascadeCategoryOptions = [
  {
    label: 'Park Type',
    value: 'parkType',
    children: [
      { 
        label: 'National', 
        value: 'national',
        children: [
          { label: 'Ascending', value: { type: 'parkType', value: 'national', order: 'asc' } },
          { label: 'Descending', value: { type: 'parkType', value: 'national', order: 'desc' } },
        ]
      },
      { 
        label: 'Urban', 
        value: 'urban',
        children: [
          { label: 'Ascending', value: { type: 'parkType', value: 'urban', order: 'asc' } },
          { label: 'Descending', value: { type: 'parkType', value: 'urban', order: 'desc' } },
        ]
      },
      { 
        label: 'Nature', 
        value: 'nature',
        children: [
          { label: 'Ascending', value: { type: 'parkType', value: 'nature', order: 'asc' } },
          { label: 'Descending', value: { type: 'parkType', value: 'nature', order: 'desc' } },
        ]
      },
    ],
  },
  {
    label: 'Distance',
    value: 'distance',
    children: [
      { label: '1km', value: { type: 'distance', value: 1, order: 'asc' } },
      { label: '5km', value: { type: 'distance', value: 5, order: 'asc' } },
      { label: '10km', value: { type: 'distance', value: 10, order: 'asc' } },
    ],
  },
  {
    label: 'Rating',
    value: 'rating',
    children: [
      { label: 'Ascending', value: { type: 'rating', order: 'asc' } },
      { label: 'Descending', value: { type: 'rating', order: 'desc' } },
    ],
  },
  {
    label: 'Amenities',
    value: 'amenities',
    children: [
      { label: 'Playground', value: { type: 'amenities', value: ['playground'] } },
      { label: 'Restrooms', value: { type: 'amenities', value: ['restrooms'] } },
      { label: 'Parking', value: { type: 'amenities', value: ['parking'] } },
      { label: 'Picnic Area', value: { type: 'amenities', value: ['picnic_area'] } },
      { label: 'Sports Field', value: { type: 'amenities', value: ['sports_field'] } },
      { label: 'Walking Trail', value: { type: 'amenities', value: ['walking_trail'] } },
    ],
  },
]

const cascadeCategoryKey = ref(null)
watch(cascadeCategoryKey, (newVal) => {
  emit('update:selectedCategories', newVal ? [newVal] : [])
})

// Hierarchical sort options for CascadeSelect
const cascadeSortOptions = [
  {
    label: 'Name',
    value: 'name',
    children: [
      { label: 'A to Z', value: { field: 'name', order: 'asc' } },
      { label: 'Z to A', value: { field: 'name', order: 'desc' } },
    ],
  },
  {
    label: 'Location',
    value: 'location',
    children: [
      { label: 'A to Z', value: { field: 'location', order: 'asc' } },
      { label: 'Z to A', value: { field: 'location', order: 'desc' } },
    ],
  },
  {
    label: 'Municipality',
    value: 'municipality',
    children: [
      { label: 'A to Z', value: { field: 'municipality', order: 'asc' } },
      { label: 'Z to A', value: { field: 'municipality', order: 'desc' } },
    ],
  },
  {
    label: 'Province',
    value: 'province',
    children: [
      { label: 'A to Z', value: { field: 'province', order: 'asc' } },
      { label: 'Z to A', value: { field: 'province', order: 'desc' } },
    ],
  },
]

const cascadeSortKey = ref(null)
watch(cascadeSortKey, (newVal) => {
  emit('update:sortKey', newVal)
})
</script>
