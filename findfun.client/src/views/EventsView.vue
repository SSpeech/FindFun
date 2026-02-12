<script setup lang="ts">
import { ViewType } from '@/config/Enums'
</script>

<template>
  <ParkEventTab :type="ViewType.Event" :key="ViewType.Event" />
</template>
