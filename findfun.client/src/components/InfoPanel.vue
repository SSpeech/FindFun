<template>
  <div class="flex flex-col justify-between gap-6">
    <div>
      <h1 class="text-3xl font-bold mb-2">{{ item[nameKey] }}</h1>
      <Tag
        v-if="statusKey"
        :value="item[statusKey]"
        :severity="statusSeverity ? statusSeverity(item[statusKey], item) : 'info'"
        class="mb-3"
      />
      <div class="flex flex-col gap-2 text-gray-700">
        <template v-for="field in fields" :key="field.valueKey">
          <p>
            <i :class="field.icon"></i>
            <span>
              <template v-if="field.format">
                {{ field.format(item[field.valueKey], item) }}
              </template>
              <template v-else>
                {{ item[field.valueKey] }}
              </template>
            </span>
          </p>
        </template>
        <Rating v-if="ratingKey" :value="item[ratingKey]" readonly :cancel="false" class="mt-2" />
      </div>
    </div>
    <div v-if="$slots.actions" class="mt-6 flex gap-3">
      <slot name="actions" :item="item" />
    </div>
  </div>
</template>

<script setup>
import { defineProps } from 'vue'
import Tag from 'primevue/tag'
import Rating from 'primevue/rating'

const props = defineProps({
  item: { type: Object, required: true },
  nameKey: { type: String, default: 'name' },
  ratingKey: { type: String, default: '' },
  statusKey: { type: String, default: '' },
  statusSeverity: { type: Function, default: null },
  fields: {
    type: Array,
    required: true,
    // [{ icon: 'pi pi-map-marker', valueKey: 'location', label: 'Location', format: (val, item) => val }]
  },
})
</script>
