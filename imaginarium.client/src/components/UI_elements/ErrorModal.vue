<template>
    <span class="error-container" v-if="error.length">{{ error }}</span>
</template>

<script setup lang="ts">

import { ref, watch } from "vue";

const emits = defineEmits(["clearError"])

const props = defineProps({
  errorText: {
    type: String,
    default:() => {}
  }
})

const error = ref(props.errorText);

watch(() => error.value, (newValue) => {
  error.value = newValue
})

setTimeout(() => {
    error.value = '';
    emits("clearError")
}, 4000);

</script>

<style scoped lang="scss">

</style>
