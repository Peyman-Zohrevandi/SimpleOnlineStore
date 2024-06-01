using SimpleOnlineStore.Api.Domain.Dtos.Order.Requests;
using SimpleOnlineStore.Api.Domain.Dtos.Order.Responses;

namespace SimpleOnlineStore.Api.IServices
{
    public interface IOrderService
    {
        Task<OrderResponseDto> CreateOrderAsync(BuyProductRequestDto buyProductRequestDto,
            CancellationToken cancellationToken);
    }
}
