using SimpleOnlineStore.Api.Domain.Dtos.Inventory.Requests;
using SimpleOnlineStore.Api.Domain.Dtos.Order.Requests;
using SimpleOnlineStore.Api.Domain.Dtos.Order.Responses;
using SimpleOnlineStore.Api.Domain.Dtos.Product.Requests;
using SimpleOnlineStore.Api.Domain.Dtos.Product.Responses;

namespace SimpleOnlineStore.Api.IServices
{
    public interface IProductService
    {
        Task<AddProductResponseDto> AddProductAsync(AddProductRequestDto addProductDto,
            CancellationToken cancellationToken);
        
        Task<GetProductResponseDto> GetProductByIdAsync(GetProductByIdRequestDto getProductByIdDto, CancellationToken cancellationToken);
        Task<OrderResponseDto> BuyProductAsync(BuyProductRequestDto buyProductDto, CancellationToken cancellationToken);
        Task ValidateProductAsync(int productId, CancellationToken cancellationToken);

        Task ReduceInventoryAsync(int productId, CancellationToken cancellationToken);

        Task<GetProductResponseDto> UpdateInventoryCountAsync(UpdateInventoryCountRequestDto updateInventoryCountDto,
            CancellationToken cancellationToken);

    }
}
