<template>
    <nav class="flex flex-row items-center justify-between h-16 bg-gray-800 p-2 w-full">        
        <div class="text-white font-extrabold">
            <router-link
                to="/"
                class="no-underline hover:text-white hover:no-underline">
                
                <span class="text-2xl pl-2">
                    {{ $t('common.app_name') }}
                </span>

            </router-link>
        </div>

        <div class="inline-flex gap-6 justify-end items-center">
            <div>
                <router-link
                    to="/documents"
                    class="items-center px-4 py-2 border border-transparent text-base font-medium rounded-md text-white bg-gray-700 hover:bg-gray-600 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-gray-500"
                >
                    {{ $t('common.document_button_title') }}
                </router-link>
            </div>

            <div 
                v-if="isAuthenticated"
                class="flex gap-5"
            >
                <router-link
                    to="/profile"
                    class="items-center px-4 py-2 border border-transparent text-base font-medium rounded-md text-white bg-gray-700 hover:bg-gray-600 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-gray-500"
                >
                    {{ $t('common.profile_button_title') }}
                </router-link>

                <va-button
                    color="warning"
                    href="/auth/logout"
                >
                    {{ $t('common.logout_button_title') }}
                </va-button>
            </div>

            <div v-else>
                <router-link
                    to="/login"
                    class="items-center px-4 py-2 border border-transparent text-base font-medium rounded-md text-white bg-gray-700 hover:bg-gray-600 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-gray-500"
                >
                    {{ $t('common.signin_button_title') }}
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