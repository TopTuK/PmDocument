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
            <router-link
                to="/login"
                class="items-center px-4 py-2 border border-transparent text-base font-medium rounded-md text-white bg-gray-700 hover:bg-gray-600 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-gray-500"
            >
                Sign In
            </router-link>
        </div>
    </nav>
</template>