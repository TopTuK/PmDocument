import axios from 'axios';
import { reactive } from 'vue';
import { DEBUG  } from '@/config';

export default function useDocumentService() {
    const GET_USER_DOCUMENTS_ACTION = "/document/GetCurrentUserDocuments";
    const GENERATE_DOCUMENT_ACTION = "/document/GenerateDocument";

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

            let response = await axios.get(GET_USER_DOCUMENTS_ACTION);

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

    // Create document
    async function generateUserDocument(typeId, text) {
        if (DEBUG) {
            console.log("DocumentService::generateUserDocument: start generating user document");
            console.log(`DocumentType=${typeId}. Text=${text}`);
        }

        try {
            const requestBody = {
                typeId: typeId,
                text: text,
            }

            let response = await axios.post(GENERATE_DOCUMENT_ACTION, requestBody);

            if (DEBUG) {
                console.log(`DocumentService::generateUserDocument: got response with status=${response.status}`);
            }

            // Check for bad request status
            if (response.status !== 200) {
                console.log("DocumentService::generateUserDocument: ERROR: can't generate user document");
                return null;
            }
            else {
                if (DEBUG) {
                    console.log("DocumentService::generateUserDocument: response data=", response.data);
                }

                return response.data;
            }
        }
        catch (error) {
            console.log("DocumentService::generateUserDocument: EXCEPTION: ", error);
            throw error;
        }
    }

    return {
        userDocuments, getUserDocuments,
        generateUserDocument,
    }
}