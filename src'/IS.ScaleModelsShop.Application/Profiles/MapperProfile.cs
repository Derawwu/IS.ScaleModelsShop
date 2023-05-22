using AutoMapper;
using IS.ScaleModelsShop.Application.Features.Categories.Commands.CreateCategory;
using IS.ScaleModelsShop.Application.Features.Categories.Queries.GetAllCategoriesList;
using IS.ScaleModelsShop.Application.Features.Manufacturers.Commands.CreateManufacturer;
using IS.ScaleModelsShop.Application.Features.Manufacturers.Commands.UpdateManufacturer;
using IS.ScaleModelsShop.Application.Features.Manufacturers.Queries.GetAllManufacturersList;
using IS.ScaleModelsShop.Application.Features.Products.Commands.CreateProduct;
using IS.ScaleModelsShop.Application.Features.Products.Commands.UpdateProduct;
using IS.ScaleModelsShop.Application.Features.Products.Queries.GetAllProductsPaginated;
using IS.ScaleModelsShop.Application.Features.Products.Queries.GetProductByName;
using IS.ScaleModelsShop.Application.Features.Products.Queries.GetProductsByCategory;
using IS.ScaleModelsShop.Domain.Entities;

namespace IS.ScaleModelsShop.Application.Profiles
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Category, CreateCategoryCommand>().ReverseMap();
            CreateMap<Category, CreateCategoryDTO>().ReverseMap();
            CreateMap<Category, CategoryListViewModel>().ReverseMap();
            CreateMap<CreateCategoryDTO, CreateCategoryCommand>().ReverseMap();

            CreateMap<Manufacturer, CreateManufacturerDTO>().ReverseMap();
            CreateMap<Manufacturer, CreateManufacturerCommand>().ReverseMap();
            CreateMap<Manufacturer, ManufacturersListViewModel>().ReverseMap();
            CreateMap<UpdateManufacturerDTO, UpdateManufacturerCommand>().ReverseMap();
            CreateMap<Manufacturer, UpdateManufacturerCommand>().ReverseMap();

            CreateMap<Product, GetProductByNameViewModel>().ReverseMap();
            CreateMap<Product, CreateProductCommand>().ReverseMap();
            CreateMap<ProductsDTO, Product>().ReverseMap()
                .ForMember(x => x.Manufacturer, opt => opt.MapFrom(src => src.ManufacturerId));
            CreateMap<Product, GetProductDTO>().ReverseMap();
            CreateMap<UpdateProductCommand, UpdateProductDTO>().ReverseMap()
                .ForMember(x => x.ManufacturerId, opt => opt.MapFrom(src => src.Manufacturer))
                .ForMember(x => x.CategoryId, opt => opt.MapFrom(src => src.Category));
            CreateMap<Product, UpdateProductCommand>().ReverseMap();
        }
    }
}