namespace eShop.CoreBusiness.Models
{
    public class OrderLineItem
    {
        public int? LineItemID { get; set; }
        public int? OrderID { get; set; }
        public int ProductId { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public Product? Product { get; set; }

    }
}
