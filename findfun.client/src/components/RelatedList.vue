<script setup lang="ts">
interface ItemType {
  [key: string]: any
}

const props = defineProps<{
  items: ItemType[]
  title?: string
  imageKey?: string
  nameKey?: string
  locationKey?: string
  ratingKey?: string
  idKey?: string
  class?: string
  cardClass?: string
  titleClass?: string
  customCard?: object | Function | string | null
}>()

const emit = defineEmits(['view-details'])

const listClass = props.class || 'grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-4'
const titleClass = props.titleClass || 'text-2xl font-semibold mb-4'
const cardClass = props.cardClass || 'rounded-t-xl h-32 w-full object-cover'
</script>

<template>
  <div>
    <h2 :class="titleClass">{{ title }}</h2>
    <div :class="listClass">
      <component
        v-for="item in items"
        :key="(item as any)[idKey ?? 'id'] || (item as any)[nameKey ?? 'name']"
        :is="customCard || 'DefaultCard'"
        :item="item"
        :image-key="imageKey"
        :name-key="nameKey"
        :location-key="locationKey"
        :rating-key="ratingKey"
        :card-class="cardClass"
        @view-details="$emit('view-details', item)"
      />
    </div>
  </div>
</template>
