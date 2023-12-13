using CatalogAPI.Context;
using CatalogAPI.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CatalogAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {

        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllAsync()
        {
            try
            {
                var products = await _context.Products.Take(10).AsNoTracking().ToListAsync();
                if (products == null)
                {
                    return NotFound("No there registered products.");
                }
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred with your request. \nMessage: {ex.Message}");
            }
        }

        [HttpGet("{id}", Name ="GetProduct")]

        public async Task<ActionResult<Product>> GetAsync(Guid id) 
        {
            try
            {
                var Product = await _context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.ProductId == id);
                if (Product == null)
                {
                    return NotFound("Product not found");
                }
                return Ok(Product);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred with your request. \nMessage: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(Product product)
        {
            try
            {
                if(product == null)
                {
                    return BadRequest();
                };

                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();
                return new CreatedAtRouteResult("GetProduct", new { id = product.ProductId }, product );
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred with your request. \nMessage: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutAsync(Guid id, Product product)
        {
            try
            {
                var findedProduct = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == id);
                if (findedProduct == null)
                {
                    return NotFound("Product not found.");
                }
                _context.Entry(findedProduct).CurrentValues.SetValues(product);
                await _context.SaveChangesAsync();
                return Ok(findedProduct); 
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred with your request. \nMessage: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]   
        public async Task<ActionResult> DeleteAsync(Guid id)
        {
            try
            {
                var product = await _context.Products.FirstOrDefaultAsync(x => x.ProductId == id);
                if (product == null)
                {
                    return NotFound("Product not found");
                }
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred with your request. \nMessage: {ex.Message}");
            }
        }
    }
}
