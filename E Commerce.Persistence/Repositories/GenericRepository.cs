using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entites;
using E_Commerce.Persistence.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Persistence.Repositories
{
    public class GenericRepository<TEntity, Tkey> : IGenericRepository<TEntity, Tkey> where TEntity : BaseEntity<Tkey>
    {
        private readonly StoreDbContext _dbContext;

        public GenericRepository(StoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddAsync(TEntity entity)
        {
           await _dbContext.Set<TEntity>().AddAsync(entity);
           
        }

        public void Remove(TEntity entity)=> _dbContext.Set<TEntity>().Remove(entity);


        public async Task<IEnumerable<TEntity>> GetAllAsync()=>await _dbContext.Set<TEntity>().ToListAsync();


        public async Task<TEntity?> GetByIdAsync(Tkey id) =>await _dbContext.Set<TEntity>().FindAsync(id);
      

        public void Update(TEntity entity)=> _dbContext.Set<TEntity>().Update(entity);

        public async Task<IEnumerable<TEntity>> GetAllAsync(ISpecifications<TEntity, Tkey> specifications)
        {
            return await SpecificationEvalater.CreateQuery(_dbContext.Set<TEntity>(), specifications).ToListAsync();
        }

        public async Task<TEntity?> GetByIdAsync(ISpecifications<TEntity, Tkey> specifications)
        {
            return await SpecificationEvalater.CreateQuery(_dbContext.Set<TEntity>(), specifications).FirstOrDefaultAsync();

        }

        public async Task<int> CountAsync(ISpecifications<TEntity, Tkey> specifications)
        {
            return await SpecificationEvalater.CreateQuery(_dbContext.Set<TEntity>(), specifications).CountAsync();
        }
    }
}
