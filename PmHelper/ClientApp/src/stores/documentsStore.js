import { defineStore } from 'pinia';
import { reactive, ref } from 'vue';
import axios from 'axios';
import { DEBUG  } from '@/config';

export const useDocumentStore = defineStore('documentsStore', () => {
    const GET_USER_DOCUMENTS_ACTION = "/document/GetCurrentUserDocuments";
    const GENERATE_DOCUMENT_ACTION = "/document/GenerateDocument";
    const REMOVE_USER_DOCUMENT_ACTION = "/document/RemoveUserDocument";

    const userDocumentsState = reactive({
        documents: [],
        isLoading: false,
        error: null,
    });

    async function getUserDocuments() {
        if (DEBUG) {
            console.log("DocumentStore::getUserDocuments: start getting current user documents");
        }

        try {
            userDocumentsState.error = null;
            userDocumentsState.isLoading = true;

            let response = await axios.get(GET_USER_DOCUMENTS_ACTION);
            if (DEBUG) {
                console.log(`DocumentStore::getUserDocuments: got response with status=${response.status}`);
            }

            // Check for bad request status
            if (response.status !== 200) {
                console.error("DocumentStore::getUserDocuments: ERROR: can't get user docuemnts");

                userDocumentsState.error = { isError: true, message: "HTTP Error" };
                userDocumentsState.documents = [];
            }
            else {
                let docs = response.data;

                console.log("DocumentStore::getUserDocuments: user documents length is: ", docs.length);
                userDocumentsState.documents = docs;
            }

            userDocumentsState.isLoading = false;
        }
        catch (ex) {
            console.error("DocumentStore::getUserDocuments: EXCEPTION: ", error);

            userDocumentsState.error = { isError: true, message: "Exception raised" };
            userDocumentsState.isLoading = false;
        }
    }

    // Remove user document
    async function removeUserDocument(documentId) {
        console.warn(`DocumentStore::removeUserDocument: removing user document with id=${documentId}`);

        try {
            const queryParams = {
                'documentId' : documentId
            }

            let response = await axios.get(REMOVE_USER_DOCUMENT_ACTION, { params: queryParams });
            
            if (DEBUG) {
                console.log(`DocumentStore::removeUserDocument: got response with status=${response.status}`);
            }

            if (response.status !== 200) {
                console.log("DocumentStore::removeUserDocument: ERROR: can't remove user document");
                return false;
            }
            else {
                // TODO: Correct filter user documents
                console.log(`DocumentStore::removeUserDocument: successfully removed document with id=${documentId}`);
                userDocumentsState.documents = userDocumentsState.documents.filter(doc => doc.id != documentId);

                return true;
            }
        }
        catch (ex) {
            console.error("DocumentStore::removeUserDocument: EXCEPTION: ", ex);
            return false;
        }
    }

    // Generate user document
    async function generateUserDocument(documentTypeId, documentName, requestText) {
        if (DEBUG) {
            console.log("DocumentStore::generateUserDocument: start generating user document. TypeId=", documentTypeId);
        }

        try {
            const requestBody = {
                typeId: documentTypeId,
                name: documentName,
                text: requestText,
            }

            let response = await axios.post(GENERATE_DOCUMENT_ACTION, requestBody);
            if (DEBUG) {
                console.log(`DocumentStore::generateUserDocument: got response with status=${response.status}`);
            }

            // Check for bad request status
            if (response.status !== 200) {
                console.error("DocumentStore::generateUserDocument: ERROR: can't generate user document");
                return null;
            }
            else {
                const doc = response.data;
                console.log("DocumentStore::generateUserDocument: sucessfully generated new user document");

                return doc;
            }
        }
        catch {
            console.error("DocumentStore::generateUserDocument: EXCEPTION: ", error);
            throw error;
        }
    }

    return {
        userDocumentsState, getUserDocuments,
        generateUserDocument,
        removeUserDocument,
    };
});