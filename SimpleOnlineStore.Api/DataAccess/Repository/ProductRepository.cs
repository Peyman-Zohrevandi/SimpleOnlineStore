using Microsoft.EntityFrameworkCore;
using SimpleOnlineStore.Api.DataAccess.Data;
using SimpleOnlineStore.Api.DataAccess.IRepository;
using SimpleOnlineStore.Api.Domain.Dtos.Product.Requests;
using SimpleOnlineStore.Api.Domain.Entities;

namespace SimpleOnlineStore.Api.DataAccess.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly StoreContext _context;

        public ProductRepository(StoreContext context)
        {
            _context = context;
        }

        public async Task<Product> AddAsync(AddProductRequestDto product, CancellationToken cancellationToken)
        {
            var addProduct = await _context.Products.AddAsync(new Product
            {
                Discount = product.Discount,
                InventoryCount = product.InventoryCount,
                Price = product.Price,
                Title = product.Title
            }, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return addProduct.Entity;
        }

        public async Task<Product> UpdateAsync(Product product, CancellationToken cancellationToken)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync(cancellationToken);
            return product;
        }

        public async Task<Product> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return (await _context.Products.FindAsync(id, cancellationToken))!;
        }

        public async Task<bool> IsTitleUniqueAsync(string title, CancellationToken cancellationToken)
        {
            return await _context.Products.AnyAsync(p => p.Title == title, cancellationToken);
        }
    }
}
