<template>
    <div class="flex flex-col justify-center items-center mx-5 my-5">
        <div class="container mx-auto my-5">
            <FormWizard
                @on-complete="onComplete"
                color="#dd9c1d"
                step-size="md"
                :disable-back="true"
                :disable-back-on-click-step="true"
                :back-button-text="$t('common.back_button_title')"
                :next-button-text="$t('common.next_button_title')"
                :finish-button-text="$t('common.finish_button_title')"
            >
                <template v-slot:footer="props">
                    <div class="wizard-footer-left">
                        <va-button
                            v-if="props.activeTabIndex > 0 && !props.isLastStep"
                            :style="props.fillButtonStyle"
                            @click.native="props.prevTab()"
                            class="wizard-button"
                        >
                            {{ $t('common.back_button_title') }}
                        </va-button>
                    </div>

                    <div class="wizard-footer-right">
                        <va-button
                            v-if="!props.isLastStep"
                            @click.native="props.nextTab()"
                            class="wizard-button"
                            :style="props.fillButtonStyle"
                        >
                            {{ $t('common.next_button_title') }}
                        </va-button>

                        <va-button
                            v-else-if="generateDocumentState.document !== null"
                            @click.native="onComplete"
                            class="finish-button"
                            :style="props.fillButtonStyle"
                        >
                            {{ $t('common.finish_button_title') }}
                        </va-button>
                    </div>
                </template>

                <TabContent :title="$t('documents_types.document_type_title')">
                    <section class="prose max-w-none h-72">
                        <h2 class="text-3xl mt-6">
                            {{ $t(`documents_types.${document.title}`) }}
                        </h2>

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
                        <h2 class="text-4xl mt-4">
                            {{ $t(`documents_types.${document.create_title}`) }}
                        </h2>

                        <div class="flex flex-col my-8">
                            <b>{{ $t(`documents_types.${document.create_descrciption}`) }}</b>

                            <va-input
                                class="mt-2 w-full max-w-2xl"
                                v-model="documentName"
                                :placeholder="$t('documents_types.docuemnt_name_title')"
                            />

                            <textarea
                                v-model="requestText"
                                :placeholder="$t(`documents_types.${document.create_placeholder}`)"
                                class="mt-2 textarea textarea-bordered textarea-lg w-full max-w-2xl h-32"/>
                        </div>
                    </section>
                </TabContent>

                <TabContent 
                    :title="$t('documents_types.document_generating_title')"
                    :lazy="true"
                    :after-change="generateDocument"
                >
                    <section class="prose max-w-none">
                        <div
                            v-if="generateDocumentState.isLoading"
                            class="flex flex-col items-center justify-items-center justify-center"
                        >
                            <p>Loading...</p>
                            <va-progress-bar indeterminate />
                        </div>

                        <div
                            v-else-if="generateDocumentState.isError"
                        >
                            <p>ERROR</p>
                        </div>

                        <div
                            v-else-if="generateDocumentState.document != null"
                        >
                            <Markdown
                                :source="generateDocumentState.document.content"
                                class="p-2 m-2 bg-gray-200"
                            />
                        </div>
                    </section>
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
// https://www.npmjs.com/package/vue3-markdown-it
import { ref, reactive } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import { FormWizard, TabContent } from 'vue3-form-wizard';
import 'vue3-form-wizard/dist/style.css';
import document_types from '@/models/documents_types.js';
import useDocumentService from '@/services/documentService.js';
import Markdown from 'vue3-markdown-it';

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

const generateDocumentState = reactive({
    document: null,
    isLoading: false,
    isError: false,
});

const generateDocument = async () => {
    console.log("CreateDocument: start generating document");

    generateDocumentState.isLoading = true;
    generateDocumentState.isError = false;

    try {
        let userDocument = await generateUserDocument(document.id, documentName.value, requestText.value);
        if (userDocument != null) {
            generateDocumentState.document = userDocument;
        }
        else {
            generateDocumentState.document = null;
            generateDocumentState.isError = true;
        }

        generateDocumentState.isLoading = false;
    }
    catch (error) {
        console.error("CreateDocument: Exception!", JSON.stringify(error));
        generateDocumentState.isError = true;
    }
};

const router = useRouter();
const onComplete = () => {
    alert('Yay. Done!');
    router.push({ name: 'Profile' });
}
</script>