<script setup lang="ts">
import { ref, computed, onMounted } from 'vue';
export interface IModal {
    title: string;
    width?: "sm" | "lg" | "xl";
    scrollable?: boolean;
    centered?: boolean;
    shown?: boolean;
    autoFocus?: boolean;
    onClose: () => void;
}
const el = ref<HTMLHeadingElement | null>(null);
const props = defineProps<IModal>()
const classModal = computed(() => ({ show: props.shown, 'd-none': !props.shown, 'd-block': props.shown }))
const classDialog = computed(() => ({
    'modal-sm': props.width === "sm",
    'modal-lg': props.width === "lg",
    'modal-xl': props.width === "xl",
    'modal-dialog-scrollable': props.scrollable,
    'modal-dialog-centered': props.centered,
}))
onMounted(() => {
    if (props.autoFocus)
        el.value?.focus()
})
</script>
<template>
    <div class="modal" :class="classModal" tabindex="-1" role="dialog" @keydown.esc="props.onClose">
        <div class="modal-dialog" :class="classDialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 ref="el" class="modal-title" tabindex="-1">{{ props.title }}</h5>
                    <button type="button" class="btn-close" @click="props.onClose"></button>
                </div>
                <div class="modal-body">
                    <slot name="body" />
                </div>
                <div class="modal-footer">
                    <slot name="footer" />
                </div>
            </div>
        </div>
    </div>
    <div class="modal-backdrop" :class="classModal"></div>
</template>