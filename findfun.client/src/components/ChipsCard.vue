<template>
  <div :class="chipsAreaWrapperClass || 'flex flex-col gap-2 mt-2 sm:mt-0'">
    <span class="font-medium text-surface-500 dark:text-surface-400 text-sm">{{ title }}</span>
    <div class="flex flex-wrap gap-2">
      <template v-for="(chip, idx) in displayedChips" :key="chip.label">
        <Chip :label="chip.label" :image="chip.image" :icon="chip.icon" />
      </template>
      <Button
        v-if="props.showToggleButton !== false && chips.length > 2"
        :icon="showAll ? 'pi pi-minus' : 'pi pi-plus'"
        raised
        severity="help"
        @click="toggleShowAll"
        class="rounded-full p-2 text-sm"
        aria-label="Toggle chips visibility"
      />
    </div>
  </div>
</template>

<script setup lang="ts">
import { useProductChips } from '../composables/useChips'

interface ChipData {
  label: string
  image: string
  icon?: string
  removable?: boolean
}

const props = defineProps<{
  chips: ChipData[]
  title?: string
  chipsAreaWrapperClass?: string
  showToggleButton?: boolean
  toggleIconType?: 'chevron' | 'plusminus'
}>()

const { showAll, displayedChips, isMobile, toggleShowAll } = useProductChips(props)
</script>
