<template>
    <div class="flex flex-col justify-center items-center mx-5 my-5">
        <div class="container mx-auto my-5">
            <FormWizard
                @on-complete="onComplete"
                color="#dd9c1d"
                step-size="md"
                :back-button-text="$t('common.back_button_title')"
                :next-button-text="$t('common.next_button_title')"
                :finish-button-text="$t('common.finish_button_title')"
            >
                <TabContent :title="$t('documents_types.document_type_title')">
                    <section class="prose max-w-none h-72">
                        <h2 class="text-3xl mt-6">{{ $t(`documents_types.${document.title}`) }}</h2>
                        <div class="my-8">
                            <p>{{ $t(`documents_types.${document.text}`) }}</p>
                        </div>
                    </section>
                </TabContent>

                <TabContent :title="$t('documents_types.document_generate_title')">
                    <section class="prose max-w-none h-72">
                        <h2 class="text-4xl mt-6">{{ $t(`documents_types.${document.create_title}`) }}</h2>
                        <div class="my-8">
                            <p>
                                <b>{{ $t(`documents_types.${document.create_descrciption}`) }}</b>
                            </p>

                            <textarea
                                v-model="requestText"
                                :placeholder="$t(`documents_types.${document.create_placeholder}`)"
                                class="textarea textarea-bordered textarea-lg w-full max-w-2xl h-36" />
                        </div>
                    </section>
                </TabContent>

                <TabContent :title="$t('documents_types.document_generating_title')">
                    <va-button
                        @click="onGenerateDocument"
                    >
                        Generate
                    </va-button>
                </TabContent>
            </FormWizard>
        </div>
    </div>
</template>

<script setup>
// https://vue3-form-wizard-document.netlify.app/demos/#simple
// https://www.koderhq.com/tutorial/vue/composition-api-router-routing/
import { ref } from 'vue';
import { useRoute } from 'vue-router';
import { FormWizard, TabContent } from 'vue3-form-wizard';
import 'vue3-form-wizard/dist/style.css';
import document_types from '@/models/documents_types.js';
import useDocumentService from '@/services/documentService.js';

const route = useRoute();
const document = document_types.find((doc) => doc.id == route.params.type_id);

const requestText = ref('');

const { generateUserDocument } = useDocumentService();

const onGenerateDocument = async () => {
    var doc = await generateUserDocument(document.id, requestText.value);
};

const onComplete = () => {
    alert('Yay. Done!');
}
</script>