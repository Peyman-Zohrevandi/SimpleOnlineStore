namespace SimpleOnlineStore.Api.Domain.Dtos.Product.Requests
{
    public record AddProductRequestDto(string Title, int InventoryCount, decimal Price, int Discount);
}
