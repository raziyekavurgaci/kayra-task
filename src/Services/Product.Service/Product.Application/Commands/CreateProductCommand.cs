using Product.Application.DTOs;

namespace Product.Application.Commands;

public class CreateProductCommand
{
    public CreateProductDto Product { get; set; } = null!;
}
