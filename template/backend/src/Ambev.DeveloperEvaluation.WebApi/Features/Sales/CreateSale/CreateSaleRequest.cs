using Ambev.DeveloperEvaluation.WebApi.Features.SalesItens.CreateSaleItem;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale
{
    public class CreateSaleRequest
    {
        public long SaleNumber{ get; set; }
        public Guid IdCustomer { get; set; }
        public Guid IdBranch { get; set; }
        public List<CreateSaleItemRequest> SaleItems { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime SaleDate { get; set; }

        public bool IsCancelled = false;
    }

   
}
