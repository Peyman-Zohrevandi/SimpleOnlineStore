using Microsoft.EntityFrameworkCore;
using SimpleOnlineStore.Api.DataAccess.Data;
using SimpleOnlineStore.Api.DataAccess.IRepository;
using SimpleOnlineStore.Api.Domain.Entities;

namespace SimpleOnlineStore.Api.DataAccess.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly StoreContext _context;

        public UserRepository(StoreContext context)
        {
            _context = context;
        }

        public async Task<User> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return (await _context.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken))!;
        }

        public async Task AddAsync(User user, CancellationToken cancellationToken)
        {
            await _context.Users.AddAsync(user, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
