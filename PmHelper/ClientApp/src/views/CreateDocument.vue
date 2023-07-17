<template>
    <div class="flex flex-col justify-center items-center mx-5 my-5">
        <div class="container mx-auto my-5">
            <FormWizard
                @on-complete="onComplete"
                color="#dd9c1d"
                step-size="md"
            >
                <TabContent title="Document type">
                    <va-card
                        color="primary"
                        gradient
                    >
                        <va-card-title>
                            {{ $t(`documents_types.${document.title}`) }}
                        </va-card-title>

                        <va-card-content>
                            <p class="mt-4">
                                {{ $t(`documents_types.${document.text}`) }}
                            </p>
                        </va-card-content>
                    </va-card>
                </TabContent>

                <TabContent title="Generate document">
                    <section class="prose max-w-none">
                        <h2 class="text-3xl mt-8">Generate Project Charter</h2>
                        <div class="my-8">
                            <p><b>What do you need Project Charter for?</b></p>
                            <textarea placeholder="Describe your project or idea with a few words..." class="textarea textarea-bordered textarea-lg w-full max-w-2xl h-36"></textarea>
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