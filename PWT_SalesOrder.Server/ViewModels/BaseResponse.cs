namespace PWT_SalesOrder.Server.ViewModels
{
    public class BaseResponse<T>
    {
        public bool Status { get; set; } = false;
        public string? Message { get; set; }
        public T? Data { get; set; }

        public static BaseResponse<T> Success(T data, string message = "OK")
        {
            return new BaseResponse<T>
            {
                Status = true,
                Message = message,
                Data = data
            };
        }

        public static BaseResponse<T> Fail(string message = "Something went wrong")
        {
            return new BaseResponse<T>
            {
                Status = false,
                Message = message,
                Data = default
            };
        }
    }

}
