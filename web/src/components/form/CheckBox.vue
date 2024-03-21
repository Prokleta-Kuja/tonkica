<script setup lang="ts">
import { onMounted, reactive, ref } from 'vue';
export interface ICheckBox {
    label: string;
    name?: string;
    autoFocus?: boolean;
    required?: boolean;
    modelValue?: boolean | null;
    help?: string;
    error?: string;
    inline?: boolean;
    onChange?: () => void;
}

const el = ref<HTMLInputElement | null>(null);
const state = reactive<{ id: string }>({ id: crypto.randomUUID() });
const props = defineProps<ICheckBox>();
const emit = defineEmits<{ (e: 'update:modelValue', modelValue?: boolean): void }>()

const update = (e: Event) => {
    const input = e.target as HTMLInputElement;
    emit('update:modelValue', input.checked);
    if (props.onChange)
        props.onChange();
}
onMounted(() => {
    if (props.autoFocus)
        el.value?.focus();
})
</script>
<template>
    <div class="form-check" :class="{ 'form-check-inline': props.inline }">
        <input ref="el" class="form-check-input" :class="{ 'is-invalid': error }" @input="update" type="checkbox"
            :checked="modelValue ?? false" :id="state.id" :required="required" :name="name">
        <label class="form-check-label" :for="state.id">{{ label }}<span v-if="required">*</span></label>
        <div v-if="error" class="invalid-feedback">{{ error }}</div>
        <div v-else-if="help" class="form-text">{{ help }}</div>
    </div>
</template>