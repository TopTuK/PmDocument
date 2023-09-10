import axios from 'axios';
import { DEBUG  } from '@/config';

export default function useDocumentService() {
    const GET_DOCUMENT_STATISTIC_ACTION = "/document/GetDocumentsStatistic";
    const GET_DOCUMENT_TYPES_ACTION = "/document/GetDocumentsTypes";
    const GET_DOCUMENT_PROMT_TYPE_ACTION = "/document/GetDocumentPromtType";

    async function getDocumentStatistic() {
        console.log('DocumentService::getDocumentStatistic: start getting statistic of documents');

        try {
            let response = await axios.get(GET_DOCUMENT_STATISTIC_ACTION);

            // Check for bad request status
            if (response.status !== 200) {
                console.error('DocumentService::getDocumentStatistic: http request error: ', response.status);
                return null;
            }
            else {
                const documentsStatistic = response.data;
                
                console.log('DocumentService::getDocumentStatistic: total documents count: ', documentsStatistic.totalDocuments);
                if (DEBUG) {
                    console.log('DocumentService::getDocumentStatistic: UserDocumentsStatistic: ', documentsStatistic.userDocumentsStatistic);
                }

                return documentsStatistic;
            }
        }
        catch (error) {
            console.error('DocumentService::getDocumentStatistic: exception raised', ex);
            return null;
        }
    }

    async function getDocumentsTypes() {
        console.log('DocumentService::getDocumentsTypes: start getting documents types');

        try {
            let response = await axios.get(GET_DOCUMENT_TYPES_ACTION);

            if (response.status !== 200) {
                console.error('DocumentService::getDocumentsTypes: http request error: ', response.status);
                return null;
            }
            else {
                const documentTypes = response.data;
                console.log('DocumentService::getDocumentsTypes: total documents types: ', documentTypes.length);

                return documentTypes;
            }
        }
        catch (error) {
            console.error('DocumentService::getDocumentsTypes: exception raised', error);
            return null;
        }
    }

    async function getDocumentPromtType(documentTypeId) {
        console.log(`DocumentService::getDocumentPromtType: start get document promt type. Id=${documentTypeId}`);

        try {
            let response = await axios.get(GET_DOCUMENT_STATISTIC_ACTION);
        }
        catch (error) {

        }
    }

    return {
        getDocumentStatistic,
        getDocumentsTypes, getDocumentPromtType,
    };
};