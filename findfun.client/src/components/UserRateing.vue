<template>
  <div class="flex items-center mb-2">
    <template v-for="n in 5" :key="n">
      <svg
        xmlns="http://www.w3.org/2000/svg"
        :class="[starSize, n <= rating ? 'text-yellow-400' : 'text-gray-300 dark:text-gray-600']"
        fill="currentColor"
        viewBox="0 0 20 20"
      >
        <path
          d="M9.049 2.927c.3-.921 1.603-.921 1.902 0l1.286 3.974a1 1 0 00.95.69h4.178c.969 0 1.371 1.24.588 1.81l-3.384 2.46a1 1 0 00-.364 1.118l1.286 3.974c.3.921-.755 1.688-1.54 1.118l-3.384-2.46a1 1 0 00-1.175 0l-3.384 2.46c-.784.57-1.838-.197-1.539-1.118l1.286-3.974a1 1 0 00-.364-1.118L2.045 9.401c-.783-.57-.38-1.81.588-1.81h4.178a1 1 0 00.95-.69l1.286-3.974z"
        />
      </svg>
    </template>
  </div>
</template>

<script setup lang="ts">
withDefaults(
  defineProps<{
    rating: number
    starSize?: string
  }>(),
  {
    starSize: 'h-4 w-4',
  },
)
</script>
