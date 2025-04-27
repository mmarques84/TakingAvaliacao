
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Ports.Interfaces;
using Ambev.DeveloperEvaluation.WebApi.Features.Product.CreateProduct;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Product
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        public ProductsController(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

  
        [HttpGet]
        public async Task<IActionResult> GetAllProductsAsync()
        {
            try
            {
                var products = await _productService.GetAllProductsAsync();
                return Ok(products);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductByIdAsync(Guid id)
        {
            try
            {
                var product = await _productService.GetProductByIdAsync(id);

                if (product == null)
                {
                    var errorResponse = new ErrorResponse(404, "Produto não encontrado.");
                    return NotFound(errorResponse);
                }

                var productResponse = _mapper.Map<CreateProductResponse>(product);
                return Ok(productResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        public async Task<IActionResult> CreateProductAsync([FromBody] CreateProductRequest createProductRequest)
        {
            if (createProductRequest == null)
            {
                var errorResponse = new ErrorResponse(400, "Dados do produto não fornecidos.");
                return BadRequest(errorResponse);
            }

            try
            {

                var ProdMapper = _mapper.Map<Item>(createProductRequest);
                var createdProduct = await _productService.CreateProductAsync(ProdMapper);
                return CreatedAtAction(nameof(CreateProductResponse), new { id = createdProduct.Id }, createdProduct);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProductAsync(Guid id, [FromBody] CreateProductRequest updateProductRequest)
        {
            if (updateProductRequest == null)
            {
                var errorResponse = new ErrorResponse(400, "Dados do produto não fornecidos.");
                return BadRequest(errorResponse);
            }


            try
            {

                var updatedProduct = _productService.UpdateProductAsync(id, updateProductRequest.Name, updateProductRequest.UnitPrice);
                if (updatedProduct == null)
                {
                    var errorResponse = new ErrorResponse(404, "Produto não encontrado.");
                    return NotFound(errorResponse);
                }


                return Ok(updatedProduct);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductAsync(Guid id)
        {
            try
            {
                var deleted = _productService.DeleteProductAsync(id);
                if (deleted != null)
                {
                    var errorResponse = new ErrorResponse(404, "Produto não encontrado.");
                    return NotFound(errorResponse);
                }
                
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
