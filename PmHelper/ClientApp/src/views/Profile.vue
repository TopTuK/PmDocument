<template>
    <div class="flex flex-col justify-center items-center">
        <div 
            class="relative shrink card mx-auto bg-white shadow-xl hover:shadow"
            v-if="!user.error && !user.loading"
        >
            <img 
                class="w-32 mx-auto rounded-full -mt-20 border-8 border-white" 
                src="https://avatars.githubusercontent.com/u/67946056?v=4"
                alt="" />

            <div class="text-center mt-2 text-3xl font-medium">
                {{ user.userInfo.firstName }}&nbsp;{{ user.userInfo.lastName }}
            </div>

            <div class="text-center mt-2 font-light text-sm">
                {{ user.userInfo.email }}
            </div>
            <!--
            <div class="text-center font-normal text-lg">Kerala</div>
            <div class="px-6 text-center mt-2 font-light text-sm">
                <p>
                    Front end Developer, avid reader. Love to take a long walk, swim
                </p>
            </div>
            -->

            <hr class="mt-8" />

            <div class="flex justify-center items-center gap-3 p-4">
                <va-button color="danger">Remove profile</va-button>
                <va-button color="warning">Log out</va-button>
            </div>

            <div
                v-show="!isAuthenticated" 
                class="flex">
                NOT authenticated
            </div>
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
import useAuthService from "@/services/authService.js";
import useUserService from "@/services/userService.js";

const authService = useAuthService();
const isAuthenticated = authService.isAuthenticated();

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