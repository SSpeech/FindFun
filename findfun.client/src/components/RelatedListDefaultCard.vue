<template>
  <Card :class="cardClass">
    <img :src="item[imageKey]" alt="Related" class="rounded-t-xl h-32 w-full object-cover" loading="lazy" />
    <div class="p-4">
      <h3 class="text-lg font-semibold mb-1">{{ item[nameKey] }}</h3>
      <p class="text-sm text-gray-600 mb-2 flex items-center gap-2">
        <i class="pi pi-map-marker"></i>{{ item[locationKey] }}
      </p>
      <Rating :value="item[ratingKey]" readonly :cancel="false" />
    </div>
    <template #footer>
      <Button
        label="View Details"
        icon="pi pi-eye"
        class="w-full"
        @click="$emit('view-details', item)"
      />
    </template>
  </Card>
</template>

<script setup lang="ts">
import type { ParkRelated } from '@/types/park'

const props = defineProps<{
  item: ParkRelated
  imageKey?: string
  nameKey?: string
  locationKey?: string
  ratingKey?: string
  cardClass?: string
}>()

const imageKey = props.imageKey ?? 'image'
const nameKey = props.nameKey ?? 'name'
const locationKey = props.locationKey ?? 'location'
const ratingKey = props.ratingKey ?? 'rating'
const cardClass = props.cardClass ?? ''

defineEmits(['view-details'])
</script>
