using PWT_SalesOrder.Server.ViewModels;

namespace PWT_SalesOrder.Server.Services.Interfaces
{
    public interface ICustomerService
    {
        public Task<List<Res_CustomerVM>> GetAllCustomers();
        public Task<Res_CustomerVM> GetCustomerById(int id);
        public Task<List<Res_CustomerVM>> SearchCustomer(string name);
        public Task<Res_CustomerVM> InsertCustomer(Req_InsertCustomerVM data);
        public Task<Res_CustomerVM> EditCustomer(Req_EditCustomerVM data);
        public Task<Res_CustomerVM> DeleteCustomer(int id);
    }
}
