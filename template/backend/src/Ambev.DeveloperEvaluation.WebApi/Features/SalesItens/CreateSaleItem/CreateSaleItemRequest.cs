namespace Ambev.DeveloperEvaluation.WebApi.Features.SalesItens.CreateSaleItem
{
    public class CreateSaleItemRequest
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        //public decimal UnitPrice { get; set; }
        //public decimal Discount { get; set; }
    }
}
