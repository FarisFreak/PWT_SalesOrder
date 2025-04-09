using PWT_SalesOrder.Server.ViewModels;

namespace PWT_SalesOrder.Server.Services.Interfaces
{
    public interface IITemService
    {
        public Task<Res_ItemVM> GetItemById(int itemId);
        public Task<List<Res_ItemVM>> GetItemByOrderId(long orderId);
        public Task<Res_ItemVM> InsertItem(long orderId, Req_InsertItemVM data);
        public Task<List<Res_ItemVM>> InsertItem(long orderId, List<Req_InsertItemVM> data);
        public Task<Res_ItemVM> EditItem(Req_EditItemVM data);
        public Task<Res_ItemVM> DeleteItem(int itemId);
        public Task<List<Res_ItemVM>> DeleteItem(long orderId);
    }
}
