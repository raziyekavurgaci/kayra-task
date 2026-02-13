using Application.Commands.Products;
using Application.DTOs.Products;
using Application.Interfaces;
using Application.Queries.Products;
using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        private readonly ICacheService _cache;

        public ProductService(IProductRepository repository, ICacheService cache)
        {
            _repository = repository;
            _cache = cache;
        }

        // QUERIES (Read)
        public async Task<IEnumerable<ProductDto>> HandleAsync(GetAllProductsQuery query)
        {
            // Cache'den dene
            var cachedProducts = await _cache.GetAsync<IEnumerable<ProductDto>>("products:all");
            if (cachedProducts != null)
                return cachedProducts;

            // Cache'de yok, DB'den al
            var products = await _repository.GetAllAsync();
            var productDtos = products.Select(MapToDto);

            // Cache'e kaydet (5 dakika)
            await _cache.SetAsync("products:all", productDtos, TimeSpan.FromMinutes(5));

            return productDtos;
        }

        public async Task<ProductDto?> HandleAsync(GetProductByIdQuery query)
        {
            // Cache'den dene
            var cacheKey = $"products:{query.Id}";
            var cachedProduct = await _cache.GetAsync<ProductDto>(cacheKey);
            if (cachedProduct != null)
                return cachedProduct;

            // Cache'de yok, DB'den al
            var product = await _repository.GetByIdAsync(query.Id);
            if (product == null)
                return null;

            var productDto = MapToDto(product);

            // Cache'e kaydet (5 dakika)
            await _cache.SetAsync(cacheKey, productDto, TimeSpan.FromMinutes(5));

            return productDto;
        }

        // COMMANDS (Write)
        public async Task<ProductDto> HandleAsync(CreateProductCommand command)
        {
            var dto = command.ProductDto;

            // Validasyon
            if (string.IsNullOrWhiteSpace(dto.Name) || dto.Name.Length > 200)
                throw new ArgumentException("Ürün adı 1-200 karakter arasında olmalıdır");

            if (dto.Price <= 0)
                throw new ArgumentException("Fiyat 0'dan büyük olmalıdır");

            if (dto.Stock < 0)
                throw new ArgumentException("Stok negatif olamaz");

            // Entity oluştur
            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Stock = dto.Stock
            };

            // Kaydet
            var createdProduct = await _repository.CreateAsync(product);

            // Cache'i temizle
            await _cache.RemoveAsync("products:all");

            return MapToDto(createdProduct);
        }

        public async Task<ProductDto?> HandleAsync(UpdateProductCommand command)
        {
            var dto = command.ProductDto;

            // Validasyon
            if (string.IsNullOrWhiteSpace(dto.Name) || dto.Name.Length > 200)
                throw new ArgumentException("Ürün adı 1-200 karakter arasında olmalıdır");

            if (dto.Price <= 0)
                throw new ArgumentException("Fiyat 0'dan büyük olmalıdır");

            if (dto.Stock < 0)
                throw new ArgumentException("Stok negatif olamaz");

            // Ürün var mı kontrol et
            var existingProduct = await _repository.GetByIdAsync(command.Id);
            if (existingProduct == null)
                return null;

            // Güncelle
            existingProduct.Name = dto.Name;
            existingProduct.Description = dto.Description;
            existingProduct.Price = dto.Price;
            existingProduct.Stock = dto.Stock;

            var updatedProduct = await _repository.UpdateAsync(existingProduct);

            // Cache'i temizle
            await _cache.RemoveAsync("products:all");
            await _cache.RemoveAsync($"products:{command.Id}");

            return MapToDto(updatedProduct);
        }

        public async Task<bool> HandleAsync(DeleteProductCommand command)
        {
            var result = await _repository.DeleteAsync(command.Id);

            if (result)
            {
                // Cache'i temizle
                await _cache.RemoveAsync("products:all");
                await _cache.RemoveAsync($"products:{command.Id}");
            }

            return result;
        }

        // DTO Mapping
        private ProductDto MapToDto(Product product)
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
}
