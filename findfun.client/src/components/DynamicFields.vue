<template>
  <div class="mb-4">
    <label v-if="field.label" :for="field.label" class="block mb-1 mt-2 font-semibold">{{
      field.label
    }}</label>
    <component
      :is="field.component"
      :modelValue="value"
      @update:modelValue="updateValue"
      v-bind="field.props"
      v-on="listeners"
      class="w-full"
      :id="field.label"
    />
    <div v-if="field.model === 'image' && previews?.length" class="flex flex-wrap gap-2 mt-2">
      <img
        v-for="(src, idx) in previews"
        :key="idx"
        :src="src"
        class="h-20 rounded shadow border"
        alt=""
        loading="lazy"
      />
    </div>
  </div>
</template>

<script setup lang="ts">
import type { FormField } from '@/config/TabsConfig'

defineProps<{
  field: FormField
  value: unknown
  previews?: string[]
  listeners?: Record<string, (event: unknown) => void>
}>()

const emit = defineEmits(['update:value'])

function updateValue(newValue: unknown) {
  emit('update:value', newValue)
}
</script>
