using SimpleOnlineStore.Api.Domain.Dtos.Product.Requests;
using SimpleOnlineStore.Api.Domain.Entities;

namespace SimpleOnlineStore.Api.DataAccess.IRepository
{
    public interface IProductRepository
    {
        Task<Product> AddAsync(AddProductRequestDto product, CancellationToken cancellationToken);
        Task<Product> UpdateAsync(Product product, CancellationToken cancellationToken);
        Task<Product> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<bool> IsTitleUniqueAsync(string title, CancellationToken cancellationToken);
    }
}
