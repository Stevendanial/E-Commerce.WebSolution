using E_Commerce.Service.Abstraction;
using E_Commerce.Shared.CommonResult;
using E_Commerce.Shared.DTOs.IdentityDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace E_Commerce.Services
{
    public class CacheService : ICacheService
    {
        private readonly ICacheService _cacheRepository;

        public CacheService(ICacheService cacheRepository )
        {
            _cacheRepository = cacheRepository;
        }

        

        public async Task<string?> GetAsync(string Cachekey)
        {
            return await _cacheRepository.GetAsync( Cachekey );
        }

       

        public async Task SetAsync(string Cachekey, object CacheValue, TimeSpan TimeToLive)
        {
            var Value =JsonSerializer.Serialize( CacheValue,new JsonSerializerOptions() 
            {
                PropertyNamingPolicy=JsonNamingPolicy.CamelCase
            
            } );
            await _cacheRepository.SetAsync(Cachekey, Value, TimeToLive);
        }
    }
}
