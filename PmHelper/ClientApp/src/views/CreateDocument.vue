<template>
    <div class="flex flex-col justify-center items-center mx-5 my-5">
        <div class="container mx-auto my-5">
            <FormWizard
                @on-complete="onComplete"
                color="#dd9c1d"
                step-size="md"
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
                        <h2 class="text-3xl mt-6">{{ $t(`documents_types.${document.create_title}`) }}</h2>
                        <div class="my-8">
                            <p><b>{{ $t(`documents_types.${document.create_descrciption}`) }}</b></p>
                            <textarea
                                :placeholder="$t(`documents_types.${document.create_placeholder}`)"
                                class="textarea textarea-bordered textarea-lg w-full max-w-2xl h-36" />
                        </div>
                    </section>
                </TabContent>

                <TabContent title="">
                    Yuhuuu! This seems pretty damn simple
                </TabContent>
            </FormWizard>
        </div>
    </div>
</template>

<script setup>
// https://vue3-form-wizard-document.netlify.app/demos/#simple
// https://www.koderhq.com/tutorial/vue/composition-api-router-routing/
import { useRoute } from 'vue-router';
import { FormWizard, TabContent } from 'vue3-form-wizard';
import 'vue3-form-wizard/dist/style.css';
import document_types from '@/models/documents_types.js';

const route = useRoute();
const document = document_types.find((doc) => doc.id == route.params.type_id);

const onComplete = () => {
    alert('Yay. Done!');
}
</script>