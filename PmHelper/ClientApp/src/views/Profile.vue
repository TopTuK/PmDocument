<template>
    <div class="flex flex-col justify-center items-center my-14">
        <div class="relative shrink card mx-auto bg-white shadow-xl hover:shadow">
            <img 
                class="w-32 mx-auto rounded-full -mt-20 border-8 border-white" 
                src="https://avatars.githubusercontent.com/u/67946056?v=4"
                alt="" />
            <div class="text-center mt-2 text-3xl font-medium">Ajo Alex</div>
            <div class="text-center mt-2 font-light text-sm">@devpenzil</div>
            <div class="text-center font-normal text-lg">Kerala</div>
            <div class="px-6 text-center mt-2 font-light text-sm">
                <p>
                    Front end Developer, avid reader. Love to take a long walk, swim
                </p>
            </div>

            <hr class="mt-8" />

            <div class="flex justify-center items-center gap-3 p-4">
                <va-button preset="danger">Remove profile</va-button>
                <va-button color="warning">Log out</va-button>
            </div>

            <div
                v-show="!isAuthenticated" 
                class="flex">
                NOT authenticated
            </div>
        </div>
    </div>
</template>

<script setup>
import useAuthService from "@/services/authService.js";

const authService = useAuthService();
const isAuthenticated = authService.isAuthenticated();
</script>