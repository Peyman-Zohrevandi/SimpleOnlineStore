using FluentValidation;
using SimpleOnlineStore.Api.DataAccess.IRepository;
using SimpleOnlineStore.Api.Domain.Entities;
using SimpleOnlineStore.Api.Helper.ExceptionHandlers;
using SimpleOnlineStore.Api.IServices;

namespace SimpleOnlineStore.Api.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IValidator<User> _userValidator;

        public UserService(IUserRepository userRepository, IValidator<User> userValidator)
        {
            _userRepository = userRepository;
            _userValidator = userValidator;
        }

        public async Task ValidateUserAsync(int userId, CancellationToken cancellationToken)
        {
            var user = await GetUserByIdAsync(userId, cancellationToken);
            await ValidateUserAsync(user, cancellationToken);
        }

        private async Task<User> GetUserByIdAsync(int userId, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
            if (user == null)
            {
                throw new UserNotFundException(userId);
            }
            return user;
        }

        private async Task ValidateUserAsync(User user, CancellationToken cancellationToken)
        {
            var validationResult = await _userValidator.ValidateAsync(user, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
        }
    }
}
