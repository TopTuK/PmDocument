<template>
    <va-card
        square
        outlined
    >
<!--
        <va-card-title>
            {{ $t('documents.create_document_card_title') }}
        </va-card-title>
-->

        <va-card-content>
            <div class="flex flex-col items-center justify-center">
                <img 
                    class="w-32 mx-auto rounded-full border-8 border-white" 
                    src="@/assets/images/add_document.png"
                    alt="" 
                />

                <va-select
                    class="mt-6"
                    v-model="document_value"
                    :options="document_types"
                    :text-by="(option) => $t(`documents_types.${option.title}`)"
                />

                <va-button
                    color="info"
                    gradient
                    size="large"
                    class="mt-6"
                    :to="{
                        name: 'CreateDocument',
                        params: {
                            type_id: document_value.id
                        }
                    }"
                >
                    {{ $t('documents.create_document_button_title') }}
                </va-button>
            </div>
            
        </va-card-content>
    </va-card>
</template>

<script setup>
import { ref } from 'vue';
import document_types from '@/models/documents_types.js';

const document_value = ref(document_types[0]);
</script>