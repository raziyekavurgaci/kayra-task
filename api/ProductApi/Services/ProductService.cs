using ProductApi.DTOs;
using ProductApi.Models;
using ProductApi.Repositories;

namespace ProductApi.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            var products = await _repository.GetAllAsync();
            return products.Select(p => MapToDto(p));
        }

        public async Task<ProductDto?> GetProductByIdAsync(int id)
        {
            var product = await _repository.GetByIdAsync(id);
            return product == null ? null : MapToDto(product);
        }

        public async Task<ProductDto> CreateProductAsync(CreateProductDto createDto)
        {
            ValidateCreateProduct(createDto);

            var product = new Product
            {
                Name = createDto.Name,
                Description = createDto.Description,
                Price = createDto.Price,
                Stock = createDto.Stock
            };

            var createdProduct = await _repository.CreateAsync(product);
            return MapToDto(createdProduct);
        }

        public async Task<ProductDto?> UpdateProductAsync(int id, UpdateProductDto updateDto)
        {
            ValidateUpdateProduct(updateDto);

            var existingProduct = await _repository.GetByIdAsync(id);
            if (existingProduct == null)
            {
                return null;
            }

            existingProduct.Name = updateDto.Name;
            existingProduct.Description = updateDto.Description;
            existingProduct.Price = updateDto.Price;
            existingProduct.Stock = updateDto.Stock;

            var updatedProduct = await _repository.UpdateAsync(existingProduct);
            return MapToDto(updatedProduct);
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

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

        private void ValidateCreateProduct(CreateProductDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                throw new ArgumentException("Ürün adı boş olamaz");
            }

            if (dto.Price <= 0)
            {
                throw new ArgumentException("Fiyat 0'dan büyük olmalıdır");
            }

            if (dto.Stock < 0)
            {
                throw new ArgumentException("Stok negatif olamaz");
            }
        }

        private void ValidateUpdateProduct(UpdateProductDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                throw new ArgumentException("Ürün adı boş olamaz");
            }

            if (dto.Price <= 0)
            {
                throw new ArgumentException("Fiyat 0'dan büyük olmalıdır");
            }

            if (dto.Stock < 0)
            {
                throw new ArgumentException("Stok negatif olamaz");
            }
        }
    }
}
