<template>
  <div>
    <label for="venunCloses" id="venunClosesLabel" class="block mb-1 font-semibold">Does it close?</label>
    <SelectButton
      id="venunCloses"
      v-model="local.venunCloses"
      :options="yesNoOptions"
      optionLabel="label"
      optionValue="value"
      required
    />

    <div v-if="local.venunCloses" class="mt-4 py-2 space-y-6">
      <div class="p-3 border rounded-md">
        <div class="flex flex-col sm:flex-row sm:flex-wrap sm:items-end gap-4 py-3">
          <FloatLabel variant="on" class="w-full md:w-20rem">
            <MultiSelect
              id="Select-days"
              v-model="multiDays"
              :maxSelectedLabels="3"
              class="w-full"
              :options="weekOptions"
              optionLabel="label"
              optionValue="value"
              display="comma"
            />
            <label for="Select-days">Select days</label>
          </FloatLabel>
          <InputGroup>
            <InputGroupAddon>
              <i class="pi pi-clock"></i>
            </InputGroupAddon>
            <FloatLabel variant="on">
              <Calendar
                id="Opening-Time"
                v-model="multiOpening"
                showTime
                timeOnly
                hourFormat="24"
                class="text-sm sm:text-base font-medium sm:font-semiboldmb-2"
                aria-label="Opening Time"
              />
              <label for="Opening-Time">Opening Time</label>
            </FloatLabel>
            <FloatLabel variant="on">
              <Calendar
                id="closing-Time"
                v-model="multiClosing"
                showTime
                timeOnly
                hourFormat="24"
                class="text-sm sm:text-base font-medium sm:font-semiboldmb-2"
              />
              <label for="closing-Time">Opening Time</label>
            </FloatLabel>
            <Button
              label="Add"
              @click="addMultiBlock"
              :disabled="!multiDays.length || !multiOpening || !multiClosing"
            />
          </InputGroup>
        </div>
        <div v-for="(entry, index) in local.schedules" :key="index" class="border p-3 rounded-md">
          <div class="flex justify-between items-center mb-2">
            <strong>{{ daysLabel(entry.days) }}</strong>
            <Button
              icon="pi pi-trash"
              @click="removeSchedule(index)"
              class="p-button-text"
              :aria-label="`Delete ${daysLabel(entry.days)}`"
            />
          </div>

          <label for="added-open-time" class="block mb-1">Opening Time</label>
          <Calendar
            id="added-open-time"
            v-model="entry.openingTime"
            showTime
            timeOnly
            hourFormat="24"
            placeholder="Opening Time"
            aria-label="Opening Time"
            class="mb-2"
            readonly
          />

          <label for="closingTime" class="block mb-1">Closing Time</label>
          <Calendar
            v-model="entry.closingTime"
            showTime
            timeOnly
            hourFormat="24"
            placeholder="Closing Time"
            readonly
            id="closingTime"
          />
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import SelectButton from 'primevue/selectbutton'
import MultiSelect from 'primevue/multiselect'
import Calendar from 'primevue/calendar'
import Button from 'primevue/button'

const props = defineProps<{
  modelValue?: {
    venunCloses?: boolean
    schedules?: {
      days: string[]
      openingTime: Date | null
      closingTime: Date | null
    }[]
  }
}>()

const emit = defineEmits<(e: 'update:modelValue', value: typeof props.modelValue) => void>()

const yesNoOptions = [
  { label: 'Yes', value: true },
  { label: 'No', value: false },
]

const weekOptions = [
  { label: 'Monday', value: 'mon' },
  { label: 'Tuesday', value: 'tue' },
  { label: 'Wednesday', value: 'wed' },
  { label: 'Thursday', value: 'thu' },
  { label: 'Friday', value: 'fri' },
  { label: 'Saturday', value: 'sat' },
  { label: 'Sunday', value: 'sun' },
]

const local = reactive({
  venunCloses: props.modelValue?.venunCloses ?? false,
  schedules: props.modelValue?.schedules ? [...props.modelValue.schedules] : [],
})

watch(
  local,
  () => {
    emit('update:modelValue', { ...local })
  },
  { deep: true },
)

function removeSchedule(index: number) {
  local.schedules.splice(index, 1)
}

function daysLabel(days: string[]) {
  const ordered = weekOptions.filter((opt) => days.includes(opt.value))
  if (ordered.length === 1) return ordered[0].label
  const first = ordered[0]?.label
  const last = ordered[ordered.length - 1]?.label
  return `${first} to ${last}`
}

const multiDays = ref<string[]>([])
const multiOpening = ref<Date | null>(null)
const multiClosing = ref<Date | null>(null)

function addMultiBlock() {
  if (!multiDays.value.length || !multiOpening.value || !multiClosing.value) return

  local.schedules.push({
    days: [...multiDays.value],
    openingTime: multiOpening.value,
    closingTime: multiClosing.value,
  })

  multiDays.value = []
  multiOpening.value = null
  multiClosing.value = null
}
</script>
