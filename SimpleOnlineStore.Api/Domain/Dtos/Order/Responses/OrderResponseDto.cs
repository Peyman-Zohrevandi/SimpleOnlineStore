using SimpleOnlineStore.Api.Domain.Dtos.Product.Responses;
using SimpleOnlineStore.Api.Domain.Dtos.User.Responses;

namespace SimpleOnlineStore.Api.Domain.Dtos.Order.Responses
{
    public class OrderResponseDto
    {
        public int Id { get; set; }
        public GetProductResponseDto Product { get; set; }
        public DateTime CreationDate { get; set; }
        public UserResponseDto Buyer { get; set; }
    }
}
