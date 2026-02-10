using Application.Commands.Products;
using Application.DTOs.Products;
using Application.Queries.Products;

namespace Application.Interfaces
{
    public interface IProductService
    {
        // Queries (Read)
        Task<IEnumerable<ProductDto>> HandleAsync(GetAllProductsQuery query);
        Task<ProductDto?> HandleAsync(GetProductByIdQuery query);
        
        // Commands (Write)
        Task<ProductDto> HandleAsync(CreateProductCommand command);
        Task<ProductDto?> HandleAsync(UpdateProductCommand command);
        Task<bool> HandleAsync(DeleteProductCommand command);
    }
}
