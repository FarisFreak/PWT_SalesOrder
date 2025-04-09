using Microsoft.EntityFrameworkCore;
using PWT_SalesOrder.Server.Models;
using PWT_SalesOrder.Server.Services.Interfaces;
using PWT_SalesOrder.Server.ViewModels;
using System.Reflection.Metadata.Ecma335;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PWT_SalesOrder.Server.Services
{
    public class SalesService(DbPwtContext context, ICustomerService customerService, IITemService itemService) : ISalesService
    {
        private readonly DbPwtContext _context = context;
        private readonly ICustomerService _customerService = customerService;
        private readonly IITemService _itemService = itemService;

        public async Task<List<Res_OrderVM>> GetAllOrders()
        {
            List<Res_OrderVM> res = await (
                from o in _context.SoOrders
                join c in _context.ComCustomers on o.ComCustomerId equals c.ComCustomerId into c_o
                from c_ox in c_o.DefaultIfEmpty()
                select new Res_OrderVM
                {
                    Id = o.SoOrderId,
                    Key = o.OrderNo,
                    Date = o.OrderDate,
                    Customer = new Res_CustomerVM
                    {
                        Id = c_ox.ComCustomerId,
                        Name = c_ox.CustomerName
                    },
                    Address = o.Address
                }
            ).ToListAsync();

            return res;
        }

        public async Task<Res_OrderDetailVM> GetOrderDetail(int id)
        {
            Res_OrderVM currentData = await _IsOrderExist(id);

            Res_OrderDetailVM? res = new Res_OrderDetailVM
            {
                Id = currentData.Id,
                Key = currentData.Key,
                Date = currentData.Date,
                Customer = currentData.Customer,
                Address = currentData.Address,
                Items = await _context.SoItems
                    .Where(x => x.SoOrderId == id)
                    .Select(x => new Res_ItemVM
                    {
                        Id = x.SoItemId,
                        OrderId = x.SoOrderId,
                        Name = x.ItemName,
                        Quantity = x.Quantity,
                        Price = x.Price
                    })
                    .ToListAsync()
            };

            return res;
        }

        public async Task<List<Res_OrderDetailVM>> SearchOrder(Req_SearchOrderVM data)
        {
            if (data == null)
                throw new Exception("Data cannot be empty.");

            var query = from o in _context.SoOrders
                        join c in _context.ComCustomers
                            on o.ComCustomerId equals c.ComCustomerId into oc
                        from c in oc.DefaultIfEmpty()
                        select new Res_OrderDetailVM
                        {
                            Id = o.SoOrderId,
                            Key = o.OrderNo,
                            Date = o.OrderDate,
                            Address = o.Address,
                            Customer = new Res_CustomerVM
                            {
                                Id = c != null ? c.ComCustomerId : 0,
                                Name = c != null ? c.CustomerName : "-"
                            }
                        };

            if (!string.IsNullOrWhiteSpace(data.Keyword))
            {
                string _keyword = data.Keyword.ToLower();
                query = query.Where(x =>
                    EF.Functions.Like(x.Key.ToLower(), $"%{_keyword}%") ||
                    //EF.Functions.Like(x.Address.ToLower(), $"%{_keyword}%") ||
                    EF.Functions.Like(x.Customer.Name.ToLower(), $"%{_keyword}%")
                );
            }

            if (data.Date != null)
                query = query
                    .Where(x => x.Date.Value.Date == data.Date.Value.Date);

            return await query.ToListAsync();
        }

        public async Task<Res_OrderDetailVM> InsertOrder(Req_InsertOrderVM data)
        {
            if (data == null)
                throw new Exception("Data cannot be empty.");

            if (data.Key == null || string.IsNullOrWhiteSpace(data.Key))
                throw new Exception("Order key cannot be empty.");

            if (data.Customer == null || string.IsNullOrWhiteSpace(data.Customer.Name))
                throw new Exception("Order customer name cannot be empty.");

            if (data.Address == null || string.IsNullOrWhiteSpace(data.Address))
                throw new Exception("Order address cannot be empty.");

            if (data.Items == null || data.Items.Count < 1)
                throw new Exception("Order items cannot be empty.");

            if (data.Date == null)
                throw new Exception("Order date cannot be empty.");

            SoOrder newData = new SoOrder
            {
                Address = data.Address,
                OrderDate = (DateTime)data.Date
            };

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    //Customer check
                    Res_CustomerVM cust = (data.Customer.Id != null && data.Customer.Id > 0) ?
                        await _customerService.GetCustomerById((int)data.Customer.Id) :
                        await _customerService.InsertCustomer(new Req_InsertCustomerVM { Name = data.Customer.Name });

                    newData.ComCustomerId = (int)cust.Id;

                    //Create order data
                    if (await _IsOrderExist(data.Key, false) == null)
                    {
                        newData.OrderNo = data.Key;

                        await _context.SoOrders.AddAsync(newData);
                        await _context.SaveChangesAsync();
                    }
                    else
                        throw new Exception("Order no already exist.");

                        //Insert items
                        List<Res_ItemVM> insertedItems = await _itemService.InsertItem(newData.SoOrderId, data.Items);

                    await transaction.CommitAsync();

                    return new Res_OrderDetailVM
                    {
                        Id = newData.SoOrderId,
                        Key = newData.OrderNo,
                        Date = newData.OrderDate,
                        Address = newData.Address,
                        Customer = cust,
                        Items = insertedItems,
                    };
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw new Exception("Failed to add new order.");
                }

            }
        }

        public async Task<Res_OrderDetailVM> EditOrderVM(Req_EditOrderVM data)
        {
            if (data == null)
                throw new Exception("Data cannot be empty.");

            if (data.Key == null || string.IsNullOrWhiteSpace(data.Key))
                throw new Exception("Order key cannot be empty.");

            if (data.Customer == null || string.IsNullOrWhiteSpace(data.Customer.Name))
                throw new Exception("Order customer name cannot be empty.");

            if (data.Address == null || string.IsNullOrWhiteSpace(data.Address))
                throw new Exception("Order address cannot be empty.");

            if (data.Date == null)
                throw new Exception("Order date cannot be empty.");

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    SoOrder currentData = await _context.SoOrders
                        .FindAsync(data.Id) ?? throw new Exception("Order id not found.");

                    if (data.Key != currentData.OrderNo)
                        currentData.OrderNo = data.Key;
                    if (data.Customer.Id != currentData.ComCustomerId)
                    {
                        Res_CustomerVM cust = (data.Customer.Id != null && data.Customer.Id > 0) ?
                            await _customerService.GetCustomerById((int)data.Customer.Id) :
                            await _customerService.InsertCustomer(new Req_InsertCustomerVM { Name = data.Customer.Name });

                        currentData.ComCustomerId = (int)cust.Id;
                    }
                    if (data.Address != currentData.Address)
                        currentData.Address = data.Address;
                    if (data.Date != currentData.OrderDate)
                        currentData.OrderDate = (DateTime)data.Date;

                    //Item edit
                    if (!data.Items.Any())
                        throw new Exception("Order items cannot be empty.");

                    var currentItems = await _itemService.GetItemByOrderId(data.Id);
                    var newItems = data.Items.Select(x => new Res_ItemVM
                    {
                        Id = x.Id,
                        OrderId = x.OrderId,
                        Name = x.Name,
                        Quantity = x.Quantity,
                        Price = x.Price,
                    });

                    //Delete data that are in current item not in new item
                    foreach (var item in currentItems.Where(ci => !newItems.Any(ni => ni.Id == ci.Id)))
                    {
                        await _itemService.DeleteItem(item.Id);
                    }

                    //Add new item that are not in current item
                    foreach (var item in newItems.Where(ni => !currentItems.Any(ci => ci.Id == ni.Id)))
                    {
                        await _itemService.InsertItem(data.Id, new Req_InsertItemVM
                        {
                            OrderId = item.Id,
                            Name = item.Name,
                            Quantity = item.Quantity,
                            Price = item.Price,
                        });
                    }

                    //Modify if any modified items
                    foreach (var item in newItems.Where(ni => currentItems.Any(ci => ci.Id == ni.Id)))
                    {
                        Req_EditItemVM _editedItem = new Req_EditItemVM
                        {
                            Id = item.Id,
                            OrderId = item.OrderId,
                            Name = item.Name,
                            Quantity = item.Quantity ?? throw new Exception($"Item (${item.Id}) quantity cannot be empty."),
                            Price = item.Price ?? 0,
                        };

                        await _itemService.EditItem(_editedItem);
                    }

                    _context.SoOrders.Update(currentData);

                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();

                    return await GetOrderDetail((int)data.Id);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception("Failed to edit current order.", ex);
                }
            }
        }

        public async Task<Res_OrderDetailVM> DeleteOrderVM(long id)
        {
            if (id == null || id < 1)
                throw new Exception("Order id cannot be empty.");

            SoOrder currentData = await _context.SoOrders
                .FindAsync(id) ?? throw new Exception("Order id not found.");

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    //Remove items first
                    List<Res_ItemVM> items = await _itemService.DeleteItem(currentData.SoOrderId);

                    _context.SoOrders.Remove(currentData);

                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();

                    return new Res_OrderDetailVM
                    {
                        Id = id,
                        Key = currentData.OrderNo,
                        Address = currentData.Address,
                        Date = currentData.OrderDate,
                        Items = items,
                        Customer = await _customerService.GetCustomerById(currentData.ComCustomerId)
                    };
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw new Exception("Failed to delete current order.");
                }
            }
        }

        private async Task<Res_OrderVM> _IsOrderExist(int id)
        {
            Res_OrderVM res = await (
                from o in _context.SoOrders
                join c in _context.ComCustomers on o.ComCustomerId equals c.ComCustomerId into c_o
                from c_ox in c_o.DefaultIfEmpty()
                where o.SoOrderId == id
                select new Res_OrderVM
                {
                    Id = o.SoOrderId,
                    Key = o.OrderNo,
                    Date = o.OrderDate,
                    Customer = new Res_CustomerVM
                    {
                        Id = c_ox.ComCustomerId,
                        Name = c_ox.CustomerName
                    },
                    Address = o.Address
                }
            )
            .FirstOrDefaultAsync() ?? throw new Exception("Order id not found.");

            return res;
        }

        private async Task<Res_OrderVM?> _IsOrderExist(string key, bool isThrow = true)
        {
            Res_OrderVM? res = await (
                from o in _context.SoOrders
                join c in _context.ComCustomers on o.ComCustomerId equals c.ComCustomerId into c_o
                from c_ox in c_o.DefaultIfEmpty()
                where o.OrderNo.ToLower() == key.ToLower()
                select new Res_OrderVM
                {
                    Id = o.SoOrderId,
                    Key = o.OrderNo,
                    Date = o.OrderDate,
                    Customer = new Res_CustomerVM
                    {
                        Id = c_ox.ComCustomerId,
                        Name = c_ox.CustomerName
                    },
                    Address = o.Address
                }
            )
            .FirstOrDefaultAsync();

            if (isThrow && res == null)
                throw new Exception("Order id not found.");

            return res;
        }
    }
}
