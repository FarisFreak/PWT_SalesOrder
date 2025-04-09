using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PWT_SalesOrder.Server.Helpers;
using PWT_SalesOrder.Server.Services.Interfaces;
using PWT_SalesOrder.Server.ViewModels;

namespace PWT_SalesOrder.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController(IITemService itemService) : ControllerBase
    {
        private readonly IITemService _itemService = itemService;

        [HttpGet("{id}")]
        public async Task<BaseResponse<Res_ItemVM>> GetItemById(int id) 
            => await TryExecuteController.Execute(async () => await _itemService.GetItemById(id));

        [HttpPost("{orderId}")]
        public async Task<BaseResponse<Res_ItemVM>> InsertItem(int orderId, [FromBody] Req_InsertItemVM data)
            => await TryExecuteController.Execute(async () => await _itemService.InsertItem(orderId, data));

        [HttpPost("Bulk/{orderId}")]
        public async Task<BaseResponse<List<Res_ItemVM>>> InsertItem(int orderId, [FromBody] List<Req_InsertItemVM> data)
            => await TryExecuteController.Execute(async () => await _itemService.InsertItem(orderId, data));

        [HttpPut]
        public async Task<BaseResponse<Res_ItemVM>> EditItem([FromBody] Req_EditItemVM data)
            => await TryExecuteController.Execute(async () => await _itemService.EditItem(data));

        [HttpDelete]
        public async Task<BaseResponse<Res_ItemVM>> DeleteItem([FromBody] Req_DeleteItemVM data)
            => await TryExecuteController.Execute(async () => await _itemService.DeleteItem(data.Id));
    }
}
