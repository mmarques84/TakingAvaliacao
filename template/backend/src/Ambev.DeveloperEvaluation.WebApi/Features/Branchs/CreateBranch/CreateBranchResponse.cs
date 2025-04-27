namespace Ambev.DeveloperEvaluation.WebApi.Features.Branchs.CreateBranch
{
    public class CreateBranchResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedAt { get; set; }
    }

}
