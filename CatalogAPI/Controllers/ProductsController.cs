using AutoMapper;
using CatalogAPI.Domain;
using CatalogAPI.DTO;
using CatalogAPI.Pagination;
using CatalogAPI.UnityOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;

namespace CatalogAPI.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {

        private readonly IUnityOfWork _unityOfWork;
        private readonly IMapper _mapper;

        public ProductsController(IUnityOfWork unityOfWork, IMapper mapper)
        {
            _unityOfWork = unityOfWork;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<PagedList<ProductDto>>> GetAllAsync([FromQuery] PaginationParameters paginationParameters)
        {
            try
            {
                var products = await _unityOfWork.ProductRepository.GetAll(paginationParameters);
                if (products == null)
                {
                    return NotFound("No there registered products.");
                }

                var metadata = new
                {
                    products.TotalCount,
                    products.PageSize,
                    products.CurrentPage,
                    products.TotalPages,
                    products.HasNext,
                    products.HasPrevious
                };
                
                Response.Headers.Add("X-Pagination", metadata.ToJson());

                var productsDto = _mapper.Map<List<ProductDto>>(products);
                return Ok(productsDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred with your request. \nMessage: {ex.Message}");
            }
        }
        
        

        [AllowAnonymous]
        [HttpGet("{id}", Name ="GetProduct")]

        public async Task<ActionResult<ProductDto>> GetAsync(string id) 
        {
            try
            {
                var parsedId = Guid.Parse(id);
                var product = await _unityOfWork.ProductRepository.GetAsync(p => p.ProductId == parsedId);
                if (product == null)
                {
                    return NotFound("Product not found");
                }

                var productDto = _mapper.Map<ProductDto>(product);
                return Ok(productDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred with your request. \nMessage: {ex.Message}");
            }
        }

        [Authorize(Roles = "super, admin")]
        [HttpGet("stock")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductsByStockAsync(int quantity)
        {
            try
            {
                var productsByStockDto = await _unityOfWork.ProductRepository.GetProductsByStock(quantity);
                if (productsByStockDto is null)
                    return NotFound($"No there products that the stock less than or equals {quantity}");
                var producstsByStockDto = _mapper.Map<List<Product>>(productsByStockDto);
                return Ok(producstsByStockDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred with your request. \nMessage: {ex.Message}");
            }
        }

        [Authorize(Roles = "super, admin")]
        [HttpPost]
        public async Task<ActionResult> PostAsync(InsertProductDto entityDto)
        {
            try
            {
                if(entityDto == null)
                {
                    return BadRequest();
                };
                var newProduct = _mapper.Map<Product>(entityDto);
                _unityOfWork.ProductRepository.CreateAsync(newProduct);
                await _unityOfWork.Commit();
                return new CreatedAtRouteResult("GetProduct", new { id = newProduct.ProductId }, newProduct );
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred with your request. \nMessage: {ex.Message}");
            }
        }

        [Authorize(Roles = "super, admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult> PutAsync(ProductDto productDto)
        {
            try
            {
                var updatedProduct = await _unityOfWork.ProductRepository.GetAsync(p => p.ProductId == productDto.ProductId);
                if (updatedProduct is null) return NotFound("Product not found.");
                var entity = _mapper.Map<Product>(productDto);
                _unityOfWork.ProductRepository.UpdateAsync(entity);
                await _unityOfWork.Commit();
                return Ok(entity); 
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred with your request. \nMessage: {ex.Message}");
            }
        }

        [Authorize(Roles = "super")]
        [HttpDelete("{id}")]   
        public async Task<ActionResult> DeleteAsync(string id)
        {
            try
            {
                var parsedId = Guid.Parse(id);
                var deletedProduct = await _unityOfWork.ProductRepository.GetAsync(p => p.ProductId == parsedId);
                if (deletedProduct is null) return NotFound("Product not found");
                _unityOfWork.ProductRepository.DeleteAsync(deletedProduct);
                await _unityOfWork.Commit();
                return Ok(deletedProduct);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred with your request. \nMessage: {ex.Message}");
            }
        }
    }
}
