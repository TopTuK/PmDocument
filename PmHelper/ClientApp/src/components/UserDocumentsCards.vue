<template>
    <!-- User documents loading -->
    <va-inner-loading
        v-if="userDocumentsState.isLoading"
        loading
    />

    <va-card
        v-else-if="(userDocumentsState.error != null)"
    >
        <va-card-title>Title</va-card-title>
        <va-card-content>
            Lorem ipsum dolor sit amet, consectetur adipiscing elit.
        </va-card-content>
    </va-card>

    <va-card
        v-else-if="(userDocumentsState.error == null) && (!userDocumentsState.isLoading) && (userDocumentsState.documents.length == 0)"
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
        v-for="userDocument in userDocumentsState.documents"
        :doc="userDocument"
    />
</template>

<script setup>
import { onBeforeMount } from 'vue';
import { storeToRefs } from 'pinia';
import { useDocumentStore } from '@/stores/documentsStore';
import UserDocumentCard from './UserDocumentCard.vue';

const documentsStore = useDocumentStore();
const { userDocumentsState } = storeToRefs(documentsStore);
// const userDocumentsState = documentsStore.userDocumentsState;

onBeforeMount(async () => {
    try {
        await documentsStore.getUserDocuments();
    } catch (error) {
        console.error(error);
    }
});
</script>