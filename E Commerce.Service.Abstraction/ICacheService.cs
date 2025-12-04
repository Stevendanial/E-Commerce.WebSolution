using E_Commerce.Shared.CommonResult;
using E_Commerce.Shared.DTOs.IdentityDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Service.Abstraction
{
    public interface ICacheService
    {
        Task<string?> GetAsync(string Cachekey);
        Task SetAsync(string Cachekey, object CacheValue, TimeSpan TimeToLive);

        
    }
}
