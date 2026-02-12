<template>
  <form
    class="flex flex-col gap-4 w-full max-w-xs mx-auto"
    @submit.prevent="handleSubmit"
    novalidate
  >
    <AccessibleInput
      v-model="email"
      type="email"
      label="Email"
      placeholder="you@example.com"
      required
    />
    <AccessibleInput
      v-model="password"
      type="password"
      label="Password"
      placeholder="Enter your password"
      required
    />
    <Button label="Sign In" type="submit" class="w-full" />
  </form>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import AccessibleInput from './AccessibleInput.vue'

const email = ref<string>('')
const password = ref<string>('')

const emit = defineEmits<(e: 'submit', value: { email: string; password: string }) => void>()

function handleSubmit() {
  emit('submit', { email: email.value, password: password.value })
}
</script>
