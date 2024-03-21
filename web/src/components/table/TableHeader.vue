<script setup lang="ts" generic="T">
import { computed } from 'vue';
import type { ITableParams } from '.';
import IconSortDown from '@/components/icons/IconSortDown.vue'
import IconSortUp from '@/components/icons/IconSortUp.vue'

const props = defineProps<{
    params: ITableParams<T>
    onSort: (params: ITableParams<T>) => void
    column?: T
    display?: string
    unsortable?: boolean
}>()

const text = computed(() => {
    if (props.display) return props.display
    if (props.column) {
        const t = props.column.toString()
        return t[0].toUpperCase() + t.substring(1).toLowerCase()
    }
    return ''
})

const sort = () => {
    if (props.unsortable) return
    let orderAsc = props.params.orderAsc
    if (props.params.orderBy === props.column) orderAsc = orderAsc ? undefined : 'true'
    props.onSort({ ...props.params, page: 1, orderBy: props.column, orderAsc: orderAsc })
}
</script>
<template>
    <th @click="sort" :class="{ pointer: !unsortable }">
        <b>{{ text }}</b>
        <span class="ms-2" v-if="!unsortable && props.params.orderBy === props.column">
            <IconSortDown v-if="props.params.orderAsc" />
            <IconSortUp v-else />
        </span>
    </th>
</template>
<style>
.pointer {
    cursor: pointer;
}
</style>