<template>
  <div>
    <template v-if="indicatesAmenities">
      <label for="amenitiesSelector" class="block mb-1 font-semibold">Are there amenities?</label>
      <SelectButton
        id="amenitiesSelector"
        v-model="local.hasAmenities"
        :options="yesNoOptions"
        optionLabel="label"
        optionValue="value"
        required
      />
    </template>

    <div v-if="local.hasAmenities" class="mt-4 space-y-6 mb-2">
      <!-- Add Amenities Block -->
      <div class="p-3 border rounded-md">
        <div class="flex flex-col sm:flex-row sm:flex-wrap sm:items-end gap-4">
          <FloatLabel variant="on">
            <MultiSelect
              v-model="selectedAmenities"
              :options="amenityOptions"
              optionLabel="label"
              optionValue="value"
              class="w-full sm:w-[16rem]"
              :maxSelectedLabels="3"
              display="comma"
              id="Select-amenities"
            />
            <label for="Select-amenities">Add amenities</label>
          </FloatLabel>
          <InputGroup>
            <InputText
              v-model="customAmenity"
              placeholder="Add custom amenity"
              class="w-full sm:w-[16rem]"
              @keyup.enter="addCustomAmenity"
            />
            <Button
              label="Add to list"
              @click="addCustomAmenity"
              :disabled="!customAmenity.trim()"
            />
          </InputGroup>
          <Textarea v-model="amenityNote" placeholder="Optional note" class="w-full" />
          <Button
            label="Add selected Amenity"
            @click="addAmenityBlock"
            :disabled="!selectedAmenities.length"
          />
          <div
            v-for="(entry, index) in local.amenities"
            :key="index"
            class="px-2 py-1 text-sm rounded-md bg-blue-600/25 dark:bg-sky-400/25 dark:text-white-800"
          >
            <InputGroup>
              <div class="flex justify-between items-center">
                <strong>{{ amenityLabel(entry.items) }}</strong>
                <Button icon="pi pi-trash" @click="removeAmenity(index)" class="p-button-text" />
              </div>
              <div v-if="entry.note" class="text-sm text-gray-600 italic">
                {{ entry.note }}
              </div>
            </InputGroup>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import SelectButton from 'primevue/selectbutton'
import MultiSelect from 'primevue/multiselect'
import InputText from 'primevue/inputtext'
import Button from 'primevue/button'

const props = defineProps<{
  modelValue?: {
    hasAmenities?: boolean
    amenities?: {
      items: string[]
      note?: string
    }[]
  }
  indicatesAmenities?: boolean
}>()

const emit = defineEmits<{
  (e: 'update:modelValue', value: typeof props.modelValue): void
}>()

const yesNoOptions = [
  { label: 'Yes', value: true },
  { label: 'No', value: false },
]

const amenityOptions = reactive([
  { label: 'Wi-Fi', value: 'wifi' },
  { label: 'Parking', value: 'parking' },
  { label: 'Restrooms', value: 'restrooms' },
  { label: 'Wheelchair Access', value: 'wheelchair' },
  { label: 'Air Conditioning', value: 'ac' },
  { label: 'Outdoor Seating', value: 'outdoor' },
  { label: 'Pet Friendly', value: 'pets' },
])

const local = reactive({
  hasAmenities: props.modelValue?.hasAmenities ?? true,
  amenities: props.modelValue?.amenities ? [...props.modelValue.amenities] : [],
})

watch(
  local,
  () => {
    emit('update:modelValue', { ...local })
  },
  { deep: true },
)

const selectedAmenities = ref<string[]>([])
const amenityNote = ref('')
const customAmenity = ref('')

function addCustomAmenity() {
  const trimmed = customAmenity.value.trim()
  if (!trimmed) return

  const exists = amenityOptions.some((opt) => opt.label.toLowerCase() === trimmed.toLowerCase())
  if (!exists) {
    const value = trimmed.toLowerCase().replace(/\s+/g, '-')
    amenityOptions.push({ label: trimmed, value })
  }

  selectedAmenities.value.push(
    amenityOptions.find((opt) => opt.label.toLowerCase() === trimmed.toLowerCase())?.value ??
      trimmed,
  )

  customAmenity.value = ''
}

function addAmenityBlock() {
  if (!selectedAmenities.value.length) return

  local.amenities.push({
    items: [...selectedAmenities.value],
    note: amenityNote.value.trim() || undefined,
  })

  selectedAmenities.value = []
  amenityNote.value = ''
}

function removeAmenity(index: number) {
  local.amenities.splice(index, 1)
}

function amenityLabel(items: string[]) {
  const labels = amenityOptions.filter((opt) => items.includes(opt.value)).map((opt) => opt.label)
  return labels.join(', ')
}
</script>
