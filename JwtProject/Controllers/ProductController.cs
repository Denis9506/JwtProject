using JwtProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JwtProject.Controllers
{
    [Route("api/products")]
    [ApiController]
    [Authorize] 
    public class ProductController : ControllerBase
    {
        private readonly JwtContext _context;

        public ProductController(JwtContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<Product>> AddProduct(Product product)
        {
            try
            {
                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
            }
            catch (Exception ex)
            {
                return BadRequest($"Could not add product: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            try
            {
                var products = await _context.Products.ToListAsync();
                return Ok(products);
            }
            catch (Exception ex)
            {
                return BadRequest($"Could not retrieve products: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);

                if (product == null)
                {
                    return NotFound("Product not found");
                }

                return Ok(product);
            }
            catch (Exception ex)
            {
                return BadRequest($"Could not retrieve product: {ex.Message}");
            }
        }
    }
}
