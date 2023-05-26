using AutoMapper;
using IS.ScaleModelsShop.API.Contracts.Category;
using IS.ScaleModelsShop.API.Contracts.Manufacturer;
using IS.ScaleModelsShop.API.Contracts.Manufacturer.UpdateManufacturer;
using IS.ScaleModelsShop.API.Contracts.Product;
using IS.ScaleModelsShop.API.Contracts.Product.CreateProduct;
using IS.ScaleModelsShop.API.Contracts.Product.GetProduct;
using IS.ScaleModelsShop.API.Contracts.Product.UpdateProduct;
using IS.ScaleModelsShop.Application.Features.Categories.Commands.CreateCategory;
using IS.ScaleModelsShop.Application.Features.Manufacturers.Commands.UpdateManufacturer;
using IS.ScaleModelsShop.Application.Features.Products.Commands.CreateProduct;
using IS.ScaleModelsShop.Application.Features.Products.Commands.UpdateProduct;
using IS.ScaleModelsShop.Domain.Entities;

namespace IS.ScaleModelsShop.Application.Profiles;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<Category, CreateCategoryCommand>().ReverseMap();
        CreateMap<Category, CategoryModel>().ReverseMap();
        
        CreateMap<Manufacturer, ManufacturerModel>().ReverseMap();
        CreateMap<UpdateManufacturerModel, UpdateManufacturerCommand>().ReverseMap();
        CreateMap<Manufacturer, UpdateManufacturerCommand>().ReverseMap();

        CreateMap<Product, ProductModel>().ReverseMap();
        CreateMap<Product, ProductByManufacturerModel>().ReverseMap();
        CreateMap<Product, ProductByCategoryModel>().ReverseMap();
        CreateMap<Product, CreateProductCommand>().ReverseMap();
        CreateMap<Product, CreateProductModel>().ReverseMap();
        CreateMap<UpdateProductCommand, UpdateProductModel>().ReverseMap();
        CreateMap<Product, UpdateProductCommand>().ReverseMap();
    }
}