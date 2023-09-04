<template>
    <va-card square outlined>
        <va-card-title>
            Documents
        </va-card-title>

        <va-card-content>
            <div v-if="isStatisticLoading">
                loading...
            </div>

            <div v-else>
                <div v-if="docStatistic != null">
                    TOTAL DOCUMENTS: {{ docStatistic.totalDocuments }}

                    <apexchart 
                        width="380"
                        type="pie"
                        :options="chartOptions"
                        :series="chartSeries"
                    />
                </div>

                <div v-else>
                    ERROR
                </div>
            </div>
        </va-card-content>
    </va-card>
</template>

<script setup>
import { ref, onBeforeMount, computed } from 'vue';
import useDocumentService from '@/services/documentService.js';

const documentService = useDocumentService();
const isStatisticLoading = ref(false);
const docStatistic = ref(null);

const chartSeries = computed(() => {
    if (docStatistic.value == null) {
        return [];
    }

    return docStatistic.value
        .userDocumentsStatistic
        .map((ud) => ud.count);
});

const chartLabels = computed(() => {
    if (docStatistic.value == null) {
        return [];
    }

    return docStatistic.value
        .userDocumentsStatistic
        .map((ud) => ud.user.email);
});

const chartOptions = {
    chart: {
        width: 380,
        type: 'pie',
    },
    labels: [],
    responsive: [{
        breakpoint: 480,
        options: {
            chart: {
                width: 200
            },
            legend: {
                position: 'bottom'
            }
        },
    }],
};

const getDocumentStatistic = async () => {
    try {
        docStatistic.value = null;
        isStatisticLoading.value = true;

        docStatistic.value = await documentService.getDocumentStatistic();

        isStatisticLoading.value = false;
    } catch (error) {
        console.error('DocumentDashboardCard::getDocumentStatistic: EXCEPTION: ', error);

        docStatistic.value = null;
        isStatisticLoading.value = false;
    }
}

onBeforeMount(async () => {
    await getDocumentStatistic();
});
</script>