namespace Ambev.DeveloperEvaluation.WebApi.Features.Customers.CreateCustomers
{
    public class CreateCustomerRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
