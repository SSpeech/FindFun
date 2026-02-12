<script setup lang="ts">
import type { Component } from 'vue';

defineProps<{
  products: {
    id: string
    code: string
    name: string
    description: string
    image: string
    price: number
    category: string
    quantity: number
    inventoryStatus: string
    rating: number
  }[]
  tabs: {
    label: string
    slot: string
    component: Component
  }[]
}>()
</script>

<template>
  <section class="flex flex-col">
    <div class="p-8">
      <h1 class="text-3xl font-bold mb-4">Parks</h1>
      <p>Welcome to the Parks page. Explore all parks and their features here.</p>
      <div v-if="products.length === 0" class="text-center text-lg text-gray-500 my-12">
        <span class="pi pi-info-circle text-2xl mr-2"></span> No parks available at the moment.
        Please check back soon!
      </div>
      <TabView v-else :scrollable="true">
        <TabPanel v-for="tab in tabs" :key="tab.label" :header="tab.label" :value="tab.slot">
          <component :is="tab.component" />
        </TabPanel>
      </TabView>
      <GoogleMap />
    </div>
  </section>
</template>
