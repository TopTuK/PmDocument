<template>
    <va-card>
        <va-card-title>
            {{ props.doc.title }}
        </va-card-title>
        
        <va-card-content>
            {{ requestText }}
        </va-card-content>

        <va-card-actions align="center" class="gap-3">
            <va-button>View</va-button>
            <va-button>Share</va-button>
            <va-button>Remove</va-button>
        </va-card-actions>
    </va-card>
</template>

<script setup>
import { computed } from 'vue';
import { filter } from '../utils';

const props = defineProps({
    doc: {
        type: Object,
        required: true,
    },
});

const requestText = computed(() => {
    return filter(props.doc.requestText, 300);
});
</script>