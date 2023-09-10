<template>
    <va-data-table
        class="m-6"
        :columns="columns"
        :items="items"
        :loading="isLoading"
    >
        <template #cell(id)="{ rowIndex }">
            {{  (rowIndex+1) }}
        </template>

        <template #cell(actions)="{ rowData }">
            <va-button
                preset="plain"
                icon="edit"
                :to="{
                    name: 'EditDocumentDashboard',
                    params: {
                        id: rowData.id
                    }
                }"
            />
        </template>
    </va-data-table>
</template>

<script setup>
import { ref, onBeforeMount, computed } from 'vue';
import useDocumentService from '@/services/documentService.js';

const columns = [
    { key: "id", label: "#", width: 80 },
    { key: "name", label: "Name" },
    { key: "actions", width: 80 },
];

const documentService = useDocumentService();

const isLoading = ref(false);
const documentTypes = ref(null);

const items = computed(() => {
    return (documentTypes.value != null)
        ? documentTypes.value
        : [];
});

const getDocumentTypes = async () => {
    isLoading.value = true;

    documentTypes.value = await documentService.getDocumentsTypes();

    isLoading.value = false;
}

onBeforeMount(async () => {
    await getDocumentTypes();
});
</script>