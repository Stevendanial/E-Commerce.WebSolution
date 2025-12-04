using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entites;
using E_Commerce.Domain.Entites.OrderModule;
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
        public async Task InitializeAsync()
        {
            try
            {
                var HasProducts =await _dbContext.Products.AnyAsync();
                var HasBrands =await _dbContext.ProductBrands.AnyAsync();
                var HasTypes = await _dbContext.ProductTypes.AnyAsync();
                var HasDelivaryMethod = await _dbContext.Set<DeliveryMethod>().AnyAsync();
                if (HasProducts && HasBrands && HasTypes&& HasDelivaryMethod) return;

                if (!HasTypes)
                {
                  await SeedDataFromJSONAsync<ProductType, int>("types.json", _dbContext.ProductTypes);
                }

                if (!HasBrands)
                {
                   await SeedDataFromJSONAsync<ProductBrand, int>("brands.json", _dbContext.ProductBrands);
                }

              await  _dbContext.SaveChangesAsync();
                if (!HasProducts)
                {
                   await SeedDataFromJSONAsync<Product, int>("products.json", _dbContext.Products);
                }

                if (!HasDelivaryMethod)
                {
                   await SeedDataFromJSONAsync<DeliveryMethod, int>("delivery.json", _dbContext.Set<DeliveryMethod>());
                }

                await _dbContext.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Data Seed is Faild :{ex}");

            }
        }
        private async Task SeedDataFromJSONAsync<T, TKey>(string FileName, DbSet<T> dbset) where T : BaseEntity<TKey> {

            //D:\sevo ass\Backend.Net\ASP.Net API\E Commerce.WebSolution\E Commerce.Persistence\Data\DataSeeding\JSONFiles\brands.json

            var FilePath= @"..\E Commerce.Persistence\Data\DataSeeding\JSONFiles\"+ FileName;
            if (!File.Exists(FilePath)) return;

            try
            {
                using var DataStrams= File.OpenRead(FilePath);
                var Data =await  JsonSerializer.DeserializeAsync<List<T>>(DataStrams, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true


                });
                if (Data != null)
                { 
                await dbset.AddRangeAsync(Data);
                }



            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error While Reading JSON file :{ ex }");

            }
            }
    }
}
