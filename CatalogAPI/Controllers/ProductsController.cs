using AutoMapper;
using CatalogAPI.Domain;
using CatalogAPI.DTO;
using CatalogAPI.Repository.Interfaces;
using CatalogAPI.UnityOfWork;
using Microsoft.AspNetCore.Mvc;

namespace CatalogAPI.Controllers
{
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllAsync()
        {
            try
            {
                var products = _unityOfWork.ProductRepository.GetAllAsync();
                if (products == null)
                {
                    return NotFound("No there registered products.");
                }

                var productsDto = _mapper.Map<List<ProductDto>>(products);
                return Ok(productsDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred with your request. \nMessage: {ex.Message}");
            }
        }

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

        [HttpPost]
        public async Task<ActionResult> PostAsync(ProductDto entityDto)
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
