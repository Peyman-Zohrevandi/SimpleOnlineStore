namespace SimpleOnlineStore.Api.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int InventoryCount { get; set; }
        public decimal Price { get; set; }
        public int Discount { get; set; } // percentage
    }
}
