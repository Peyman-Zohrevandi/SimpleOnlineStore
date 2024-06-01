using FluentValidation;
using SimpleOnlineStore.Api.Domain.Entities;

namespace SimpleOnlineStore.Api.Validators
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(u => u.Name)
                .NotEmpty().WithMessage("Name is required.");
        }
    }
}
