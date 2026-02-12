<template>
  <form
    class="flex flex-col gap-4 w-full max-w-xs mx-auto"
    @submit.prevent="handleSubmit"
    novalidate
  >
    <AccessibleInput
      v-model="email"
      label="Email"
      type="email"
      placeholder="you@example.com"
      required
    />
    <AccessibleInput
      v-model="password"
      label="Password"
      type="password"
      placeholder="Enter your password"
      required
    />
    <AccessibleInput
      v-model="confirmPassword"
      label="Confirm Password"
      type="password"
      placeholder="Confirm your password"
      required
    />
    <div class="flex items-center gap-2">
      <Checkbox v-model="acceptTerms" inputId="acceptTerms" :binary="true" required />
      <label for="acceptTerms" class="text-sm select-none"
        >I accept the <a href="#" class="text-primary underline">terms and conditions</a></label
      >
    </div>
    <Button label="Sign Up" type="submit" class="w-full" :disabled="!acceptTerms" />
  </form>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import AccessibleInput from './AccessibleInput.vue'

const emit =
  defineEmits<
    (e: 'submit', value: { email: string; password: string; confirmPassword: string }) => void
  >()

function handleSubmit() {
  if (!acceptTerms.value) return
  emit('submit', {
    email: email.value,
    password: password.value,
    confirmPassword: confirmPassword.value,
  })
}
const email = ref('')
const password = ref('')
const confirmPassword = ref('')
const acceptTerms = ref(false)
</script>
