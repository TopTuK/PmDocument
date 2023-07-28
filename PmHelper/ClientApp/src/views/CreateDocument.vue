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

                <TabContent
                    :title="$t('documents_types.document_generate_title')"
                    :before-change="checkDocumentInfo"
                >
                    <section class="prose max-w-none h-72">
                        <h2 class="text-4xl mt-4">{{ $t(`documents_types.${document.create_title}`) }}</h2>
                        <div class="flex flex-col my-4">
                            <p>
                                <b>{{ $t(`documents_types.${document.create_descrciption}`) }}</b>
                            </p>

                            <va-input
                                class="w-full max-w-2xl"
                                v-model="documentName"
                                :placeholder="$t('documents_types.docuemnt_name_title')"
                            />

                            <textarea
                                v-model="requestText"
                                :placeholder="$t(`documents_types.${document.create_placeholder}`)"
                                class="mt-2 textarea textarea-bordered textarea-lg w-full max-w-2xl h-28" />
                        </div>
                    </section>
                </TabContent>

                <TabContent 
                    :title="$t('documents_types.document_generating_title')"
                    :lazy="true"
                >
                    <va-button
                        @click="onGenerateDocument"
                    >
                        Generate
                    </va-button>
                </TabContent>
            </FormWizard>
        </div>

        <va-modal
            v-model="isDocumentNameError"
        >
            NAME ERROR!
        </va-modal>

        <va-modal
            v-model="isRequestTextError"
        >
            REQUEST TEXT ERROR!
        </va-modal>
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

const documentName = ref('');
const requestText = ref('');

const isDocumentNameError = ref(false);
const isRequestTextError = ref(false);

const checkDocumentInfo = () => {
    if (documentName.value.trim() == "") {
        
        isDocumentNameError.value = true;
        return false;
    }

    if (requestText.value.trim() == "") {
        isRequestTextError.value = true;

        return false;
    }

    return true;
}

const { generateUserDocument } = useDocumentService();

const onGenerateDocument = async () => {
    var doc = await generateUserDocument(document.id, requestText.value);
};

const onComplete = () => {
    alert('Yay. Done!');
}
</script>