<template>
    <div class="flex flex-col justify-center items-center mx-5 my-5">

        <div class="grid gap-4 lg:grid-cols-3 grid-cols-2">
            <DocumentCard
                v-for="doc in document_types"
                :doc="doc"
                :canCreate="isAuthicated"
            />

            <!--TO DO CARD-->
            <va-card
                color="secondary"
                stripe
                stripe-color="success"
            >
            
                <va-card-title>
                    {{ $t('documents.new_document_title') }}
                </va-card-title>

                <va-card-content>
                    <p>
                        {{ $t('documents.new_document_description') }}
                    </p>

                    <div
                        v-if="isAuthicated"
                        class="mt-6 flex flex-col items-center justify-center"
                    >
                        <va-button preset="primary">
                            {{ $t('documents.new_document_button_title') }}
                        </va-button>
                    </div>

                    <div 
                        class="mt-6 flex flex-col items-center justify-center"
                        v-else
                    >
                        <va-button 
                            preset="primary"
                            to="/login"
                        >
                            {{ $t('common.signin_button_title') }}
                        </va-button>
                    </div>
                </va-card-content>
            </va-card>
        </div>
    </div>
</template>

<script setup>
import DocumentCard from '@/components/DocumentCard.vue';
import document_types from '@/models/documents_types.js';
import { useUserStore } from '@/stores/userStore';

const userStore = useUserStore();
const isAuthicated = userStore.isAuthenticated();
</script>