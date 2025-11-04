using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entites;
using E_Commerce.Domain.Entites.Product_Module;
using E_Commerce.Persistence.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace E_Commerce.Persistence.Data.DataSeeding
{
    public class DataInitializer : IDataInitializer
    {
        private readonly StoreDbContext _dbContext;

        public DataInitializer(StoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Initialize()
        {
            try
            {
                var HasProducts =_dbContext.Products.Any();
                var HasBrands = _dbContext.ProductBrands.Any();
                var HasTypes = _dbContext.ProductTypes.Any();

                if (HasProducts && HasBrands && HasTypes) return;

                if (!HasTypes)
                {
                    SeedDataFromJSON<ProductType, int>("types.json", _dbContext.ProductTypes);
                }

                if (!HasBrands)
                {
                    SeedDataFromJSON<ProductBrand, int>("brands.json", _dbContext.ProductBrands);
                }

                _dbContext.SaveChanges();
                if (!HasProducts)
                {
                    SeedDataFromJSON<Product, int>("products.json", _dbContext.Products);
                }



            }
            catch (Exception ex)
            {
                Console.WriteLine($"Data Seed is Faild :{ex}");

            }
        }
        private void SeedDataFromJSON<T, TKey>(string FileName, DbSet<T> dbset) where T : BaseEntity<TKey> {

            //D:\sevo ass\Backend.Net\ASP.Net API\E Commerce.WebSolution\E Commerce.Persistence\Data\DataSeeding\JSONFiles\brands.json

            var FilePath= @"..\E Commerce.Persistence\Data\DataSeeding\JSONFiles\"+ FileName;
            if (!File.Exists(FilePath)) return;

            try
            {
                using var DataStrams=File.OpenRead(FilePath);
                var Data = JsonSerializer.Deserialize<List<T>>(DataStrams, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true


                });
                if (Data != null)
                { 
                dbset.AddRange(Data);
                }



            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error While Reading JSON file :{ ex }");

            }
            }
    }
}
