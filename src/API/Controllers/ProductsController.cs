using Microsoft.AspNetCore.Mvc;
using Application.Commands.Products;
using Application.DTOs.Products;
using Application.Interfaces;
using Application.Queries.Products;

namespace API.Controllers
{
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
                return NotFound(new { message = $"Ürün bulunamadı (Id: {id})" });
            
            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<ProductDto>> Create([FromBody] CreateProductDto dto)
        {
            var command = new CreateProductCommand { ProductDto = dto };
            var product = await _productService.HandleAsync(command);
            
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProductDto>> Update(int id, [FromBody] UpdateProductDto dto)
        {
            var command = new UpdateProductCommand { Id = id, ProductDto = dto };
            var product = await _productService.HandleAsync(command);
            
            if (product == null)
                return NotFound(new { message = $"Ürün bulunamadı (Id: {id})" });
            
            return Ok(product);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var command = new DeleteProductCommand { Id = id };
            var result = await _productService.HandleAsync(command);
            
            if (!result)
                return NotFound(new { message = $"Ürün bulunamadı (Id: {id})" });
            
            return NoContent();
        }
    }
}
