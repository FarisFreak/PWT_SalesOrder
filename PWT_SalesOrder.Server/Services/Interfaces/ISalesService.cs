using PWT_SalesOrder.Server.ViewModels;

namespace PWT_SalesOrder.Server.Services.Interfaces
{
    public interface ISalesService
    {
        public Task<List<Res_OrderVM>> GetAllOrders();
        public Task<Res_OrderDetailVM> GetOrderDetail(int id);
        public Task<List<Res_OrderDetailVM>> SearchOrder(Req_SearchOrderVM data);
        public Task<Res_OrderDetailVM> InsertOrder(Req_InsertOrderVM data);
        public Task<Res_OrderDetailVM> EditOrderVM(Req_EditOrderVM data);
        public Task<Res_OrderDetailVM> DeleteOrderVM(long id);
    }
}
