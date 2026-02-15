using Product.Application.Commands;
using Product.Application.DTOs;
using Product.Application.Queries;

namespace Product.Application.Interfaces;

public interface IProductService
{
    // Queries
    Task<IEnumerable<ProductDto>> HandleAsync(GetAllProductsQuery query);
    Task<ProductDto?> HandleAsync(GetProductByIdQuery query);
    
    // Commands
    Task<ProductDto> HandleAsync(CreateProductCommand command);
    Task<ProductDto> HandleAsync(UpdateProductCommand command);
    Task<bool> HandleAsync(DeleteProductCommand command);
}
