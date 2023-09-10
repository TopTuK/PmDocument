<template>
    <va-card square outlined>
        <va-card-title>
            <div class="flex flex-row">
                Documents
            </div>
        </va-card-title>

        <va-card-content>
            <div v-if="isStatisticLoading">
                loading...
            </div>

            <div v-else>
                <div v-if="docStatistic != null">
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

        <va-card-actions v-if="(!isStatisticLoading)">
            <div v-if="(docStatistic != null)">
                <va-button @click="getDocumentStatistic">
                    Refresh
                </va-button>
                <va-button
                    :to="{ name: 'DashboardDocuments' }"
                >
                    Manage
                </va-button>
            </div>
        </va-card-actions>
    </va-card>
</template>

<script setup>
import { ref, onBeforeMount, computed, watch } from 'vue';
import useDocumentService from '@/services/documentService.js';

const documentService = useDocumentService();
const isStatisticLoading = ref(false);
const docStatistic = ref(null);

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

const chartSeries = computed(() => {
    if (docStatistic.value == null) {
        return [];
    }

    return docStatistic.value
        .userDocumentsStatistic
        .map((ud) => ud.count);
});

watch(() => docStatistic.value, (docStat, _) => {
    if (docStat != null) {
        let labels = docStat.userDocumentsStatistic
            .map((ud) => ud.user.email);

        chartOptions.labels = labels;
    }
});

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