<script setup lang="ts">
import { computed, onMounted, ref } from 'vue';
export interface IConfirmationModal {
    title: string;
    shown?: boolean;
    confirmText?: string;
    cancelText?: string;
    onClose: () => void;
    onConfirm: () => void;
}
const el = ref<HTMLHeadingElement | null>(null);
const props = defineProps<IConfirmationModal>()
const classModal = computed(() => ({ show: props.shown, 'd-none': !props.shown, 'd-block': props.shown }))

onMounted(() => {
    el.value?.focus()
})
</script>
<template>
    <div class="modal" :class="classModal" tabindex="-1" role="dialog" @keydown.esc="props.onClose">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 ref="el" class="modal-title" tabindex="-1">{{ props.title }}</h5>
                    <button type="button" class="btn-close" @click="props.onClose"></button>
                </div>
                <div class="modal-body">
                    <slot />
                </div>
                <div class="modal-footer">
                    <button class="btn btn-outline-danger" @click="props.onClose">
                        <span v-if="props.cancelText">{{ props.cancelText }}</span>
                        <span v-else>Close</span>
                    </button>
                    <button class="btn btn-success" @click="props.onConfirm">
                        <span v-if="props.confirmText">{{ props.confirmText }}</span>
                        <span v-else>Okey</span>
                    </button>
                </div>
            </div>
        </div>
    </div>
    <div class="modal-backdrop" :class="classModal"></div>
</template>