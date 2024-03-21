<script setup lang="ts">
import { onMounted, reactive, ref } from 'vue';
export interface ITextBox {
    label?: string;
    name?: string;
    autoFocus?: boolean;
    required?: boolean;
    placeholder?: string;
    type?: "text" | "email" | "tel" | "password";
    autoComplete?: "off" | "username" | "password" | "one-time-code";
    modelValue?: string | null;
    help?: string;
    error?: string;
}

const el = ref<HTMLInputElement | null>(null);
const state = reactive<{ id: string }>({ id: crypto.randomUUID() });
const props = defineProps<ITextBox>();
const emit = defineEmits<{ (e: 'update:modelValue', modelValue?: string): void }>()

const update = (e: Event) => {
    const input = e.target as HTMLInputElement;
    emit('update:modelValue', input.value);
}
onMounted(() => {
    if (props.autoFocus)
        el.value?.focus();
})
</script>
<template>
    <div>
        <label v-if="label" :for="state.id" class="form-label">{{ label }} <span v-if="required">*</span></label>
        <input ref="el" class="form-control" :id="state.id" :class="{ 'is-invalid': error }" :placeholder="placeholder"
            :value="modelValue" @input="update" :required="required" :type="type" :autocomplete="autoComplete" :name="name">
        <div v-if="error" class="invalid-feedback">{{ error }}</div>
        <div v-else-if="help" class="form-text">{{ help }}</div>
    </div>
</template>