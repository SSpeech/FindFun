<template>
  <div class="flex flex-col items-center justify-center min-h-[60vh] p-8 gap-4">
    <h1 class="text-2xl font-bold mb-4 text-center">Add a {{ type }}</h1>
    <div class="mb-6">
      <TabView v-model:activeIndex="activeTabIndex" class="w-full">
        <TabPanel
          v-for="option in typeOptions"
          :key="option.value"
          :header="option.label"
          :value="option.value"
          @click="type = option.value"
        >
          <form @submit.prevent="handleSubmit" class="space-y-4">
            <DynamicFields
              v-for="field in visibleFields"
              :key="field.model"
              :field="field"
              :value="form[field.model]"
              @update:value="form[field.model] = $event"
              :previews="field.model === 'image' ? imagePreviews : []"
              :listeners="field.listeners"
            />
            <Button type="submit" label="Create" class="w-full p-button-primary mt-4" />
          </form>
        </TabPanel>
      </TabView>
    </div>
  </div>
</template>

<script setup lang="ts">
import Button from 'primevue/button'
import { useLocationValidation } from '@/composables/useLocationValidation'
import { useImageUpload } from '@/composables/useImageUpload'
import { useFormFields } from '@/composables/useFormFields'
import { ViewType } from '@/config/Enums'

interface ScheduleEntry {
  days: string[]
  openingTime: Date | null
  closingTime: Date | null
}

interface AmenityBlock {
  items: string[]
  note?: string
}

interface ClosingScheduleData {
  venunCloses: boolean
  schedules: ScheduleEntry[]
}

interface FormData {
  name: string
  description: string
  location: string
  date: unknown
  image: File[]
  organizer: string
  isFree: boolean
  amenities: AmenityBlock[]
  ageRecommendation: string
  closingSchedule: ClosingScheduleData
  [key: string]: unknown
}

const typeOptions = [
  { label: 'Park', value: ViewType.Park },
  { label: 'Event', value: ViewType.Event },
]
const locationInput = ref<HTMLInputElement | null>(null)
const form = ref<FormData>({
  name: '',
  description: '',
  location: '',
  date: null,
  image: [],
  organizer: '',
  isFree: false,
  amenities: [],
  ageRecommendation: '',
  closingSchedule: {
    venunCloses: false,
    schedules: [],
  },
})

const activeTabIndex = ref(0)
const type = computed(() => typeOptions[activeTabIndex.value].value)
const visibleFields = computed(() =>
  useFormFields(type.value).map((field) => ({
    ...field,
    props: {
      ...field.props,
    },
    listeners:
      field.model === 'image'
        ? {
            select: onImageSelect,
            uploader: onImageUpload,
          }
        : ({} as Record<string, () => void>),
  })),
)

const { imagePreviews, onImageSelect, onImageUpload } = useImageUpload(form)
const { placeDetails, validateLocation } = useLocationValidation(form)
async function handleSubmit() {
   alert(`Created ${type.value}: ${form.value.name}`)
  const isValid = await validateLocation()
  if (!isValid) return

  alert(`Created ${type.value}: ${form.value.name}`)

  form.value = {
    name: '',
    description: '',
    location: '',
    date: null,
    image: [],
    organizer: '',
    isFree: false,
    amenities: [],
    ageRecommendation: '',
    closingSchedule: {
      venunCloses: false,
      schedules: [],
    },
  }
  imagePreviews.value = []
  placeDetails.value = {
    placeId: '',
    lat: null,
    lng: null,
    formattedAddress: '',
    street: '',
    streetNumber: '',
    neighborhood: '',
    sublocality: '',
    locality: '',
    district: '',
    province: '',
    region: '',
    country: '',
    postalCode: '',
  }

  if (locationInput.value) {
    locationInput.value.value = ''
  }
}
</script>
