using FluentValidation;
using SimpleOnlineStore.Api.Domain.Dtos.Order.Requests;

namespace SimpleOnlineStore.Api.Validators
{
    public class OrderValidator : AbstractValidator<BuyProductRequestDto>
    {
        public OrderValidator()
        {
            RuleFor(o => o.ProductId)
                .NotNull().WithMessage("Product is required.");

            RuleFor(o => o.UserId)
                .NotNull().WithMessage("Buyer is required.");
        }
    }
}
