<script lang="ts" setup>
import router from '@/router';
import { computed, onMounted, ref, watch } from 'vue';
import { fetchWrapper } from '@/helpers/fetch-wrapper';
import type { IOrder, ICustomer, IResult, IItem } from '@/models';
import { Format } from '@/helpers/format';
import { RandomNumber } from '@/helpers/random-key';
import { useToast } from 'primevue';

const toast = useToast();

const customers = ref<Array<ICustomer>>([])

const newItem = ref<IItem>({
  id: 0,
  orderId: 0,
  name: '',
  quantity: 0,
  price: 0
});

const newData = ref<IOrder>(
  {
    key: '',
    date: new Date(),
    address: '',
    customer: {
      id: 0,
      name: ''
    },
    items: []
  }
);

const goBack = () => {
  router.push('/');
}

const search = async (event) => {
  if (event.query.length == 0 && (newData.value.customer.name == null || newData.value.customer.name.length < 1))
    loadCustomer();
  else {
    if (event.query.length != 0) {
      const result = await fetchWrapper.get(`/api/Customer/Search/${event.query}`);
      if (result.status) {
        if (result.data.length > 0){
          customers.value = result.data
        }
        else
          customers.value = [{
            id: 0,
            name: event.query
          }]
      }
    } else {
      customers.value = customers.value;
    }
  }
}

const loadCustomer = async () => {
  const result = await fetchWrapper.get('/api/Customer') as IResult<ICustomer>;
  if (result.status) {
    customers.value = result.data
  }
}

const saveOrder = async () => {
  const result = await fetchWrapper.post('/api/Sales', newData.value) as IResult<IOrder>;
  if (result.status){
    toast.add({ severity: 'success', summary: 'Info', detail: 'Data Berhasil Ditambahkan', life: 3000 });
    goBack();
  } else {
    toast.add({ severity: 'error', summary: 'Info', detail: `Data Gagal Ditambahkan. Pesan : ${result.message}`, life: 3000 });
  }
}

const dialogItem = {
  Visible: ref<boolean>(false),
  IsEdit: ref<boolean>(false),
  Save: async () => {
    if (!dialogItem.IsEdit.value){
      newItem.value.id = RandomNumber()
      newData.value.items.push(newItem.value);
    } else {
      const editItem = newData.value.items.find((key) => key.id == newItem.value.id);
      if (editItem) {
        editItem.name = newItem.value.name,
        editItem.quantity = newItem.value.quantity,
        editItem.price = newItem.value.price
      }
    }

    //Reset
    newItem.value = {
        id: 0,
        orderId: 0,
        name: '',
        quantity: 0,
        price: 0
      }

      dialogItem.Visible.value = false;
      dialogItem.IsEdit.value = false;
  },
  Edit: async (data) => {
    newItem.value = Object.assign({}, data);
    dialogItem.Visible.value = true
    dialogItem.IsEdit.value = true
  },
  Delete: (data: IItem) => {
    newData.value.items = newData.value.items.filter((x) => x.id !== data.id)
  },
  Cancel: () => {
    //Reset
    newItem.value = {
      id: 0,
      orderId: 0,
      name: '',
      quantity: 0,
      price: 0
    }

    dialogItem.Visible.value = false;
    dialogItem.IsEdit.value = false;
  }
}

const totalItems = computed(() => newData.value.items.reduce((sum, item) => sum + item.quantity, 0));
const totalPrice = computed(() => newData.value.items.reduce((sum, item) => sum + item.quantity * item.price, 0));

const disableSaveOrder = computed(() =>
  newData.value.key.length < 1 ||
  newData.value.address.length < 1 ||
  newData.value.items.length < 1 ||
  newData.value.date == null ||
  newData.value.customer == null ||
  newData.value.customer.name.length < 1
);
</script>
<template>
  <Card>
    <template #title>Sales Order - Add</template>
    <template #content>
      <div class="pt-5 flex flex-col gap-5">
        <Card>
          <template #title>Information</template>
          <template #content>
            <div class="grid grid-cols-2 gap-5">
              <div class="flex flex-col gap-3">
                <div class="flex flex-col gap-2">
                  <span>Sales Order Number</span>
                  <div class="w-full">
                    <InputText v-model="newData.key" id="username" :fluid="true" aria-describedby="username-help" />
                  </div>
                </div>
                <div class="flex flex-col gap-2">
                  <span>Order Date</span>
                  <div class="w-full">
                    <DatePicker v-model="newData.date" :fluid="true" />
                  </div>
                </div>
              </div>
              <div class="flex flex-col gap-3">
                <div class="flex flex-col gap-2">
                  <span>Customer</span>
                  <div class="w-full">
                    <AutoComplete v-model="newData.customer" optionLabel="name" :fluid="true" :suggestions="customers" @complete="search" />
                  </div>
                </div>
                <div class="flex flex-col gap-2">
                  <span>Address</span>
                  <div class="w-full">
                    <Textarea v-model="newData.address" :fluid="true" rows="5" />
                  </div>
                </div>
              </div>
            </div>
          </template>
        </Card>
        <Card>
          <template #title>Order Items</template>
          <template #content>
            <DataTable :value="newData.items" tableStyle="min-width: 50rem">
              <template #header>
                <div class="flex flex-wrap items-center justify-between gap-2">
                  <Button label="Add Item" @click="dialogItem.Visible.value = true" />
                  <Button label="Hints" />
                </div>
              </template>
              <Column field="name" header="Item Name" style="width: calc(100%/5)"></Column>
              <Column field="quantity" header="Qty" style="width: calc(100%/5)"></Column>
              <Column header="Price" style="width: calc(100%/5)">
                <template #body="{ data }">
                  {{ Format.Currency(data.price) }}
                </template>
              </Column>
              <Column header="Total" style="width: calc(100%/5)">
                <template #body="{ data }">
                  {{ Format.Currency(data.price * data.quantity) }}
                </template>
              </Column>
              <Column header="Action" style="width: calc(100%/5)">
                <template #body="{ data }">
                  <div class="flex gap-2">
                    <Button label="Edit" @click="dialogItem.Edit(data)" />
                    <Button label="Delete" @click="dialogItem.Delete(data)" />
                  </div>
                </template>
              </Column>
              <template #footer>
                <div class="flex justify-end gap-10">
                  <div class="flex gap-5">
                    <span>Total Item :</span>
                    <span class="font-medium">{{totalItems}}</span>
                  </div>
                  <div class="flex gap-5">
                    <span>Total Amount :</span>
                    <span class="font-medium">{{ Format.Currency(totalPrice) }}</span>
                  </div>
                </div>
              </template>
            </DataTable>
          </template>
        </Card>
      </div>
    </template>
    <template #footer>
      <div class="flex gap-4 mt-1">
        <Button label="Cancel" severity="secondary" outlined class="w-full" @click="goBack"/>
        <Button label="Save" class="w-full" @click="saveOrder" :disabled="disableSaveOrder" />
      </div>
    </template>
  </Card>
  <Dialog v-model:visible="dialogItem.Visible.value" modal :header="`${dialogItem.IsEdit.value ? 'Edit' : 'Add'} Item`" :style="{ width: '25rem' }">
    <div class="flex flex-col gap-4">
      <div class="flex flex-col gap-2">
        <label for="username" class="font-semibold w-24">Item Name</label>
        <InputText id="itemName" v-model="newItem.name" class="flex-auto" autocomplete="off" />
      </div>
      <div class="flex flex-col gap-2">
        <label for="username" class="font-semibold w-24">Quantity</label>
        <InputNumber v-model="newItem.quantity" inputId="withoutgrouping" :useGrouping="false" fluid />
      </div>
      <div class="flex flex-col gap-2">
        <label for="username" class="font-semibold w-24">Price</label>
        <InputNumber v-model="newItem.price" inputId="integeronly" fluid />
      </div>
      <div class="flex justify-between border-t-1 pt-3">
        <span>Total : </span>
        <span class="font-medium">{{ Format.Currency(newItem.quantity * newItem.price) }}</span>
      </div>

      <div class="flex justify-end gap-2">
        <Button type="button" label="Cancel" severity="secondary" @click="dialogItem.Cancel()"></Button>
        <Button type="button" :label="dialogItem.IsEdit.value ? 'Save': 'Add'" @click="dialogItem.Save()" :disabled="newItem.name == null || newItem.quantity == 0"></Button>
      </div>
    </div>
  </Dialog>
</template>
