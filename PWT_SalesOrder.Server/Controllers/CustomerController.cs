using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PWT_SalesOrder.Server.Helpers;
using PWT_SalesOrder.Server.Services.Interfaces;
using PWT_SalesOrder.Server.ViewModels;

namespace PWT_SalesOrder.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController (ICustomerService customerService) : ControllerBase
    {
        private readonly ICustomerService _customerService = customerService;

        [HttpGet]
        public async Task<BaseResponse<List<Res_CustomerVM>>> GetAllCustomers()
            => await TryExecuteController.Execute(async () => await _customerService.GetAllCustomers());

        [HttpGet("{Id}")]
        public async Task<BaseResponse<Res_CustomerVM>> GetCustomerById(int Id)
            => await TryExecuteController.Execute(async () => await _customerService.GetCustomerById(Id));

        [HttpGet("Search/{name}")]
        public async Task<BaseResponse<List<Res_CustomerVM>>> SearchCustomer(string name)
            => await TryExecuteController.Execute(async () => await _customerService.SearchCustomer(name));

        [HttpPost]
        public async Task<BaseResponse<Res_CustomerVM>> InsertCustomer([FromBody] Req_InsertCustomerVM data)
            => await TryExecuteController.Execute(async () => await _customerService.InsertCustomer(data));

        [HttpPatch]
        public async Task<BaseResponse<Res_CustomerVM>> EditCustomer([FromBody] Req_EditCustomerVM data)
            => await TryExecuteController.Execute(async () => await _customerService.EditCustomer(data));

        [HttpDelete]
        public async Task<BaseResponse<Res_CustomerVM>> DeleteCustomer([FromBody] Req_DeleteCustomerVM data)
            => await TryExecuteController.Execute(async () => await _customerService.DeleteCustomer(data.Id));
    }
}
