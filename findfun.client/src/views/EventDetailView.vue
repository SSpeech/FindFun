<template>
  <div class="p-8 max-w-2xl mx-auto">
    <Card>
      <template #title>
        {{ event?.title || 'Event Details' }}
      </template>
      <template #content>
        <img
          v-if="event?.image"
          :src="event.image"
          :alt="event.title"
          class="w-full h-64 object-cover rounded mb-4"
        />
        <ParkInfoPanel
          v-if="event"
          :item="event"
          :fields="eventInfoFields"
          nameKey="title"
          statusKey="status"
          :statusSeverity="statusSeverity"
        >
          <template #actions="{ item }">
            <Button label="Back to Events" icon="pi pi-arrow-left" @click="goBack" outlined />
          </template>
        </ParkInfoPanel>
      </template>
    </Card>
  </div>
</template>

<script setup lang="ts">
import { useRoute, useRouter } from 'vue-router'

const route = useRoute()
const router = useRouter()

// In a real app, fetch from API or store
const events = [
  {
    id: 1,
    title: 'Summer Music Festival',
    date: '2025-07-15',
    status: 'Upcoming',
    image: 'https://via.placeholder.com/400x200?text=Music+Festival',
    description: 'Join us for a day of live music, food, and fun in the sun!',
  },
  {
    id: 2,
    title: 'Art Expo',
    date: '2025-06-30',
    status: 'Upcoming',
    image: 'https://via.placeholder.com/400x200?text=Art+Expo',
    description: 'Explore the latest works from local and international artists.',
  },
  {
    id: 3,
    title: 'Charity Run',
    date: '2025-05-20',
    status: 'Past',
    image: 'https://via.placeholder.com/400x200?text=Charity+Run',
    description: 'Thank you for supporting our annual charity run!',
  },
]

const event = computed(() => {
  const id = Number(route.params.id)
  return events.find((e) => e.id === id)
})

const eventInfoFields = [
  { icon: 'pi pi-calendar text-primary', valueKey: 'date', label: 'Date' },
  { icon: 'pi pi-map-marker text-primary', valueKey: 'location', label: 'Location' },
]

function statusSeverity(status: string | undefined) {
  return status === 'Upcoming' ? 'success' : 'secondary'
}

function goBack() {
  router.push({ name: 'events' })
}
</script>
