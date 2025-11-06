using AutoMapper;
using E_Commerce.Domain.Entites.Product_Module;
using E_Commerce.Shared.DTOs.ProductDTOs;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace E_Commerce.Services.MappingProfiles
{
    

    public class ProductPictureUrlResolver : IValueResolver<Product, ProductDTO, string>
    {
        private readonly IConfiguration _configuration;

        public ProductPictureUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(Product source, ProductDTO destination, string destMember, ResolutionContext context)
        {
            if (string.IsNullOrEmpty(source.PicturUrl))
                return string.Empty;

            if(source.PicturUrl.StartsWith("http"))
                return source.PicturUrl;

            var BaseUrl = _configuration.GetSection("URLs")["BaseURL"];

            if (string.IsNullOrEmpty(BaseUrl))
             return string.Empty;

            var  PicUrl=$"{BaseUrl}/{source.PicturUrl}";

            return PicUrl;
        }
    }
}
