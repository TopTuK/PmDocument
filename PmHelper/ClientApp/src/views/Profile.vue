<template>
    <div class="flex flex-col">
        <!--<div 
            class="p-4 grid shrink gap-4 lg:grid-cols-3 grid-cols-2"
            v-if="!user.error && !user.loading"
        >-->
        <div
            class="p-4 grid shrink gap-4 lg:grid-cols-3 grid-cols-2"
            v-if="!error && !isLoading"
        >
            <!-- User Profile card -->
            <ProfileCard :userInfo="user" />
            <!-- Create document card -->
            <CreateDocumentCard />

            <!-- User Documents -->
            <UserDocumentsCards />
        </div>

        <div
            class="flex items-center justify-center"
            v-else-if="error"
        >
            Error!
        </div>

        <div
            class="flex items-center justify-center"
            v-else
        >
            Loading...
        </div>
    </div>
</template>

<script setup>
import { onBeforeMount } from 'vue';
import { storeToRefs } from 'pinia';

import { useUserStore } from '@/stores/userStore';

import ProfileCard from '@/components/ProfileCard.vue';
import CreateDocumentCard from '@/components/CreateDocumentCard.vue';
import UserDocumentsCards from '@/components/UserDocumentsCards.vue';

const userStore = useUserStore();
const { user, isLoading, error } = storeToRefs(userStore);

onBeforeMount(async () => {
    try {
        await userStore.getUser(true);
    } catch (error) {
        console.error(error);
    }
});
</script>