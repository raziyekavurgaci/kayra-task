using Product.Application.DTOs;

namespace Product.Application.Commands;

public class UpdateProductCommand
{
    public int Id { get; set; }
    public UpdateProductDto Product { get; set; } = null!;
}
