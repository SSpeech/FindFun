<script setup lang="ts">
const colorMode = ref('')
const toggleColorMode = () => {
  document.documentElement.classList.toggle('my-app-dark')
  document.documentElement.classList.toggle('dark')
  colorMode.value = document.documentElement.classList.contains('dark') ? 'dark' : 'light'
}

provide('colorMode', colorMode)
provide('toggleColorMode', toggleColorMode)
</script>

<template>
  <Button
    :icon="colorMode === 'light' ? 'pi pi-moon' : 'pi pi-sun'"
    class="p-button-rounded-full p-button-text w-10 h-10 flex items-center justify-center object-cover aspect-square"
    aria-label="Toggle color mode"
    @click="toggleColorMode"
  />
</template>
