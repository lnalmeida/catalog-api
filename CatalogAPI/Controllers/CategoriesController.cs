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
        public ActionResult<IEnumerable<Category>> GetAll()
        {
            try
            {
                var categories = _context.Categories.Take(10).AsNoTracking().ToList();
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
        public ActionResult<IEnumerable<Category>> GetCategoryProducts(Guid categoryId)
        {
            try
            {
                var categoryProducts = _context.Categories.AsNoTracking().Include(p => p.Products).Where(p => p.CategoryId == categoryId).ToList();
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
        public ActionResult<Category> GetById(Guid id) 
        {
            try
            {
                var category = _context.Categories.AsNoTracking().FirstOrDefault(c => c.CategoryId == id);
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
        public ActionResult Post(Category category)
        {
            try
            {
                if(category is null)
                {
                    return BadRequest();
                }
                _context.Categories.Add(category);
                _context.SaveChanges();
                return new CreatedAtRouteResult("GetCategory", new { id = category.CategoryId }, category);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred with your request. \nMessage: {ex.Message}");
            }           
        }

        [HttpPut("{id}")]
        public ActionResult<Category> Put(Guid id,  Category category) 
        {
            try
            {
                var findedCategory = _context.Categories.FirstOrDefault(c => c.CategoryId == id);
                if(findedCategory is null) 
                {
                    return NotFound("Category not found.");
                }

                _context.Entry(findedCategory).CurrentValues.SetValues(category);
                _context.SaveChanges();
                return Ok(findedCategory);
            }
            catch(Exception ex) 
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred with your request. \nMessage: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(Guid id)
        {
            try
            {
                var findedCategory = _context.Categories.FirstOrDefault(c => c.CategoryId == id);
                if(findedCategory is null)
                {
                    return NotFound("Category not found.");
                }
                _context.Categories.Remove(findedCategory);
                _context.SaveChanges();
                return Ok(findedCategory);
            }
            catch( Exception ex) 
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred with your request. \nMessage: {ex.Message}");
            }
        }
    }
}
