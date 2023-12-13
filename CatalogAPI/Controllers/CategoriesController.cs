using CatalogAPI.Context;
using CatalogAPI.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CatalogAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoriesController (AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetAllAsync()
        {
            try
            {
                var categories = await _context.Categories.Take(10).AsNoTracking().ToListAsync();
                if(categories is null) 
                {
                    return NotFound("No there registered categpries");
                }
                return Ok(categories);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred with your request. \nMessage: {ex.Message}");
            }

        }

        [HttpGet("products/{categoryId}")]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategoryProductsAsync(Guid categoryId)
        {
            try
            {
                var categoryProducts = await _context.Categories.AsNoTracking().Include(p => p.Products).Where(p => p.CategoryId == categoryId).ToListAsync();
                if (categoryProducts is null)
                {
                    return NotFound("Category not found.");
                }
                return Ok(categoryProducts);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred with your request. \nMessage: {ex.Message}");
            }

        }

        [HttpGet("{id}", Name ="GetCategory")]
        public async Task<ActionResult<Category>> GetByIdAsync(Guid id) 
        {
            try
            {
                var category = await _context.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.CategoryId == id);
                if(category is null)
                {
                    return NotFound("Category not found.");
                }
            
                return Ok(category);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred with your request. \nMessage: {ex.Message}");
            }
            
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(Category category)
        {
            try
            {
                if(category is null)
                {
                    return BadRequest();
                }
                await _context.Categories.AddAsync(category);
                await _context.SaveChangesAsync();
                return new CreatedAtRouteResult("GetCategory", new { id = category.CategoryId }, category);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred with your request. \nMessage: {ex.Message}");
            }           
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Category>> PutAsync(Guid id,  Category category) 
        {
            try
            {
                var findedCategory = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == id);
                if(findedCategory is null) 
                {
                    return NotFound("Category not found.");
                }

                _context.Entry(findedCategory).CurrentValues.SetValues(category);
                 await _context.SaveChangesAsync();
                return Ok(findedCategory);
            }
            catch(Exception ex) 
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred with your request. \nMessage: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(Guid id)
        {
            try
            {
                var findedCategory = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == id);
                if(findedCategory is null)
                {
                    return NotFound("Category not found.");
                }
                _context.Categories.Remove(findedCategory);
                await _context.SaveChangesAsync();
                return Ok(findedCategory);
            }
            catch( Exception ex) 
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred with your request. \nMessage: {ex.Message}");
            }
        }
    }
}
