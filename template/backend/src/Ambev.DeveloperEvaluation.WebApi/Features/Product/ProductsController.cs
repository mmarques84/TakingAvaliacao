
using Ambev.DeveloperEvaluation.Domain.Entities;
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

        // Endpoint para listar todos os produtos
        [HttpGet]
        public async Task<IActionResult> GetAllProductsAsync()
        {
            try
            {
                var products = await _productService.GetAllProductsAsync();
                return Ok(products); // Retorna todos os produtos com código 200
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); // Retorna erro caso ocorra um problema
            }
        }

        // Endpoint para obter um produto específico por ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductByIdAsync(Guid id)
        {
            try
            {
                var product = await _productService.GetProductByIdAsync(id);
                if (product == null)
                {
                    return NotFound("Produto não encontrado."); // Retorna 404 se o produto não existir
                }
                return Ok(product); // Retorna o produto encontrado com código 200
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); // Retorna erro caso ocorra um problema
            }
        }

        // Endpoint para criar um novo produto
        [HttpPost]
        public async Task<IActionResult> CreateProductAsync([FromBody] CreateProductRequest createProductRequest)
        {
            if (createProductRequest == null)
                return BadRequest("Dados do produto não fornecidos.");

            try
            {
             
                var ProdMapper = _mapper.Map<Item>(createProductRequest);
                var createdProduct = await _productService.CreateProductAsync(ProdMapper);
                return CreatedAtAction(nameof(GetProductByIdAsync), new { id = createdProduct.Id }, createdProduct);
                // Retorna 201 com o recurso criado e o local para obter esse recurso
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); // Retorna erro caso ocorra um problema
            }
        }

        // Endpoint para atualizar um produto existente
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProductAsync(Guid id, [FromBody] CreateProductRequest updateProductRequest)
        {
            if (updateProductRequest == null)
                return BadRequest("Dados do produto não fornecidos.");

            try
            {
          
                var updatedProduct =  _productService.UpdateProductAsync(id, updateProductRequest.Name, updateProductRequest.UnitPrice);
                if (updatedProduct == null)
                    return NotFound("Produto não encontrado.");

                return Ok(updatedProduct); // Retorna o produto atualizado
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); // Retorna erro caso ocorra um problema
            }
        }

        // Endpoint para excluir um produto
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductAsync(Guid id)
        {
            try
            {
                var deleted = _productService.DeleteProductAsync(id);
                if (deleted !=null)
                    return NotFound("Produto não encontrado.");

                return NoContent(); // Retorna 204 sem conteúdo, indicando que a exclusão foi bem-sucedida
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); // Retorna erro caso ocorra um problema
            }
        }
    }
}
