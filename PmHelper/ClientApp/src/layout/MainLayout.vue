<script setup>
import Header from '@/components/Header.vue'
// import Footer from '@/components/Footer.vue'
</script>

<template>
    <div class="bg-gray-400 leading-normal tracking-normal">
        <div class="h-screen">
            <Header />

            <main class="flex flex-wrap flex-col pt-3 items-center">
                <slot />
            </main>
        </div>
    </div>
</template>