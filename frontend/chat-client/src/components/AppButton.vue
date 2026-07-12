<script setup>
defineProps({
    disabled: {
        type: Boolean,
        default: false
    }
});
</script>

<template>
    <button :disabled="disabled">
        <slot />
    </button>
</template>