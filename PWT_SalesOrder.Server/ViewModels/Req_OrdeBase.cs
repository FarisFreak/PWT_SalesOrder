namespace PWT_SalesOrder.Server.ViewModels
{
    public class Req_OrdeBase
    {
        public string? Key { get; set; }
        public DateTime? Date { get; set; }
        public Res_CustomerVM? Customer { get; set; }
        public string? Address { get; set; }
    }
}
