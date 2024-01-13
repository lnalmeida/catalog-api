using CatalogAPI.Context;
using CatalogAPI.Domain;
using CatalogAPI.Domain.DTO;
using CatalogAPI.Repository;
using CatalogAPI.Repository.Interfaces;
using CatalogAPI.UnityOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CatalogAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {

        private readonly IUnityOfWork _unityOfWork;

        public ProductsController(IUnityOfWork unityOfWork)
        {
            _unityOfWork = unityOfWork;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAllAsync()
        {
            try
            {
                var products = await _unityOfWork.ProductRepository.GetAllAsync();
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
                var Product = await _unityOfWork.ProductRepository.GetAsync(id);
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

                await _unityOfWork.ProductRepository.CreateAsync(product);
                _unityOfWork.Commit();
                return new CreatedAtRouteResult("GetProduct", new { id = product.ProductId }, product );
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred with your request. \nMessage: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutAsync(Product product)
        {
            try
            {
                await _unityOfWork.ProductRepository.UpdateAsync(product);
                _unityOfWork.Commit();
                return Ok(product); 
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
                await _unityOfWork.ProductRepository.DeleteAsync(id);
                _unityOfWork.Commit();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred with your request. \nMessage: {ex.Message}");
            }
        }
    }
}
