using Microsoft.EntityFrameworkCore;
using Product.Core.Interfaces;
using Product.Infrastructure.Data;

namespace Product.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ProductDbContext _context;

    public ProductRepository(ProductDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Core.Entities.Product>> GetAllAsync()
    {
        return await _context.Products.ToListAsync();
    }

    public async Task<Core.Entities.Product?> GetByIdAsync(int id)
    {
        return await _context.Products.FindAsync(id);
    }

    public async Task<Core.Entities.Product> CreateAsync(Core.Entities.Product entity)
    {
        entity.CreatedDate = DateTime.UtcNow;
        _context.Products.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<Core.Entities.Product> UpdateAsync(Core.Entities.Product entity)
    {
        entity.UpdatedDate = DateTime.UtcNow;
        _context.Products.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var product = await GetByIdAsync(id);
        if (product == null) return false;

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Products.AnyAsync(p => p.Id == id);
    }
}
