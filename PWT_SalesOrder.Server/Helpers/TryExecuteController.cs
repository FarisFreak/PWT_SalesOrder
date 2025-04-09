using PWT_SalesOrder.Server.ViewModels;
using System.Runtime.CompilerServices;

namespace PWT_SalesOrder.Server.Helpers
{
    public static class TryExecuteController
    {
        public static async Task<BaseResponse<T>> Execute<T>(Func<Task<T>> action)
        {
            try
            {
                var result = await action();
                return BaseResponse<T>.Success(result);
            }
            catch (Exception ex)
            {
                return BaseResponse<T>.Fail(ex.Message);
            }
        }
    }
}
