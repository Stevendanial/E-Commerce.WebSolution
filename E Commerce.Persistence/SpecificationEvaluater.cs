using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entites;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Persistence
{
    internal static class SpecificationEvalater
    {
        public static IQueryable<TEntity> CreateQuery<TEntity, TKey>(IQueryable<TEntity> EntryPoint,
            ISpecifications<TEntity,TKey> specifications) where TEntity : BaseEntity<TKey>
        {
            var Query = EntryPoint;
            if (specifications is not null)
            {
                if (specifications is not null )
                {
                    Query = Query.Where(specifications.Criteria);
                
                }
                    
                if (specifications.IncludeExpressions is not null&& specifications.IncludeExpressions.Any())
                {
                    //foreach (var includeExp in specifications.IncludeExpressions)
                    //{
                    //    Query = Query.Include(includeExp);
                    //}
                    Query = specifications.IncludeExpressions.Aggregate(Query
                        ,(CurrentQuery, includeExp) => CurrentQuery.Include(includeExp));

                }

                if (specifications.OrderBy is not null)
                {
                    Query = Query.OrderBy(specifications.OrderBy);
                }
                if (specifications.OrderByDescending is not null)
                {
                    Query = Query.OrderByDescending(specifications.OrderByDescending);
                }

                if (specifications.IsPagination)
                {
                    Query = Query.Skip(specifications.Skip).Take(specifications.Take);
                }

            }
          return Query;
        }
    }
}
