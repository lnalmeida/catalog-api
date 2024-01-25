using AutoMapper;
using CatalogAPI.Domain;
using CatalogAPI.DTO;
using CatalogAPI.Pagination;
using CatalogAPI.UnityOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;

namespace CatalogAPI.Controllers;

[Authorize(AuthenticationSchemes = "Bearer")]
[Route("[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly IUnityOfWork _unityOfWork;
    private readonly IMapper _mapper;

    public CategoriesController (IUnityOfWork unityOfWork, IMapper mapper)
    {
        _unityOfWork = unityOfWork;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAllAsync([FromQuery] PaginationParameters paginationParameters)
    {
        try
        {
            var categories = await _unityOfWork.CategoryRepository.GetAll(paginationParameters);
            if(categories is null) 
            {
                return NotFound("No there registered categpries");
            }
                
            var metadata = new
            {
                categories.TotalCount,
                categories.PageSize,
                categories.CurrentPage,
                categories.TotalPages,
                categories.HasNext,
                categories.HasPrevious
            };
                
            Response.Headers.Add("X-Pagination", metadata.ToJson());

            var categoriesDto = _mapper.Map<List<CategoryDto>>(categories);
            return Ok(categoriesDto);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred with your request. \nMessage: {ex.Message}");
        }

    }

    [HttpGet("products/{categoryId}")]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetCategoryProductsAsync(string categoryId)
    {
        try
        {
            var categoryProducts = await _unityOfWork.CategoryRepository.GetCategoryProducts(categoryId);
            if (categoryProducts is null)
            {
                return NotFound("Category not found.");
            }
            var categoryProductsDto = _mapper.Map<List<CategoryDto>>(categoryProducts);
            return Ok(categoryProductsDto);
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
            var parsedId = Guid.Parse(id);
            var category = await _unityOfWork.CategoryRepository.GetAsync(c => c.CategoryId == parsedId);
            if(category is null)
            {
                return NotFound("Category not found.");
            }

            var categoryDto = _mapper.Map<CategoryDto>(category);
            return Ok(categoryDto);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred with your request. \nMessage: {ex.Message}");
        }
            
    }

    [HttpPost]
    public async Task<ActionResult> PostAsync(CategoryDto entityDto)
    {
        try
        {
            if(entityDto is null)
            {
                return BadRequest();
            }

            var entity = _mapper.Map<Category>(entityDto);
            _unityOfWork.CategoryRepository.CreateAsync(entity);
            await _unityOfWork.Commit();
            return new CreatedAtRouteResult("GetCategory", new { id = entityDto.CategoryId }, entityDto);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred with your request. \nMessage: {ex.Message}");
        }           
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<CategoryDto>> PutAsync(CategoryDto entityDto) 
    {
        try
        {
            var updatedCategory =  await _unityOfWork.CategoryRepository.GetAsync(c => c.CategoryId == entityDto.CategoryId);

            if (updatedCategory is null) return NotFound("Category not found");
            var entity = _mapper.Map<Category>(entityDto);
            _unityOfWork.CategoryRepository.UpdateAsync(entity);
            await _unityOfWork.Commit();
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
            var parsedId = Guid.Parse(id);
            var deletedCategory = await _unityOfWork.CategoryRepository.GetAsync(c => c.CategoryId == parsedId);
            if (deletedCategory is null) return NotFound("Category not found");
            _unityOfWork.CategoryRepository.DeleteAsync(deletedCategory);
            await _unityOfWork.Commit();
            return Ok();
        }
        catch( Exception ex) 
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred with your request. \nMessage: {ex.Message}");
        }
    }
}