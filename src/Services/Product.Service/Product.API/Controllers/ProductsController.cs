using Microsoft.AspNetCore.Mvc;
using Product.Application.Commands;
using Product.Application.DTOs;
using Product.Application.Interfaces;
using Product.Application.Queries;

namespace Product.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetAll()
    {
        var query = new GetAllProductsQuery();
        var products = await _productService.HandleAsync(query);
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetById(int id)
    {
        var query = new GetProductByIdQuery { Id = id };
        var product = await _productService.HandleAsync(query);
        
        if (product == null)
            return NotFound(new { message = $"Product with ID {id} not found" });

        return Ok(product);
    }

    [HttpPost]
    public async Task<ActionResult<ProductDto>> Create([FromBody] CreateProductDto dto)
    {
        try
        {
            var command = new CreateProductCommand { Product = dto };
            var product = await _productService.HandleAsync(command);
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ProductDto>> Update(int id, [FromBody] UpdateProductDto dto)
    {
        try
        {
            var command = new UpdateProductCommand { Id = id, Product = dto };
            var product = await _productService.HandleAsync(command);
            return Ok(product);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var command = new DeleteProductCommand { Id = id };
        var result = await _productService.HandleAsync(command);

        if (!result)
            return NotFound(new { message = $"Product with ID {id} not found" });

        return NoContent();
    }
}
