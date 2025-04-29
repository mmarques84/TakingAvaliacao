using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Ports.Interfaces;
using Ambev.DeveloperEvaluation.WebApi.Features.Product.CreateProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        private readonly ISaleService _saleService;
        private readonly IMapper _mapper;
       
        public SalesController(ISaleService saleService, IMapper mapper)
        {
            _saleService = saleService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSaleAsync([FromBody] CreateSaleRequest createSaleRequest)
        {
            if (createSaleRequest == null)
            {
                var errorResponse = new ErrorResponse(400, "Dados da venda não fornecidos.");
                return BadRequest(errorResponse);
            }

            try
            {
                //var SaleMapper = _mapper.Map<Sale>(createSaleRequest);
                var SaleMapper = new Sale
                {
                    IdCustomer = createSaleRequest.IdCustomer,
                    IdBranch = createSaleRequest.IdBranch,
                    SaleDate = createSaleRequest.SaleDate,
                    IsCancelled = createSaleRequest.IsCancelled,
                    SaleItems = _mapper.Map<List<SaleItem>>(createSaleRequest.SaleItems)
                };
                var sale = await _saleService.CreateSaleAsync(SaleMapper);
                return Ok(sale);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Obter uma venda por ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSaleByIdAsync(Guid id)
        {
            try
            {
                var sale = await _saleService.GetSaleByIdAsync(id);
                if (sale == null)
                {
                    var errorResponse = new ErrorResponse(404, "Venda não encontrada.");
                    return NotFound(errorResponse);
                }
                var resultMap = _mapper.Map<CreateSaleResponse>(sale);
                return Ok(resultMap);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSalesAsync()
        {
            try
            {
                var sales = await _saleService.GetAllSalesAsync();
                return Ok(sales);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("cancel/{saleId}")]
        public async Task<IActionResult> CancelSaleAsync(Guid saleId)
        {
            try
            {
                await _saleService.CancelSaleAsync(saleId);
                return Ok(new { Message = "Venda cancelada com sucesso!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
        // Atualizar uma venda
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSaleAsync(Guid id, [FromBody] UpdateSaleRequest updateSaleRequest)
        {
            if (updateSaleRequest == null)
            {
                var errorResponse = new ErrorResponse(404, "Dados da venda não fornecidos.");
                return BadRequest(errorResponse);
            }  

            try
            {
                var SaleMapper = _mapper.Map<Sale>(updateSaleRequest);
                var updatedSale = await _saleService.UpdateAsync(id, SaleMapper);
                if (updatedSale == null)
                {
                    var errorResponse = new ErrorResponse(404, "Venda não encontrada.");
                    return NotFound(errorResponse);
                }
     

                return Ok(updatedSale);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Excluir uma venda
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSaleAsync(Guid id)
        {
            try
            {
                var result = await _saleService.DeleteAsync(id);
                if (!result)
                {
                    var errorResponse = new ErrorResponse(404, "Venda não encontrada.");
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
