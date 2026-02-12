<script setup lang="ts">
import { useRoute } from 'vue-router'
import { useParksStore } from '@/stores/data'
import type { Events, Park } from '@/types/park'
import { Routes } from '@/config/Enums'

const route = useRoute()
const parksStore = useParksStore()

const parkId = computed(() => {
  const id = route.params.id as string
  const name = route.name as string
  return [id, name]
})

const [id, name] = parkId.value
const isPark = name == Routes.ParkDetail

// onMounted(async () => {
//   if (isPark) {
//     await parksStore.fetchParkById(id);
//   } else {
//     await parksStore.fetchEventById(id);
//   }
// });
watch(
  [() => id, () => isPark],
  async ([newId, newIsPark]) => {
    if (newIsPark) {
      await parksStore.fetchParkById(newId)
    } else {
      await parksStore.fetchEventById(newId)
    }
  },
  { immediate: true },
)
// have to meake sure no property missing when it event
const park = computed<Park>(() => {
  const found = isPark
    ? parksStore.parks.find((p: Park) => p.id === id)
    : parksStore.events.find((p: Events) => p.id === id)
  return found ?? ({} as Park)
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
    :isPark="isPark"
  />
</template>
