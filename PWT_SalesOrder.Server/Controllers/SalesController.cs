using Microsoft.AspNetCore.Mvc;
using PWT_SalesOrder.Server.Helpers;
using PWT_SalesOrder.Server.Services.Interfaces;
using PWT_SalesOrder.Server.ViewModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PWT_SalesOrder.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController(ISalesService salesService) : ControllerBase
    {
        private readonly ISalesService _salesService = salesService;

        [HttpGet]
        public async Task<BaseResponse<List<Res_OrderVM>>> GetAllOrders()
            => await TryExecuteController.Execute(async () => await _salesService.GetAllOrders());

        [HttpGet("{id}")]
        public async Task<BaseResponse<Res_OrderDetailVM>> GetOrderDetailById(int id)
            => await TryExecuteController.Execute(async () => await _salesService.GetOrderDetail(id));

        [HttpPost("Search")]
        public async Task<BaseResponse<List<Res_OrderDetailVM>>> SearchOrder([FromBody] Req_SearchOrderVM data)
            => await TryExecuteController.Execute(async () => await _salesService.SearchOrder(data));

        [HttpPost]
        public async Task<BaseResponse<Res_OrderDetailVM>> InsertOrder([FromBody] Req_InsertOrderVM data)
            => await TryExecuteController.Execute(async () => await _salesService.InsertOrder(data));

        [HttpPut]
        public async Task<BaseResponse<Res_OrderDetailVM>> EditOrder([FromBody] Req_EditOrderVM data)
            => await TryExecuteController.Execute(async () => await _salesService.EditOrderVM(data));

        [HttpDelete]
        public async Task<BaseResponse<Res_OrderDetailVM>> DeleteOrder([FromBody] Req_DeleteOrderVM data)
            => await TryExecuteController.Execute(async () => await _salesService.DeleteOrderVM(data.Id));
    }
}
