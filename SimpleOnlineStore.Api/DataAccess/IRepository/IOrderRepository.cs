using SimpleOnlineStore.Api.Domain.Entities;

namespace SimpleOnlineStore.Api.DataAccess.IRepository
{
    public interface IOrderRepository
    {
        Task<int> AddAsync(Order order, CancellationToken cancellationToken);
    }
}
