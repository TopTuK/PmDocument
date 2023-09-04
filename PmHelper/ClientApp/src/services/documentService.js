import axios from 'axios';
import { DEBUG  } from '@/config';

export default function useDocumentService() {
    const GET_DOCUMENT_STATISTIC = "/document/GetDocumentsStatistic";

    async function getDocumentStatistic() {
        console.log('DocumentService::getDocumentStatistic: start getting statistic of documents');

        try {
            let response = await axios.get(GET_DOCUMENT_STATISTIC);

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

    return {
        getDocumentStatistic,
    };
};