using Core.Entities;

namespace Core.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        // Ürüne özel metodlar buraya eklenebilir
        // Örnek: Task<IEnumerable<Product>> GetByPriceRangeAsync(decimal min, decimal max);
    }
}
