<template>
    <div class="flex flex-col">
        <div 
            class="p-4 grid shrink gap-4 lg:grid-cols-3 grid-cols-2"
            v-if="!user.error && !user.loading"
        >
            <!-- Profile card -->
            <ProfileCard :userInfo="user.userInfo" />

            <va-card
                square
                outlined
            >
                <va-card-title>
                    Add new document
                </va-card-title>

                <va-card-content>
                    <img 
                        class="w-32 mx-auto rounded-full border-8 border-white" 
                        src="@/assets/images/add_document.png"
                        alt="" 
                    />
                    <div class="text-center text-3xl font-medium">
                        Create document
                    </div>
                </va-card-content>
            </va-card>
        </div>

        <div
            v-else-if="user.error"
        >
            Error!
        </div>

        <div
            v-else
        >
            Loading...
        </div>
    </div>
</template>

<script setup>
import { onBeforeMount } from 'vue';
import useUserService from "@/services/userService.js";
import ProfileCard from '@/components/ProfileCard.vue';

const userService = useUserService();
const user = userService.userState;

onBeforeMount(async () => {
    try {
        await userService.getUserInfo();
    } catch (error) {
        console.error(error);
    }
});
</script>