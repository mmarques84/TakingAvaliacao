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
            // Mapeamentos básicos
            CreateMap<CreateBranchRequest, Branch>();
            CreateMap<Branch, CreateBranchResponse>();

            CreateMap<CreateCustomerRequest, Customer>();
            CreateMap<Customer, CreateCustomerResponse>();

            CreateMap<CreateProductRequest, Item>();
            CreateMap<Item, CreateProductResponse>();

            // Mapeamento de itens da venda
            CreateMap<CreateSaleItemRequest, SaleItem>()
                .ForMember(dest => dest.TotalAmount, opt => opt.Ignore()) // será calculado no serviço
                .ForMember(dest => dest.Sale, opt => opt.Ignore())        // evitar ciclos
                .ForMember(dest => dest.SaleId, opt => opt.Ignore());     // setado depois

            // Mapeamento da venda
            CreateMap<CreateSaleRequest, Sale>()
                .ForMember(dest => dest.TotalAmount, opt => opt.Ignore()) // calculado no serviço
                .ForMember(dest => dest.SaleItems, opt => opt.MapFrom(src => src.SaleItems));

            // Resposta de venda
            CreateMap<Sale, CreateSaleResponse>();

            CreateMap<Sale, CreateSaleResponse>()
                .ForMember(dest => dest.SaleItems, opt => opt.MapFrom(src => src.SaleItems));
            CreateMap<CreateSaleRequest, Sale>()
                .ForMember(dest => dest.SaleItems, opt => opt.MapFrom(src => src.SaleItems));
            CreateMap<SaleItem, CreateSaleItemResponse>();

            CreateMap<Sale, CreateSaleResponse>()
                .ForMember(dest => dest.SaleItems, opt => opt.MapFrom(src => src.SaleItems));
        }
    }
}
