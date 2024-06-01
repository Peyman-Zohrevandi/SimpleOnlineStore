using SimpleOnlineStore.Api.Domain.Entities;

namespace SimpleOnlineStore.Api.DataAccess.IRepository
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task AddAsync(User user, CancellationToken cancellationToken);
    }
}
