<script setup lang="ts">
import { ViewType } from '@/config/Enums'
</script>
<template>
  <ParkEventTab :type="ViewType.Park" :key="ViewType.Park" />
</template>
