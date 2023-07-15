import axios from 'axios';
import { reactive } from 'vue';
import { DEBUG  } from '@/config';

export default function useDocumentService() {
    const GET_USER_DOCUMENTS_ACTION = "/document/GetCurrentUserDocuments";

    // Creating reactive object to hold user documents and loading state
    const userDocuments = reactive({
        documents: [],
        loading: false,
        error: false,
    });

    // Get user documents
    async function getUserDocuments() {
        if (DEBUG) {
            console.log("DocumentService::getUserDocuments: start getting current user documents");
        }

        try {
            userDocuments.error = false;
            userDocuments.loading = true;

            var response = await axios.get(GET_USER_DOCUMENTS_ACTION);

            if (DEBUG) {
                console.log(`DocumentService::getUserDocuments: got response with status=${response.status}`);
            }

            // Check for bad request status
            if (response.status !== 200) {
                console.log("DocumentService::getUserDocuments: ERROR: can't get user docuemnts");

                userDocuments.error = true;
                userDocuments.documents = [];
            }
            else {
                if (DEBUG) {
                    console.log("DocumentService::getUserDocuments: userDocuments=", response.data);
                }

                userDocuments.documents = response.data;
            }

            userDocuments.loading = false;
        }
        catch (error) {
            console.log("DocumentService::getUserDocuments: EXCEPTION: ", error);

            userDocuments.error = true;
            userDocuments.loading = false;

            throw error;
        }
    }

    return {
        userDocuments, getUserDocuments,
    }
}