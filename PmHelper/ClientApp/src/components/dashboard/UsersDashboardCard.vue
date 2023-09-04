<template>
    <va-card>
        <va-card-title>
            Users
        </va-card-title>

        <va-card-content>
            <div v-if="isUsersLoading">
                loading...
            </div>

            <div v-else>
                <div v-if="users != null">
                    Users count: {{ users.length }}
                </div>

                <div v-else>
                    ERROR
                </div>
            </div>
        </va-card-content>
    </va-card>
</template>

<script setup>
import { ref, onBeforeMount } from 'vue';
import useUserService from '@/services/userService.js';

const userService = useUserService();
const isUsersLoading = ref(false);
const users = ref(null);

const getAllUsers = async () => {
    try {
        users.value = null;
        isUsersLoading.value = true;

        users.value = await userService.getAllUsers();

        isUsersLoading.value = false;
    } catch (error) {
        console.error('UserDashboardCard::getAllUsers: EXCEPTION: ', error);

        users.value = null;
        isUsersLoading.value = false;
    }
}

onBeforeMount(async () => {
    await getAllUsers();
});
</script>