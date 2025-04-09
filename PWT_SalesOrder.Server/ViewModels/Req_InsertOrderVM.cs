namespace PWT_SalesOrder.Server.ViewModels
{
    public class Req_InsertOrderVM : Req_OrdeBase
    {
        public List<Req_InsertItemVM> Items { get; set; }
    }
}
