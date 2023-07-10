<template>
    <va-card>
        <va-card-title>
            <img 
                class="w-32 mx-auto rounded-full border-8 border-white" 
                src="@/assets/images/user_profile.png"
                alt="" 
            />
        </va-card-title>

        <va-card-content>
            <div class="text-center text-3xl font-medium">
                {{ props.userInfo.firstName }}&nbsp;{{ props.userInfo.lastName }}
            </div>
            
            <div class="text-center mt-2 font-light text-sm">
                {{ props.userInfo.email }}
            </div>

            <div class="flex justify-center mt-2 items-center gap-3 p-2">
                <va-button
                    color="warning"
                    href="/auth/logout"
                >
                    Log out
                </va-button>
            </div>
        </va-card-content>
    </va-card>
</template>

<script setup>
const props = defineProps({
    userInfo: {
        type: Object,
        required: true,
    },
});
</script>