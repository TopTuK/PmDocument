<template>
    <div class="p-4 grid grid-cols-1 lg:grid-cols-2 gap-14">
        <va-card>
            <va-card-title>
                {{ $t('documents_types.user_story_title') }}
            </va-card-title>

            <va-card-content>
                <div 
                    class="max-w-3xl"
                    v-html="$t('documents_types.user_story_detailed_description')"
                />
            </va-card-content>
        </va-card>

        <va-card>
            <va-card-content>
                <img 
                    src="@/assets/images/documents/user-story.webp" 
                    class="bg-base-100"
                    alt="User story" 
                />
            </va-card-content>
        </va-card>
    </div>
</template>

<script setup>
</script>