using CatalogAPI.Context;
using CatalogAPI.Domain;
using CatalogAPI.Domain.DTO;
using CatalogAPI.Repository;
using CatalogAPI.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CatalogAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {

        private readonly IProductRepository<ProductDTO> _productRepository;

        public ProductsController(IProductRepository<ProductDTO> productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAllAsync()
        {
            try
            {
                var products = await _productRepository.GetAllAsync();
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

        public async Task<ActionResult<Product>> GetAsync(string id) 
        {
            try
            {
                var Product = await _productRepository.GetAsync(id);
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
        public async Task<ActionResult> PostAsync(ProductDTO productDTO)
        {
            try
            {
                if(productDTO == null)
                {
                    return BadRequest();
                };

                await _productRepository.CreateAsync(productDTO);
                return new CreatedAtRouteResult("GetProduct", new { id = productDTO.ProductId }, productDTO );
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred with your request. \nMessage: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutAsync(Guid id, ProductDTO productDTO)
        {
            try
            {
                await _productRepository.UpdateAsync(productDTO);
                return Ok(productDTO); 
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
                await _productRepository.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred with your request. \nMessage: {ex.Message}");
            }
        }
    }
}
