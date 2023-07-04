<template>
    <div class="flex flex-col justify-center items-center mx-5 my-5">

        <div class="grid gap-4 lg:grid-cols-3 grid-cols-2">
            <DocumentCard
                v-for="doc in document_types"
                :doc="doc"
            />

            <!--TO DO CARD-->
            <va-card
                color="secondary"
                stripe
                stripe-color="success"
            >
                <va-card-title>Can't find the desired document?</va-card-title>
                <va-card-content>
                    <p>
                        Tell us which document you want to generate, and we will do our best to create it 👇
                    </p>

                    <div
                        v-if="isAuthicated"
                        class="mt-6 flex flex-col items-center justify-center"
                    >
                        <va-button preset="primary">
                            Tell us more
                        </va-button>
                    </div>

                    <div 
                        class="mt-6 flex flex-col items-center justify-center"
                        v-else
                    >
                        <va-button preset="primary">
                            Sign In
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
import useAuthService from '@/services/authService.js';

const authService = useAuthService();
const isAuthicated = authService.isAuthenticated();
</script>