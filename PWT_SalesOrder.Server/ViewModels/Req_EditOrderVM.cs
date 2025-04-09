namespace PWT_SalesOrder.Server.ViewModels
{
    public class Req_EditOrderVM : Req_OrdeBase
    {
        public long Id { get; set; }
        public List<Req_EditItemVM> Items { get; set; }
    }
}
