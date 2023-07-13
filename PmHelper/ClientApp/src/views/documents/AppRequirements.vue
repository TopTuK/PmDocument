<template>
    <div class="p-4 grid grid-cols-1 lg:grid-cols-2 gap-14">
        <va-card>
            <va-card-title>
                {{ $t('documents_types.app_requirements_title') }}
            </va-card-title>

            <va-card-content>
                <div 
                    class="max-w-3xl"
                    v-html="$t('documents_types.app_requirements_detailed_description')"
                />
            </va-card-content>
        </va-card>

        <va-card>
            <va-card-content>
                <img 
                    src="@/assets/images/documents/project-charter.webp" 
                    class="bg-base-100"
                    alt="Project charter" 
                />
            </va-card-content>
        </va-card>
    </div>
</template>

<script setup>
</script>