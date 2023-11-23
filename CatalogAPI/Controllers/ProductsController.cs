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
        public ActionResult<IEnumerable<Product>> GetAll()
        {
            try
            {
                var products = _context.Products.Take(10).AsNoTracking().ToList();
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

        public ActionResult<Product> Get(Guid id) 
        {
            try
            {
                var Product = _context.Products.AsNoTracking().FirstOrDefault(p => p.ProductId == id);
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
        public ActionResult Post(Product product)
        {
            try
            {
                if(product == null)
                {
                    return BadRequest();
                };

                _context.Products.Add(product);
                _context.SaveChanges();
                return new CreatedAtRouteResult("GetProduct", new { id = product.ProductId }, product );
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred with your request. \nMessage: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public ActionResult Put(Guid id, Product product)
        {
            try
            {
                var findedProduct = _context.Products.FirstOrDefault(p => p.ProductId == id);
                if (findedProduct == null)
                {
                    return NotFound("Product not found.");
                }
                _context.Entry(findedProduct).CurrentValues.SetValues(product);
                _context.SaveChanges();
                return Ok(findedProduct); 
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred with your request. \nMessage: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]   
        public ActionResult Delete(Guid id)
        {
            try
            {
                var product = _context.Products.FirstOrDefault(x => x.ProductId == id);
                if (product == null)
                {
                    return NotFound("Product not found");
                }
                _context.Products.Remove(product);
                _context.SaveChanges();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred with your request. \nMessage: {ex.Message}");
            }
        }
    }
}
