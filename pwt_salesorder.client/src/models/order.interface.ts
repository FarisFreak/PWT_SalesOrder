import type { ICustomer } from "./customer.interface";
import type { IItem } from "./item.interface";

export interface IOrder {
  id?: number,
  key?: string,
  date?: Date,
  address?: string,
  customer?: ICustomer,
  items?: IItem[]
}
