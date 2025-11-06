using E_Commerce.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Formats.Tar;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Contracts
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangeAsync();

        IGenericRepository<TEntity,Tkey> GetRepository<TEntity,Tkey>()where TEntity:BaseEntity<Tkey>; 
    }
}
