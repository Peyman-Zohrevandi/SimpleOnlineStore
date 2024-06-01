namespace SimpleOnlineStore.Api.Domain.Dtos.Product.Responses
{
    public class GetProductResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int InventoryCount { get; set; }
        public int Discount { get; set; }
        public decimal Price { get; set; }
        public decimal PriceAfterDiscount { get; set; }
    }
}
