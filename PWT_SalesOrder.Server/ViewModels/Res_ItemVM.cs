namespace PWT_SalesOrder.Server.ViewModels
{
    public class Res_ItemVM
    {
        public long Id { get; set; }
        public long OrderId { get; set; }
        public string? Name { get; set; }
        public int? Quantity { get; set; }
        public double? Price { get; set; }
    }
}
