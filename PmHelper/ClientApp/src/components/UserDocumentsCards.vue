<template>
    <!-- User documents loading -->
    <va-inner-loading
        v-if="userDocuments.loading"
        loading
    />

    <va-card
        v-else-if="userDocuments.error"
    >
        <va-card-title>Title</va-card-title>
        <va-card-content>
            Lorem ipsum dolor sit amet, consectetur adipiscing elit.
        </va-card-content>
    </va-card>

    <va-card
        v-else-if="!userDocuments.error && !userDocuments.loading && (userDocuments.documents.length == 0)"
        :bordered="false"
        disabled
    >
        <va-card-title>Empty</va-card-title>
        <va-card-content>
            User documents empty
        </va-card-content>
    </va-card>

    <UserDocumentCard
        v-else
        v-for="userDoc in userDocuments.documents"
        :doc="userDoc"
    />
</template>

<script setup>
import { onBeforeMount } from 'vue';
import useDocumentService from '@/services/documentService.js';
import UserDocumentCard from './UserDocumentCard.vue';

const documentService = useDocumentService();
const userDocuments = documentService.userDocuments;

onBeforeMount(async () => {
    try {
        await documentService.getUserDocuments();
    } catch (error) {
        console.error(error);
    }
});
</script>