using Microsoft.AspNetCore.Mvc;
using ProductApi.DTOs;
using ProductApi.Services;

namespace ProductApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductsController(IProductService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAll()
        {
            var products = await _service.GetAllProductsAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetById(int id)
        {
            var product = await _service.GetProductByIdAsync(id);
            
            if (product == null)
            {
                return NotFound(new { message = $"Ürün bulunamadı (Id: {id})" });
            }
            
            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<ProductDto>> Create([FromBody] CreateProductDto createDto)
        {
            try
            {
                var product = await _service.CreateProductAsync(createDto);
                return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProductDto>> Update(int id, [FromBody] UpdateProductDto updateDto)
        {
            try
            {
                var product = await _service.UpdateProductAsync(id, updateDto);
                
                if (product == null)
                {
                    return NotFound(new { message = $"Ürün bulunamadı (Id: {id})" });
                }
                
                return Ok(product);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _service.DeleteProductAsync(id);
            
            if (!result)
            {
                return NotFound(new { message = $"Ürün bulunamadı (Id: {id})" });
            }
            
            return NoContent();
        }
    }
}
