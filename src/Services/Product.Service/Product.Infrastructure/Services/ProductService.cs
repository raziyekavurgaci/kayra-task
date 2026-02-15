using Product.Application.Commands;
using Product.Application.DTOs;
using Product.Application.Interfaces;
using Product.Application.Queries;
using Product.Core.Interfaces;

namespace Product.Infrastructure.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;
    private readonly ICacheService _cache;

    public ProductService(IProductRepository repository, ICacheService cache)
    {
        _repository = repository;
        _cache = cache;
    }

    // QUERIES (Read) - with Redis Cache
    public async Task<IEnumerable<ProductDto>> HandleAsync(GetAllProductsQuery query)
    {
        // Try cache first
        var cachedProducts = await _cache.GetAsync<IEnumerable<ProductDto>>("products:all");
        if (cachedProducts != null)
            return cachedProducts;

        // Cache miss - get from DB
        var products = await _repository.GetAllAsync();
        var productDtos = products.Select(MapToDto);

        // Cache for 5 minutes
        await _cache.SetAsync("products:all", productDtos, TimeSpan.FromMinutes(5));

        return productDtos;
    }

    public async Task<ProductDto?> HandleAsync(GetProductByIdQuery query)
    {
        // Try cache first
        var cacheKey = $"products:{query.Id}";
        var cachedProduct = await _cache.GetAsync<ProductDto>(cacheKey);
        if (cachedProduct != null)
            return cachedProduct;

        // Cache miss - get from DB
        var product = await _repository.GetByIdAsync(query.Id);
        if (product == null)
            return null;

        var productDto = MapToDto(product);

        // Cache for 5 minutes
        await _cache.SetAsync(cacheKey, productDto, TimeSpan.FromMinutes(5));

        return productDto;
    }

    // COMMANDS (Write) - invalidate cache
    public async Task<ProductDto> HandleAsync(CreateProductCommand command)
    {
        var dto = command.Product;

        // Validation
        if (string.IsNullOrWhiteSpace(dto.Name))
            throw new ArgumentException("Product name is required");

        if (dto.Price <= 0)
            throw new ArgumentException("Price must be greater than 0");

        if (dto.Stock < 0)
            throw new ArgumentException("Stock cannot be negative");

        // Create product
        var product = new Core.Entities.Product
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            Stock = dto.Stock
        };

        var createdProduct = await _repository.CreateAsync(product);

        // Invalidate cache
        await _cache.RemoveAsync("products:all");

        return MapToDto(createdProduct);
    }

    public async Task<ProductDto> HandleAsync(UpdateProductCommand command)
    {
        // Validation
        if (string.IsNullOrWhiteSpace(command.Product.Name))
            throw new ArgumentException("Product name is required");

        if (command.Product.Price <= 0)
            throw new ArgumentException("Price must be greater than 0");

        if (command.Product.Stock < 0)
            throw new ArgumentException("Stock cannot be negative");

        // Get existing product
        var existingProduct = await _repository.GetByIdAsync(command.Id);
        if (existingProduct == null)
            throw new KeyNotFoundException($"Product with ID {command.Id} not found");

        // Update
        existingProduct.Name = command.Product.Name;
        existingProduct.Description = command.Product.Description;
        existingProduct.Price = command.Product.Price;
        existingProduct.Stock = command.Product.Stock;

        var updatedProduct = await _repository.UpdateAsync(existingProduct);

        // Invalidate cache
        await _cache.RemoveAsync("products:all");
        await _cache.RemoveAsync($"products:{command.Id}");

        return MapToDto(updatedProduct);
    }

    public async Task<bool> HandleAsync(DeleteProductCommand command)
    {
        var result = await _repository.DeleteAsync(command.Id);

        if (result)
        {
            // Invalidate cache
            await _cache.RemoveAsync("products:all");
            await _cache.RemoveAsync($"products:{command.Id}");
        }

        return result;
    }

    // DTO Mapping
    private static ProductDto MapToDto(Core.Entities.Product product)
    {
        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Stock = product.Stock,
            CreatedDate = product.CreatedDate,
            UpdatedDate = product.UpdatedDate
        };
    }
}
