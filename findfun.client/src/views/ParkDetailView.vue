<script setup lang="ts">
import { useRoute } from 'vue-router'
import { useParksStore } from '@/stores/data'
import type { Park } from '@/types/park'

const route = useRoute()
const parksStore = useParksStore()
const parkId = computed(() => {
  return route.params.id as string
})
const park = computed<Park>(() => {
  const found = parksStore.parks.find((p: Park) => p.id === parkId.value)
  if (!found) {
    return {} as Park
  }
  return found
})

const images = ref([])
const relatedParks = ref([])
</script>

<template>
  <EventParkDetails
    :parkInfo="park"
    :images="images"
    :reviews="park.reviews"
    :relatedParks="relatedParks"
  />
</template>
