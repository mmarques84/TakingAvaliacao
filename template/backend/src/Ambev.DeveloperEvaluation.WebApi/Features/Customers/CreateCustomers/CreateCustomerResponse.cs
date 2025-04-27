namespace Ambev.DeveloperEvaluation.WebApi.Features.Customers.CreateCustomers
{
    public class CreateCustomerResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
