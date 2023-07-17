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

            <va-button
                v-show="canCreate"
                color="info"
                gradient
                size="large"
                class="mt-6"
                :to="{
                    name: 'CreateDocument',
                    params: {
                        type_id: doc.id
                    }
                }"
            >
                {{ $t('documents.create_document_button_title') }}
            </va-button>
        </va-card-content>
    </va-card>
</template>

<script setup>
defineProps({
    doc: {
        type: Object,
        required: true,
    },
    canCreate: {
        type: Boolean,
        required: false,
        default: false,
    }
});
</script>