<template>
  <div class="card">
    <section
      ref="carouselRegion"
      aria-roledescription="carousel"
      aria-label="Featured parks carousel"
      @keydown="handleKeyDown"
      class="focus:outline-none"
    >
      <!-- Screen-reader instructions -->
      <p class="sr-only" aria-hidden="false">Use left and right arrow keys to navigate slides.</p>
      <Carousel
        :value="products"
        :numVisible="responsiveOptions[0].numVisible"
        :numScroll="responsiveOptions[0].numScroll"
        :responsiveOptions="responsiveOptions"
        :showNavigators="responsiveOptions[0].showNavigators"
        :circular="responsiveOptions[0].circular"
        :autoplayInterval="responsiveOptions[0].autoplayInterval"
      >
        <template #item="slotProps">
          <article
            :id="slotProps.data.id"
            :aria-labelledby="'product-card-title-' + slotProps.data.id"
            tabindex="-1"
            class="border-surface-200 dark:border-surface-700 rounded-xl m-2 mb-0 p-4 shadow-md transform scale-95 transition-transform duration-300 flex flex-col items-center"
          >
            <div class="w-full flex flex-col items-center">
              <div
                class="bg-gradient-to-r from-blue-500 to-green-400 text-white p-4 shadow w-full flex flex-col items-center -mt-1 rounded-t-lg rounded-b-none"
              >
                <h2 class="text-xl font-bold mb-1">Find Your Next Adventure!</h2>
                <p class="mb-2 text-sm">
                  Browse, compare, and plan your visit to the best parks around you.
                </p>
                <Button
                  label="Explore Parks"
                  icon="pi pi-search"
                  class="bg-white text-blue-600 font-bold px-4 py-2 rounded shadow hover:bg-blue-50"
                  @click="goToDetail(slotProps.data.id)"
                  tabindex="0"
                />
              </div>
              <div class="w-full">
                <ImageCard
                  :src="slotProps.data?.parkImages[0]?.replace('localhost', 'localhost:5163')"
                  :alt="slotProps.data.name"
                  :status="getStatusLabel(slotProps.data.averageRating)"
                  imgStyle="width: 100%; height: 20rem; object-fit: cover; border-radius: 0 0 0.75rem 0.75rem ;"
                />
              </div>
            </div>
            <div class="w-full flex flex-col items-center mt-4">
              <div
                :id="'product-card-title-' + slotProps.data.id"
                class="mb-2 font-medium text-lg text-center"
              >
                {{ slotProps.data.name }}
              </div>
              <div class="mb-2 font-medium text-lg text-center">
                {{ slotProps.data.description }}
              </div>
              <div class="text-xs text-gray-600 dark:text-white mb-2 text-center">
                <strong class="text-blue-600 dark:text-sky-400 font-large">Location: </strong
                >{{ slotProps.data.locationName }}
              </div>
              <div class="flex flex-col items-center gap-1 mb-2">
                <div class="flex items-center gap-2">
                  <span class="text-yellow-600 dark:text-yellow-400 font-medium">Type:</span>
                  <span class="text-xs text-gray-600 dark:text-gray-300">
                    {{ getParkTypeOrEvent(slotProps.data) }}
                  </span>
                </div>
                <div class="flex items-center gap-3 mt-1">
                  <span
                    :class="[
                      'flex items-center gap-1',
                      slotProps.data.reviews.length > 0
                        ? 'text-yellow-600 dark:text-yellow-400'
                        : 'text-gray-400 dark:text-gray-500',
                    ]"
                  >
                    <i
                      :class="slotProps.data.averageRating === 0 ? 'pi pi-star' : 'pi pi-star-fill'"
                    />
                    <span>{{
                      slotProps.data.averageRating !== undefined
                        ? slotProps.data.averageRating.toFixed(1)
                        : 'N/A'
                    }}</span>
                  </span>
                  <span class="text-gray-500 dark:text-gray-300 flex items-center gap-1">
                    <i class="pi pi-comments" />
                    <span>{{ slotProps.data.reviews.length }} reviews</span>
                  </span>
                </div>
              </div>

              <div class="flex items-center mb-4">
                <Button
                  v-for="action in slotProps.data.actions"
                  :key="action.icon"
                  :icon="action.icon"
                  :title="action.title"
                  :severity="action.severity || 'secondary'"
                  :outlined="action.outlined !== false"
                  @click="onActionClick(action, slotProps.data)"
                  :aria-label="action.title || 'action'"
                  :aria-controls="'product-card-title-' + slotProps.data.id"
                />
              </div>
            </div>
          </article>
        </template>
      </Carousel>
    </section>
  </div>
</template>

<script setup lang="ts">
import type { Items, Park } from '../types/park'
import { useRouter } from 'vue-router'
import { ref } from 'vue'
import { Routes, ViewType } from '@/config/Enums'

defineProps<{
  products: Items
  responsiveOptions: Array<{
    breakpoint: string
    numVisible: number
    numScroll: number
    circular?: boolean
    autoplayInterval?: number
    showNavigators?: boolean
    showIndicators?: boolean
  }>
  actions?: Array<{
    icon: string
    title?: string
    severity?: string
    outlined?: boolean
    onClick?: (product: unknown) => void
  }>
}>()

const router = useRouter()

const goToDetail = (parkId: string) => {
  router.push({ name: Routes.ParkDetail, params: { id: parkId } })
}

const getParkTypeOrEvent = (data: Park | Event) => {
  if ('parkType' in data) {
    return ViewType.Park
  }
  return ViewType.Event
}
const getStatusLabel = (rating?: number) => {
  if (typeof rating !== 'number' || rating === 0) return 'Not rated'
  if (rating >= 4.5) return 'Best'
  if (rating >= 3) return 'Good'
  if (rating >= 2) return 'Average'
  if (rating > 0) return 'Poor'
  return 'Not rated'
}

const carouselRegion = ref<HTMLElement | null>(null)

function clickNavigator(direction: 'prev' | 'next') {
  if (!carouselRegion.value) return
  // PrimeVue Carousel navigator buttons often have classes or aria-labels; try both strategies.
  const prevBtn = carouselRegion.value.querySelector<HTMLButtonElement>(
    "button[aria-label='Previous']",
  )
  const nextBtn = carouselRegion.value.querySelector<HTMLButtonElement>("button[aria-label='Next']")
  if (direction === 'prev' && prevBtn) prevBtn.click()
  else if (direction === 'next' && nextBtn) nextBtn.click()
  else {
    // fallback: try common class names
    const fallbackPrev = carouselRegion.value.querySelector<HTMLButtonElement>('.p-carousel-prev')
    const fallbackNext = carouselRegion.value.querySelector<HTMLButtonElement>('.p-carousel-next')
    if (direction === 'prev' && fallbackPrev) fallbackPrev.click()
    if (direction === 'next' && fallbackNext) fallbackNext.click()
  }
}

function handleKeyDown(e: KeyboardEvent) {
  if (e.key === 'ArrowLeft') {
    e.preventDefault()
    clickNavigator('prev')
  } else if (e.key === 'ArrowRight') {
    e.preventDefault()
    clickNavigator('next')
  }
}

type ActionType = {
  icon: string
  title?: string
  severity?: string
  outlined?: boolean
  onClick?: (product: Park) => void
}
function onActionClick(action: ActionType, data: Park) {
  // prefer action handler if provided
  if (action && typeof action.onClick === 'function') {
    action.onClick(data)
  }
  const targetId = 'product-card-' + data.id
  const el = document.getElementById(targetId) as HTMLElement | null
  if (el) {
    // make focusable if not naturally focusable
    const prevTab = el.getAttribute('tabindex')
    if (!el.hasAttribute('tabindex')) el.setAttribute('tabindex', '-1')
    el.scrollIntoView({ behavior: 'smooth', block: 'center' })
    el.focus({ preventScroll: true })
    // restore previous tabindex if any after a short delay
    setTimeout(() => {
      if (prevTab === null) el.removeAttribute('tabindex')
      else el.setAttribute('tabindex', prevTab)
    }, 1500)
  }
}
</script>
