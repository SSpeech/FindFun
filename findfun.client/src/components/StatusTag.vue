<template>
  <Tag :value="status" :severity="severity" :class="tagStyle ?? 'rounded-xl'" />
</template>

<script setup lang="ts">
import type { DefineComponent } from 'vue'
declare const Tag: DefineComponent
import { useProductStatusTag } from '../composables/useStatusTag'

const props = defineProps<{
  status: string
  tagStyle?: string
}>()

const { severity } = useProductStatusTag(props)
</script>
