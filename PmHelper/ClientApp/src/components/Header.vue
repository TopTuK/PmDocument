<template>
    <nav class="flex flex-row items-center justify-between h-16 bg-gray-800 p-2 w-full">        
        <div class="text-white font-extrabold">
            <router-link
                to="/"
                class="no-underline hover:text-white hover:no-underline">
                <span class="text-2xl pl-2">PM Helper</span>
            </router-link>
        </div>

        <div class="inline-flex gap-6 justify-end items-center">
            <div>
                <router-link
                    to="/documents"
                    class="items-center px-4 py-2 border border-transparent text-base font-medium rounded-md text-white bg-gray-700 hover:bg-gray-600 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-gray-500"
                >
                    Documents
                </router-link>
            </div>

            <div>
                <router-link
                    v-if="!isAuthenticated"
                    to="/login"
                    class="items-center px-4 py-2 border border-transparent text-base font-medium rounded-md text-white bg-gray-700 hover:bg-gray-600 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-gray-500"
                >
                    Sign In
                </router-link>
                <router-link
                    v-else
                    to="/profile"
                    class="items-center px-4 py-2 border border-transparent text-base font-medium rounded-md text-white bg-gray-700 hover:bg-gray-600 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-gray-500"
                >
                    Profile
                </router-link>
            </div>
        </div>
    </nav>
</template>

<script setup>
import { computed } from "vue";
import useAuthService from "@/services/authService.js";

const authService = useAuthService();
const isAuthenticated = computed(() => {
    return authService.isAuthenticated();
});
</script>