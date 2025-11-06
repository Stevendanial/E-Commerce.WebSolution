using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entites;
using E_Commerce.Persistence.Data.DbContexts;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreDbContext _dbContext;
        private readonly Dictionary<Type, object> _repositories = [];

        public UnitOfWork(StoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IGenericRepository<TEntity, Tkey> GetRepository<TEntity, Tkey>() where TEntity : BaseEntity<Tkey>
        {
            var EntityType = typeof(TEntity);

            if (_repositories.TryGetValue(EntityType, out object? repository))
                return (IGenericRepository<TEntity, Tkey>)repository;

            var NewRepo = new GenericRepository<TEntity, Tkey>(_dbContext);

            _repositories[EntityType] = NewRepo;
            return NewRepo;

        }

        public async Task<int> SaveChangeAsync()=> await _dbContext.SaveChangesAsync();

    }
}
