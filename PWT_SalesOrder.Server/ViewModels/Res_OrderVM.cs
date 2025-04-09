namespace PWT_SalesOrder.Server.ViewModels
{
    public class Res_OrderVM
    {
        public long Id { get; set; }
        public string? Key { get; set; }
        public DateTime? Date { get; set; }
        public Res_CustomerVM? Customer { get; set; }
        public string? Address { get; set; }
    }
}
