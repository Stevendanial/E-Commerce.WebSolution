
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entites.IdentityModule;
using E_Commerce.Persistence.Data.DataSeeding;
using E_Commerce.Persistence.Data.DbContexts;
using E_Commerce.Persistence.IdentityData.DataSeed;
using E_Commerce.Persistence.IdentityData.DbContext;
using E_Commerce.Persistence.Repositories;
using E_Commerce.Service.Abstraction;
using E_Commerce.Services;
using E_Commerce.Services.MappingProfiles;
using E_Commerce.Web.CustomMiddleWares;
using E_Commerce.Web.Extenstion;
using E_Commerce.Web.Factories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System.Threading.Tasks;

namespace E_Commerce.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            #region Add services to the container
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddKeyedScoped<IDataInitializer, DataInitializer>("Default");
            builder.Services.AddKeyedScoped<IDataInitializer, IdentityDataIntializer>("Identity");

            builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();
            builder.Services.AddDbContext<StoreDbContext>(Option =>
            {
                Option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

            });
            //builder.Services.AddAutoMapper(x => x.AddProfile< ProductProfile>());
            builder.Services.AddAutoMapper(typeof(ServiceAssemplyReference).Assembly);

            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddTransient<ProductPictureUrlResolver>();
            builder.Services.AddSingleton<IConnectionMultiplexer>(
                sp => {

                    return ConnectionMultiplexer.Connect(
                        builder.Configuration.GetConnectionString("RedisConnection")!); 
                });
            builder.Services.AddScoped<IBasketRepository, BasketRepository>();
            builder.Services.AddScoped<IBasketService, BasketService>();
            builder.Services.AddScoped<ICacheService, CacheService>();
            //  builder.Services.AddScoped<ICacheService,CacheService>();
            builder.Services.AddScoped<Service.Abstraction.IAuthenticationService, Services.AuthenticationService>();

            builder.Services.Configure<ApiBehaviorOptions>(
                options=>
                {
                    options.InvalidModelStateResponseFactory = ApiResponseFactory.GenerateApiValidationResponse;

                });
            builder.Services.AddMemoryCache();


            builder.Services.AddDbContext<StoreIdentityDbContext>(options=>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
            });

            builder.Services.AddIdentityCore<ApplicationUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<StoreIdentityDbContext>();

           
            #endregion

            var app = builder.Build();

            #region DataSeeding-Apply Migration
           await app.MigrateIdentityDatabaseAsync();
            await app.MigrateDatabaseAsync();
           await app.SeedDatabaseAsync();
            await app.SeedIdentityDatabaseAsync();
            

            #endregion



            app.UseMiddleware<ExceptionHandlerMiddleWare>();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            

            app.UseHttpsRedirection();
            app.UseStaticFiles();   

            app.UseAuthorization();

            app.MapControllers();

           await app.RunAsync();
        }
    }
}
