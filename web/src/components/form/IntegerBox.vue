<script setup lang="ts">
import { onMounted, reactive, ref } from 'vue';
export interface IIntegerBox {
    label?: string;
    name?: string;
    autoFocus?: boolean;
    required?: boolean;
    placeholder?: string;
    autoComplete?: "off" | "one-time-code";
    modelValue?: number | null;
    help?: string;
    error?: string;
}

const el = ref<HTMLInputElement | null>(null);
const state = reactive<{ id: string }>({ id: crypto.randomUUID() });
const props = defineProps<IIntegerBox>();
const emit = defineEmits<{ (e: 'update:modelValue', modelValue?: number): void }>()

const update = (e: Event) => {
    const input = e.target as HTMLInputElement;
    const val = parseInt(input.value);
    if (isNaN(val)) {
        input.value = "";
        emit('update:modelValue', undefined);
        return;
    }

    const valStr = val.toString();
    if (input.value !== valStr)
        input.value = valStr;
    emit('update:modelValue', val);
}
onMounted(() => {
    if (props.autoFocus)
        el.value?.focus();
})
</script>
<template>
    <div>
        <label v-if="label" :for="state.id" class="form-label">{{ label }} <span v-if="required">*</span></label>
        <input ref="el" class="form-control" :class="{ 'is-invalid': error }" :id="state.id" :placeholder="placeholder"
            :value="modelValue" @blur="update" @keydown.enter="update" :required="required" type="text" :name="name"
            :autocomplete="autoComplete">
        <div v-if="error" class="invalid-feedback">{{ error }}</div>
        <div v-else-if="help" class="form-text">{{ help }}</div>
    </div>
</template>