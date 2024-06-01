using FluentValidation;
using SimpleOnlineStore.Api.Domain.Dtos.Product.Requests;

namespace SimpleOnlineStore.Api.Validators
{
    public class ProductValidator : AbstractValidator<AddProductRequestDto>
    {
        public ProductValidator()
        {
            RuleFor(p => p.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(40).WithMessage("Title must be less than 40 characters.");

            RuleFor(p => p.InventoryCount)
                .GreaterThanOrEqualTo(0).WithMessage("Inventory count must be zero or more.");

            RuleFor(p => p.Price)
                .GreaterThan(0).WithMessage("Price must be greater than zero.");

            RuleFor(p => p.Discount)
                .InclusiveBetween(0, 100).WithMessage("Discount must be between 0 and 100.");
        }
    }
}
