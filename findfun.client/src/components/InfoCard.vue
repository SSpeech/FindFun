<template>
  <div
    :class="
      layout === 'list'
        ? 'flex flex-col sm:flex-row sm:items-center gap-2 sm:gap-4 justify-between w-full'
        : 'flex items-center gap-4 justify-between w-full'
    "
  >
    <template v-if="layout === 'list'">
      <div class="flex flex-row sm:flex-col items-center sm:items-start gap-2 w-full">
        <ChipsCard :chips="chips" />
        <div
          class="bg-surface-100 p-1 align-middle sm:static sm:mt-0 mt-2"
          style="border-radius: 30px"
        >
          <div
            class="bg-surface-0 flex items-center gap-2 justify-center py-1 px-2"
            style="
              border-radius: 30px;
              box-shadow:
                0px 1px 2px 0px rgba(0, 0, 0, 0.04),
                0px 1px 2px 0px rgba(0, 0, 0, 0.06);
            "
          >
            <span class="text-surface-900 font-medium text-sm">{{ rating }}</span>
            <i class="pi pi-star-fill text-yellow-500"></i>
          </div>
        </div>
      </div>
    </template>
    <template v-else>
      <ChipsCard :chips="chips" />
      <div class="bg-surface-100 p-1 align-middle" style="border-radius: 30px">
        <div
          class="bg-surface-0 flex items-center gap-2 justify-center py-1 px-2"
          style="
            border-radius: 30px;
            box-shadow:
              0px 1px 2px 0px rgba(0, 0, 0, 0.04),
              0px 1px 2px 0px rgba(0, 0, 0, 0.06);
          "
        >
          <span class="text-surface-900 font-medium text-sm">{{ rating }}</span>
          <i class="pi pi-star-fill text-yellow-500"></i>
        </div>
      </div>
    </template>
  </div>
</template>

<script setup lang="ts">
import { useProductInfo } from '../composables/useInfo'

const props = defineProps<{
  category: string
  name: string
  rating: number
  layout?: string
}>()

const { wrapperClass, chips } = useProductInfo(props)
</script>
