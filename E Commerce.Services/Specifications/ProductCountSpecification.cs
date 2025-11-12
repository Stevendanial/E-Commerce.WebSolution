using E_Commerce.Domain.Entites.Product_Module;
using E_Commerce.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services.Specifications
{
    internal class ProductCountSpecification : BaseSpecifications< Product, int>
    {
        public ProductCountSpecification(ProductQueryParams queryParams):base(ProductSpecificationHelper.GetProductCriteria(queryParams))
        {


            
        }
    }
}
