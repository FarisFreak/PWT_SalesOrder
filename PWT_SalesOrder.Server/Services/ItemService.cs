using Microsoft.EntityFrameworkCore;
using PWT_SalesOrder.Server.Models;
using PWT_SalesOrder.Server.ViewModels;

namespace PWT_SalesOrder.Server.Services
{
    public class ItemService(DbPwtContext context) : Interfaces.IITemService
    {
        private readonly DbPwtContext _context = context;

        public async Task<Res_ItemVM> GetItemById(int itemId)
        {
            if (itemId == null || itemId < 1)
                throw new Exception("Item id cannot be empty");

            SoItem currentData = await _context.SoItems
                .FindAsync(itemId) ?? throw new Exception("Item id not found");

            return new Res_ItemVM
            {
                Id = currentData.SoItemId,
                OrderId = currentData.SoOrderId,
                Name = currentData.ItemName,
                Quantity = currentData.Quantity,
                Price = currentData.Price,
            };
        }

        public async Task<List<Res_ItemVM>> GetItemByOrderId(long orderId)
        {
            if (orderId == null || orderId < 1)
                throw new Exception("Item order id cannot be empty.");

            List<Res_ItemVM> currentData = await _context.SoItems
                .Where(x => x.SoOrderId == orderId)
                .Select(x => new Res_ItemVM
                {
                    Id = x.SoItemId,
                    OrderId = x.SoOrderId,
                    Name = x.ItemName,
                    Price = x.Price,
                    Quantity = x.Quantity
                })
                .ToListAsync();

            return currentData;
        }

        public async Task<Res_ItemVM> InsertItem(long orderId, Req_InsertItemVM data)
        {
            if (orderId == null || orderId < 1)
                throw new Exception("Order id cannot be empty.");

            if (string.IsNullOrWhiteSpace(data.Name))
                throw new Exception("Item name cannot be empty.");

            if (data.Quantity == null || data.Quantity < 1)
                throw new Exception("Item quantity cannot be empty.");

            if (data.Price == null || data.Price < 1)
                data.Price = 0;

            SoOrder currentOrder = await _IsOrderExist(orderId);

            try
            {
                SoItem newData = new SoItem
                {
                    SoOrderId = currentOrder.SoOrderId,
                    ItemName = data.Name,
                    Quantity = (int)data.Quantity,
                    Price = (int)data.Price
                };

                await _context.SoItems.AddAsync(newData);

                await _context.SaveChangesAsync();

                return new Res_ItemVM
                {
                    Id = newData.SoItemId,
                    OrderId = newData.SoOrderId,
                    Name = newData.ItemName,
                    Quantity = (int)newData.Quantity,
                    Price = (int)newData.Price
                };
            }
            catch (Exception)
            {
                throw new Exception("Failed to add new item.");
            }
        }

        public async Task<List<Res_ItemVM>> InsertItem(long orderId, List<Req_InsertItemVM> data)
        {
            if (orderId == null || orderId < 1)
                throw new Exception("Order id cannot be empty.");

            if (data == null || data.Count == 0)
                throw new Exception("No items to insert.");

            SoOrder currentOrder = await _IsOrderExist(orderId);

            var newData = data.Select(item =>
            {
                if (string.IsNullOrWhiteSpace(item.Name))
                    throw new Exception("Item name cannot be empty.");

                if (item.Quantity == null || item.Quantity < 1)
                    throw new Exception("Item quantity must be greater than 0.");

                if (item.Price == null || item.Price < 1)
                    item.Price = 0;

                return new SoItem
                {
                    SoOrderId = currentOrder.SoOrderId,
                    ItemName = item.Name,
                    Quantity = item.Quantity.Value,
                    Price = item.Price.GetValueOrDefault(0)
                };
            }).ToList();

            try
            {
                await _context.AddRangeAsync(newData);

                await _context.SaveChangesAsync();

                List<Res_ItemVM> res = newData
                    .Select(x => new Res_ItemVM
                    {
                        Id = x.SoItemId,
                        OrderId = x.SoOrderId,
                        Name = x.ItemName,
                        Quantity = (int)x.Quantity,
                        Price = (int)x.Price
                    })
                    .ToList();

                return res;

            }
            catch (Exception)
            {
                throw new Exception("Failed to add new items.");
            }
        }

        public async Task<Res_ItemVM> EditItem(Req_EditItemVM data)
        {
            if (data == null)
                throw new Exception("Data cannot be empty.");

            if (data.Id == null || data.Id < 1)
                throw new Exception("Item id cannot be empty.");

            if (string.IsNullOrWhiteSpace(data.Name))
                throw new Exception("Item name cannot be empty.");

            if (data.Quantity == null || data.Quantity < 1)
                throw new Exception("Item quantity cannot be empty.");

            if (data.Price == null || data.Price < 1)
                data.Price = 0;

            try
            {
                SoItem currentItem = await _context.SoItems.FindAsync(data.Id) ?? throw new Exception("Item id not found.");

                if (data.Name != currentItem.ItemName)
                    currentItem.ItemName = data.Name;
                if (data.Quantity != currentItem.Quantity)
                    currentItem.Quantity = (int)data.Quantity;
                if (data.Price != currentItem.Price)
                    currentItem.Price = (double)data.Price;

                _context.SoItems.Update(currentItem);

                await _context.SaveChangesAsync();

                return new Res_ItemVM
                {
                    Id = data.Id,
                    OrderId = data.OrderId,
                    Name = currentItem.ItemName,
                    Quantity = currentItem.Quantity,
                    Price = currentItem.Price
                };
            }
            catch (Exception)
            {
                throw new Exception("Failed to edit current item.");
            }
        }

        public async Task<Res_ItemVM> DeleteItem(int itemId)
        {
            if (itemId == null || itemId < 0)
                throw new Exception("Item id cannot be empty.");

            SoItem currentData = await _context.SoItems
                .FindAsync(itemId) ?? throw new Exception("Item id not found.");

            try
            {
                _context.SoItems.Remove(currentData);

                await _context.SaveChangesAsync();

                return new Res_ItemVM
                {
                    Id = currentData.SoItemId,
                    OrderId = currentData.SoOrderId,
                    Name = currentData.ItemName,
                    Quantity = currentData.Quantity,
                    Price = currentData.Price
                };
            }
            catch (Exception)
            {
                throw new Exception("Failed to delete current item.");
            }
        }

        public async Task<List<Res_ItemVM>> DeleteItem(long orderId)
        {
            if (orderId == null || orderId < 0) 
                throw new Exception("Item order id cannot be empty.");

            List<SoItem> currentData = await _context.SoItems
                .Where(x => x.SoOrderId == orderId)
                .ToListAsync();

            if (currentData.Count < 1)
                throw new Exception("Item order id not found.");

            try
            {
                _context.SoItems.RemoveRange(currentData);

                await _context.SaveChangesAsync();

                return currentData
                    .Select(x => new Res_ItemVM
                    {
                        Id = x.SoItemId,
                        OrderId = x.SoOrderId,
                        Name = x.ItemName,
                        Quantity = x.Quantity,
                        Price = x.Price
                    })
                    .ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task<SoOrder> _IsOrderExist(long orderId)
        {
            SoOrder res = await _context.SoOrders
                .FindAsync(orderId) ?? throw new Exception("Order id not found.");

            return res;
        }
    }
}