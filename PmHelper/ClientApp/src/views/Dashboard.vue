<template>
    <div class="flex flex-col justify-center items-center mx-5 my-5">
        <div class="grid gap-4 lg:grid-cols-3 grid-cols-2">
            <UsersDashboardCard />
            <DocumentsDashboardCard />
        </div>
    </div>
</template>

<script setup>
import UsersDashboardCard from '@/components/dashboard/UsersDashboardCard.vue';
import DocumentsDashboardCard from '@/components/dashboard/DocumentsDashboardCard.vue';
</script>