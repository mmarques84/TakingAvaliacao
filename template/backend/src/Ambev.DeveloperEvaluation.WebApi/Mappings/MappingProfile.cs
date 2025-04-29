using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Ports.DTOs;
using Ambev.DeveloperEvaluation.WebApi.Features.Branchs.CreateBranch;
using Ambev.DeveloperEvaluation.WebApi.Features.Customers.CreateCustomers;
using Ambev.DeveloperEvaluation.WebApi.Features.Product.CreateProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.SalesItens.CreateSaleItem;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateBranchRequest, Branch>();
            CreateMap<Branch, CreateBranchResponse>();

            CreateMap<CreateCustomerRequest, Customer>();
            CreateMap<Customer, CreateCustomerResponse>();

            CreateMap<CreateProductRequest, Item>();
            //CreateMap<Item, ProductResponse>();

            CreateMap<CreateSaleRequest, Sale>();
            CreateMap<CreateSaleItemRequest, SaleItem>();
            //CreateMap<Sale, SaleResponse>();
        }
    }
}
