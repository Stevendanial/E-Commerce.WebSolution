using AutoMapper;
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entites.Product_Module;
using E_Commerce.Service.Abstraction;
using E_Commerce.Services.Exceptions;
using E_Commerce.Services.Specifications;
using E_Commerce.Shared;
using E_Commerce.Shared.DTOs.ProductDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BrandDTO>> GetAllBrandsAsync()
        {
            var Brands=await _unitOfWork.GetRepository<ProductBrand,int>().GetAllAsync();

            return _mapper.Map<IEnumerable<BrandDTO>>(Brands);
        }

        public async Task<PaginatedResult<ProductDTO>> GetAllProductsAsync(ProductQueryParams queryParams)
        {
            var Repo = _unitOfWork.GetRepository<Product, int>();
            var Spec = new ProductWithTypeAndBrandSpecification(queryParams);
            var products = await Repo.GetAllAsync(Spec);
            var DataToReturn = _mapper.Map<IEnumerable<ProductDTO>>(products);
            var countofReturnedData =DataToReturn.Count();
            var CountSpec = new ProductCountSpecification(queryParams);
            var CountOfAllProducts =await Repo.CountAsync(Spec);
            return new PaginatedResult<ProductDTO>(queryParams.PageIndex, queryParams.PageSize,CountOfAllProducts,DataToReturn) ;
        }

        public async Task<IEnumerable<TypeDTO>> GetAllTypesAsync()
        {
            var Types = await _unitOfWork.GetRepository<ProductType, int>().GetAllAsync();

            return _mapper.Map<IEnumerable<TypeDTO>>(Types);
        }

        public async Task<ProductDTO> GetProductByIdAsync(int Id)
        {
            var Spec = new ProductWithTypeAndBrandSpecification(Id);
            var product =await  _unitOfWork.GetRepository<Product, int>().GetByIdAsync(Spec);
            if (product == null)
            {
                throw new ProductNotFoundException(Id);
            }

            return _mapper.Map<ProductDTO>(product);

        }
    }
}
