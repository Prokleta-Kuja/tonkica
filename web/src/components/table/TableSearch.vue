<script setup lang="ts">
import { onMounted, ref } from 'vue';
export interface ISearch {
    label?: string;
    name?: string;
    autoFocus?: boolean;
    placeholder?: string;
    modelValue?: string;
    onChange: () => void;
}

const el = ref<HTMLInputElement | null>(null);
const props = defineProps<ISearch>();
const emit = defineEmits<{ (e: 'update:modelValue', modelValue?: string): void }>()

const search = () => {
    emit('update:modelValue', el.value?.value);
    props.onChange();
}
onMounted(() => {
    if (props.autoFocus) el.value?.focus()
})
</script>
<template>
    <div>
        <label for="search" class="form-label">
            <span v-if="label">{{ label }}</span>
            <span v-else>Search</span>
        </label>
        <div class="input-group">
            <input ref="el" class="form-control" id="search" :placeholder="placeholder" :value="props.modelValue"
                @keyup.enter="search" type="search" :name="name">
        </div>
    </div>
</template>