<script lang="ts" setup>
import router from '@/router';
import { fetchWrapper } from '@/helpers/fetch-wrapper';
import { ref, onMounted } from 'vue';
import type { IOrder, IResult } from '@/models';
import { Format } from '@/helpers/format';
import { useConfirm, useToast } from 'primevue';

const dt = ref();

const keyword = ref<string>("");
const date = ref<Date>(null);
const savedKeyword = ref<string>("");
const savedDate = ref<Date>(null);

const confirm = useConfirm();
const toast = useToast();

// eslint-disable-next-line no-undef
const orders = ref<Array<IOrder>>([])

const search = async (reload: boolean = false) => {
  const result = await fetchWrapper.post('/api/Sales/Search', { keyword: reload ? savedKeyword.value : keyword.value, date: reload ? savedDate.value : date.value }) as IResult<IOrder>;
  if (result.status){
    orders.value = result.data;

    savedKeyword.value = reload ? savedKeyword.value : keyword.value;
    savedDate.value = reload ? savedDate.value : date.value;
  }
}

const deleteData = async (data: IOrder) => {
  confirm.require({
    message: 'Are you sure want to delete data?',
    header: 'Confirmation',
    icon: 'pi pi-exclamation-triangle',
    rejectProps: {
      label: 'Cancel',
      severity: 'secondary',
      outlined: true
    },
    acceptProps: {
      label: 'Save'
    },
    accept: async () => {
      const result = await fetchWrapper.delete('/api/Sales', data);
      if (result.status){
        toast.add({ severity: 'success', summary: 'Info', detail: 'Data berhasil dihapus', life: 3000 });
        await refresh();
      } else {
        toast.add({ severity: 'error', summary: 'Info', detail: `Data gagal dihapus. Pesan : ${result.message}`, life: 3000 });
      }
    },
    reject: () => {
      console.log('Reject Delete');
    }
  })
}

const goEdit = (data: IOrder) => {
  router.push(`/edit/${data.id}`);
}

const goAdd = () => {
  router.push('/add')
}

const loadData = async () => {
  const result = await fetchWrapper.get('/api/Sales') as IResult<IOrder>;
  if (result.status){
    orders.value = result.data;
  }
}

const refresh = () => {
  if ((savedKeyword.value == null || savedKeyword.value.length == 0) && savedDate.value == null)
    loadData();
  else
    search(true);
}

const exportCSV = () => {
    dt.value.exportCSV();
};

onMounted(async () => {
  const result = await fetchWrapper.get('/api/Sales') as IResult<IOrder>;
  if (result.status){
    orders.value = result.data;
  }
})
</script>

<template>
  <Card>
    <template #title>Sales Order</template>
    <template #content>
      <div class="pt-5 flex flex-col gap-5">
        <Card>
          <template #content>
            <div class="flex flex-col gap-3">
              <div class="grid grid-cols-2 gap-5">
                <div class="flex flex-col gap-2">
                  <span>Keyword</span>
                  <div class="w-full">
                    <InputText id="username" :fluid="true" v-model="keyword" aria-describedby="username-help" />
                  </div>
                </div>
                <div class="flex flex-col gap-2">
                  <span>Date</span>
                  <div class="w-full">
                    <DatePicker v-model="date" :fluid="true"/>
                  </div>
                </div>
              </div>
              <div class="flex justify-end">
                <Button label="Search" @click="search()" icon="pi pi-search" iconPos="left"/>
              </div>
            </div>
          </template>
        </Card>
        <Card>
          <template #content>
            <DataTable ref="dt" :value="orders" paginator :rows="5" :rowsPerPageOptions="[5, 10, 20, 50]" tableStyle="min-width: 50rem">
              <template #header>
                <div class="flex flex-wrap items-center justify-between gap-2">
                  <div class="flex gap-2">
                    <Button label="Add Order" @click="goAdd()" icon="pi pi-plus" iconPos="left"/>
                    <Button label="Export" @click="exportCSV"  icon="pi pi-file-export" iconPos="left"/>
                  </div>
                  <Button label="Hints" :hidden="true"/>
                </div>
              </template>
              <Column field="id" header="No"></Column>
              <Column field="key" header="Sales Order"></Column>
              <Column field="date" header="Order Date">
                <template #body="{ data }">
                    {{ Format.Date({date: data.date, separator: '/'}) }}
                </template>
              </Column>
              <Column field="customer.name" header="Customer"></Column>
              <Column header="Action" style="width: 100px">
                <template #body="{ data }">
                  <div class="flex gap-2">
                    <Button icon="pi pi-pencil" size="small" severity="warn" @click="goEdit(data)" />
                    <Button icon="pi pi-trash" size="small" severity="danger" @click="deleteData(data)" />
                  </div>
                </template>
              </Column>
            </DataTable>
          </template>
        </Card>
      </div>
    </template>
  </Card>
</template>
