<template>
    <div class="flex flex-col">
        <div 
            class="p-4 grid shrink gap-4 lg:grid-cols-3 grid-cols-2"
            v-if="!user.error && !user.loading"
        >
            <!-- User Profile card -->
            <ProfileCard :userInfo="user.userInfo" />
            <!-- Create document card -->
            <CreateDocumentCard />
        </div>

        <div
            class="flex items-center justify-center"
            v-else-if="user.error"
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
import useUserService from "@/services/userService.js";
import useDocumentService from '@/services/documentService.js';
import ProfileCard from '@/components/ProfileCard.vue';
import CreateDocumentCard from '@/components/CreateDocumentCard.vue';

const userService = useUserService();
const user = userService.userState;

const documentService = useDocumentService();
const userDocuments = documentService.userDocuments;

onBeforeMount(async () => {
    try {
        await userService.getUserInfo();
        await documentService.getUserDocuments();
    } catch (error) {
        console.error(error);
    }
});
</script>