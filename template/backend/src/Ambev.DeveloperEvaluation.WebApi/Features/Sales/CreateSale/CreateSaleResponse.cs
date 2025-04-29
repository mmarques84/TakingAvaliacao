using Ambev.DeveloperEvaluation.WebApi.Features.SalesItens.CreateSaleItem;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale
{
    public class CreateSaleResponse
    {
        public Guid Id { get; set; } // ID da venda
        public Guid CustomerId { get; set; } // ID do cliente
        public Guid BranchId { get; set; } // ID da filial
        public DateTime SaleDate { get; set; } // Data da venda
        public decimal TotalAmount { get; set; } // Total da venda (com os descontos aplicados)
        public List<CreateSaleItemResponse> SaleItems { get; set; } // Itens da venda
        public bool IsCancelled;
    }

   

}
