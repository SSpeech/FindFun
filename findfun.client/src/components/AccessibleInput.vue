<template>
  <div class="flex flex-col gap-1">
    <label v-if="label" :for="computedId" class="text-sm text-gray-700 dark:text-gray-200">
      {{ label }}<span v-if="required" aria-hidden="true">*</span>
    </label>
    <input
      :id="computedId"
      :type="type"
      :placeholder="placeholder"
      :value="modelValue"
      @input="$emit('update:modelValue', ($event.target as HTMLInputElement).value)"
      :required="required"
      :aria-required="required ? 'true' : undefined"
      :aria-invalid="error ? 'true' : 'false'"
      :aria-describedby="error ? computedId + '-error' : undefined"
      class="rounded border border-gray-300 dark:border-gray-700 bg-white dark:bg-gray-900 text-gray-900 dark:text-gray-100 px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-400"
    />
    <p v-if="error" :id="computedId + '-error'" class="text-xs text-red-500 mt-1">{{ error }}</p>
  </div>
</template>

<script setup lang="ts">
import { computed } from 'vue'

const props = defineProps<{
  modelValue?: string
  label?: string
  id?: string
  type?: string
  placeholder?: string
  required?: boolean
  error?: string
}>()

defineEmits(['update:modelValue'])

const computedId = computed(
  () => props.id ?? `input-${crypto.randomUUID()}`,
)
</script>