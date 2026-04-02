using ProductEntity = Product.Domain.Entities.Product;

namespace Product.Application.Abstractions.Persistence;

public interface IProductRepository
{
    Task AddAsync(ProductEntity product, CancellationToken cancellationToken = default);
    Task UpdateAsync(ProductEntity product, CancellationToken cancellationToken = default);
    Task<ProductEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ProductEntity>> GetAllAsync(CancellationToken cancellationToken = default);
}