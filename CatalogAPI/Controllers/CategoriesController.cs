using CatalogAPI.Context;
using CatalogAPI.Domain;
using CatalogAPI.Domain.DTO;
using CatalogAPI.Repository.Interfaces;
using CatalogAPI.UnityOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CatalogAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository<Category> _categoryRepo;

        public CategoriesController (ICategoryRepository<Category> categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetAllAsync()
        {
            try
            {
                var categories = await _categoryRepo.GetAllAsync();
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
        public async Task<ActionResult<IEnumerable<Category>>> GetCategoryProductsAsync(string categoryId)
        {
            try
            {
                var categoryProducts = await _categoryRepo.GetCategoryProducts(categoryId);
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
        public async Task<ActionResult<Category>> GetByIdAsync(string id) 
        {
            try
            {
                var category = await _categoryRepo.GetAsync(id);
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
        public async Task<ActionResult> PostAsync(Category entity)
        {
            try
            {
                if(entity is null)
                {
                    return BadRequest();
                }
                await _categoryRepo.CreateAsync(entity);
                return new CreatedAtRouteResult("GetCategory", new { id = entity.CategoryId }, entity);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred with your request. \nMessage: {ex.Message}");
            }           
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Category>> PutAsync(Category entity) 
        {
            try
            {
                var updatedCategory = await _categoryRepo.UpdateAsync(entity);

                if (updatedCategory is null) return NotFound("Category not found");
                return Ok(entity);
            }
            catch(Exception ex) 
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred with your request. \nMessage: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(string id)
        {
            try
            {
                await _categoryRepo.DeleteAsync(id);
                return Ok();
            }
            catch( Exception ex) 
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred with your request. \nMessage: {ex.Message}");
            }
        }
    }
}
