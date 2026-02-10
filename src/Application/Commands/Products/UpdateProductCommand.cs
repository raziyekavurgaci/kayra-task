using Application.DTOs.Products;

namespace Application.Commands.Products
{
    public class UpdateProductCommand
    {
        public int Id { get; set; }
        public UpdateProductDto ProductDto { get; set; } = new();
    }
}
