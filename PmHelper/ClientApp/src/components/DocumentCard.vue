<template>
    <va-card
        color="primary"
        gradient
        :href="doc.url"
    >
        <va-card-title>
            {{ $t(`documents_types.${doc.title}`) }}
        </va-card-title>
        <va-card-content>
            <img 
                :src="doc.image" 
                class="h-48 bg-base-100"
                alt="" 
            />

            <p class="mt-4">
                {{ $t(`documents_types.${doc.text}`) }}
            </p>
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