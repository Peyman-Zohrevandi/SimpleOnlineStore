namespace SimpleOnlineStore.Api.Domain.Dtos.Product.Responses
{
    public record AddProductResponseDto(int Id, string Title, int InventoryCount, decimal Price, int Discount);
}
