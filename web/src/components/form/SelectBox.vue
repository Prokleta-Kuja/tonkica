<script setup lang="ts">
import { onMounted, reactive, ref } from 'vue';
export interface ISelectBox {
    label: string;
    name?: string;
    autoFocus?: boolean;
    required?: boolean;
    modelValue?: number | null;
    help?: string;
    error?: string;
    undefinedLabel?: string;
    options: { value: number, label: string }[];
}

const el = ref<HTMLSelectElement | null>(null);
const state = reactive<{ id: string }>({ id: crypto.randomUUID() });
const props = defineProps<ISelectBox>();
const emit = defineEmits<{ (e: 'update:modelValue', modelValue?: number): void }>()

const update = (e: Event) => {
    const select = e.target as HTMLSelectElement;
    const val = parseInt(select.value);
    if (isNaN(val)) {
        select.value = "";
        emit('update:modelValue', undefined);
        return;
    }

    emit('update:modelValue', val);
}
onMounted(() => {
    if (props.autoFocus)
        el.value?.focus();
})
</script>
<template>
    <div>
        <label class="form-label" :for="state.id">{{ props.label }} <span v-if="required">*</span></label>
        <select class="form-select" :class="{ 'is-invalid': error }" :id="state.id" :required="required" @input="update"
            :value="modelValue" :name="name">
            <option v-if="props.undefinedLabel" value="">{{ props.undefinedLabel }}</option>
            <option v-for="option in props.options" :key="option.value" :value="option.value">
                {{ option.label }}
            </option>
        </select>
        <div v-if="error" class="invalid-feedback">{{ error }}</div>
        <div v-else-if="help" class="form-text">{{ help }}</div>
    </div>
</template>