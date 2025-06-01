using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using Web_Shop.Application.DTOs;
using Web_Shop.Application.Services.Interfaces;
using Web_Shop.Persistence.UOW.Interfaces;
using Web_Shop.Application.Mappings;
using Sieve.Models;
using Web_Shop.Application.Helpers.PagedList;
using WWSI_Shop.Persistence.MySQL.Model;

namespace Web_Shop.RestAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductController> _logger;

        public ProductController(ILogger<ProductController> logger, IProductService productService)
        {
            _productService = productService;
            _logger = logger;
        }

        [HttpGet("{id}")]
        [SwaggerOperation(OperationId = "GetProductById")]
        public async Task<ActionResult<GetSingleProductDTO>> GetProduct(ulong id)
        {
            var result = await _productService.GetByIdAsync(id);

            if (!result.IsSuccess)
            {
                return Problem(statusCode: (int)result.StatusCode, title: "Read error.", detail: result.ErrorMessage);
            }

            return StatusCode((int)result.StatusCode, result.entity!.MapGetSingleProductDTO());
        }

        [HttpGet("list")]
        [SwaggerOperation(OperationId = "GetProducts")]
        public async Task<ActionResult<IPagedList<GetSingleProductDTO>>> GetProducts([FromQuery] SieveModel paginationParams)
        {
            var result = await _productService.SearchAsync(paginationParams, resultEntity => DomainToDtoMapper.MapGetSingleProductDTO(resultEntity));

            if (!result.IsSuccess)
            {
                return Problem(statusCode: (int)result.StatusCode, title: "Read error.", detail: result.ErrorMessage);
            }

            return Ok(result.entityList);
        }

        [HttpPost("add")]
        [SwaggerOperation(OperationId = "AddProduct")]
        public async Task<ActionResult<GetSingleProductDTO>> AddProduct([FromBody] AddUpdateProductDTO dto)
        {
            var result = await _productService.CreateNewProductAsync(dto);

            if (!result.IsSuccess)
            {
                return Problem(statusCode: (int)result.StatusCode, title: "Add error.", detail: result.ErrorMessage);
            }

            return CreatedAtAction(nameof(GetProduct), new { id = result.entity.IdProduct }, result.entity.MapGetSingleProductDTO());
        }

        [HttpPut("update/{id}")]
        [SwaggerOperation(OperationId = "UpdateProduct")]
        public async Task<ActionResult<GetSingleProductDTO>> UpdateProduct(ulong id, [FromBody] AddUpdateProductDTO dto)
        {
            var result = await _productService.UpdateExistingProductAsync(dto, id);

            if (!result.IsSuccess)
            {
                return Problem(statusCode: (int)result.StatusCode, title: "Update error.", detail: result.ErrorMessage);
            }

            return StatusCode((int)result.StatusCode, result.entity.MapGetSingleProductDTO());
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(OperationId = "DeleteProduct")]
        public async Task<IActionResult> DeleteProduct(ulong id)
        {
            var result = await _productService.DeleteAndSaveAsync(id);

            if (!result.IsSuccess)
            {
                return Problem(statusCode: (int)result.StatusCode, title: "Delete error.", detail: result.ErrorMessage);
            }

            return NoContent();
        }
    }
}
