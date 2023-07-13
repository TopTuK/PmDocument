import axios from 'axios';
import { reactive } from 'vue';

export default function useDocumentService() {
    // Creating reactive object to hold documents types and loading state
    const documentsTypesState = reactive({
        documentsTypes: [],
        loading: false,
        error: false,
    });

    // Get Documents Types
    async function getDocumentsTypes() {
        try {
            documentsTypesState.error = false;
            documentsTypesState.loading = true;

            const response = await axios.get('/documents/getdocumentstypes');
            if (response.status !== 200) {
                documentsTypesState.error = true;
            }
            else {
                documentsTypesState.documentsTypes = response.data;
            }

            documentsTypesState.loading = false;
        }
        catch (error) {
            documentsTypesState.loading = false;
            documentsTypesState.error = true;

            throw error;
        }
    }

    return {
        documentsTypesState, getDocumentsTypes,
    }
}