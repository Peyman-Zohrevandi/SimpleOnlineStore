using SimpleOnlineStore.Api.DataAccess.Data;
using SimpleOnlineStore.Api.DataAccess.IRepository;
using SimpleOnlineStore.Api.Domain.Entities;

namespace SimpleOnlineStore.Api.DataAccess.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly StoreContext _context;

        public OrderRepository(StoreContext context)
        {
            _context = context;
        }

        public async Task<int> AddAsync(Order order, CancellationToken cancellationToken)
        {
            var result = await _context.Orders.AddAsync(order, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return result.Entity.Id;
        }
    }
}
