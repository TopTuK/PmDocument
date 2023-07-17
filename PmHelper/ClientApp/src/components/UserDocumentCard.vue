<template>
    <va-card>
        <va-card-title>
            Title
        </va-card-title>
        
        <va-card-content>
            Lorem ipsum dolor sit amet, consectetur adipiscing elit.
        </va-card-content>
    </va-card>
</template>

<script setup>
defineProps({
    doc: {
        type: Object,
        required: true,
    },
});
</script>