<script setup lang="ts" generic="T">
import { computed } from 'vue';
import type { ITableParams } from '.';
import IconSkipBackwardFill from '@/components/icons/IconSkipBackwardFill.vue'
import IconSkipForwardFill from '@/components/icons/IconSkipForwardFill.vue'


const pageOffset = 2
const props = defineProps<{
    params: ITableParams<T>
    onChange: (params: ITableParams<T>) => void
}>()
const fmt = new Intl.NumberFormat()
const counter = computed<{
    startItem: string
    endItem: string
    totalItems: string
    totalPages: number
    pages: number[]
    startPage: number
    endPage: number
}>(() => {
    const last = (props.params.page - 1) * props.params.size
    const startItem = fmt.format(last + 1)
    const endItem = fmt.format(Math.min(last + props.params.size, props.params.total))
    const totalItems = fmt.format(props.params.total)
    const totalPages = Math.max(1, Math.ceil(props.params.total / props.params.size))
    const startPage = Math.max(1, props.params.page - pageOffset)
    const endPage = Math.min(totalPages, props.params.page + pageOffset)
    const pages = []
    for (let i = startPage; i <= endPage; i++) pages.push(i)

    return { startItem, endItem, totalItems, totalPages, pages, startPage, endPage }
})

const change = (page: number) => {
    if (props.params.page === page) return
    props.onChange({ ...props.params, page: page })
}
</script>
<template>
    <div class="d-flex justify-content-between align-items-center flex-wrap-reverse">
        <div v-if="props.params.total === 0">No results</div>
        <div v-else>Showing {{ counter.startItem }} to {{ counter.endItem }} of {{ counter.totalItems }} entries.</div>
        <nav v-if="counter.totalPages > 1">
            <ul class="pagination mb-0">
                <li class="page-item" :class="{ disabled: props.params.page === 1, pointer: props.params.page !== 1 }"
                    @click="change(1)">
                    <span class="page-link" aria-label="Previous">
                        <span aria-hidden="true">
                            <IconSkipBackwardFill />
                        </span>
                    </span>
                </li>

                <li class="page-item disabled" v-if="counter.startPage !== 1">
                    <span class="page-link">...</span>
                </li>

                <li class="page-item" v-for="page in counter.pages" :key="page" @click="change(page)">
                    <span class="page-link"
                        :class="{ active: props.params.page === page, pointer: props.params.page !== page }">{{ page
                        }}</span>
                </li>

                <li class="page-item disabled" v-if="counter.endPage !== counter.totalPages">
                    <span class="page-link">...</span>
                </li>

                <li class="page-item" @click="change(counter.totalPages)"
                    :class="{ disabled: props.params.page === counter.totalPages, pointer: props.params.page !== counter.totalPages }">
                    <span class="page-link" aria-label="Next">
                        <span aria-hidden="true">
                            <IconSkipForwardFill />
                        </span>
                    </span>
                </li>
            </ul>
        </nav>
    </div>
</template>
<style>
.pointer {
    cursor: pointer;
}
</style>