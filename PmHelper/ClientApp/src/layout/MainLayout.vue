<script setup>
import Header from '@/components/Header.vue'
import Footer from '@/components/Footer.vue'
</script>

<template>
    <div class="flex flex-col h-screen bg-gray-400 tracking-normal leading-normal">
        <div class="flex-none">
            <Header />
        </div>

        <div class="flex flex-1 bg-slate-500">
            <slot />
        </div>

        <div class="flex-none">
            <Footer />
        </div>
    </div>
</template>