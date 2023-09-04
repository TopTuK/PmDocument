<template>
    <va-card>
        <va-card-title>
            {{ props.doc.title }}
        </va-card-title>
        
        <va-card-content>
            <div v-if="!isLoading">
                {{ requestText }}
            </div>

            <div v-else>
                <va-progress-circle indeterminate />
            </div>
        </va-card-content>

        <va-card-actions
            v-show="!isLoading"
            align="center" class="gap-3"
        >
            <va-button>
                {{ $t('documents.show_button_title') }}
            </va-button>

            <va-button @click="removeDocument">
                {{ $t('documents.remove_button_title') }}
            </va-button>
        </va-card-actions>
    </va-card>
</template>

<script setup>
import { computed, ref } from 'vue';
import { filter } from '../utils';
import { useDocumentStore } from '@/stores/documentsStore';

const props = defineProps({
    doc: {
        type: Object,
        required: true,
    },
});

const documentsStore = useDocumentStore();
const isLoading = ref(false);

const requestText = computed(() => {
    return filter(props.doc.requestText, 300);
});

const removeDocument = async () => {
    isLoading.value = true;

    await documentsStore.removeUserDocument(props.doc.id);

    isLoading.value = false;
}
</script>