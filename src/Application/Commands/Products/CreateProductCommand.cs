using Application.DTOs.Products;

namespace Application.Commands.Products
{
    public class CreateProductCommand
    {
        public CreateProductDto ProductDto { get; set; } = new();
    }
}
