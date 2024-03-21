<script setup lang="ts">
import { computed } from 'vue';

export interface ISpinButton {
    text: string;
    loading?: boolean;
    loadingText?: string;
    onClick?: () => void;
    isSubmit?: boolean;
}

const props = defineProps<ISpinButton>();
const type = computed<undefined | "submit" | "button" | "reset">(() => {
    return props.isSubmit ? "submit" : "button";
})
</script>
<template>
    <button class="btn" :type="type" @click="onClick" :disabled="loading">
        <template v-if="loading">
            <span class="spinner-border spinner-border-sm"></span>&nbsp;
            <span v-if="loadingText">{{ loadingText }}</span>
        </template>
        <span v-else>&nbsp;{{ text }}</span>
    </button>
</template>