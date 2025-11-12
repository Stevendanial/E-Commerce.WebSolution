using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services.Specifications
{
    public class BaseSpecifications<TEntity, Tkey> : ISpecifications<TEntity, Tkey> where TEntity : BaseEntity<Tkey>
    {
        #region Include
         public ICollection<Expression<Func<TEntity, object>>> IncludeExpressions { get; } = [];    
         protected void AddInclude(Expression<Func<TEntity, object>> includeExp)
        {
            IncludeExpressions.Add(includeExp);
        }
        #endregion

        #region Criteria
         public Expression<Func<TEntity, bool>> Criteria { get; }
        protected BaseSpecifications(Expression<Func<TEntity, bool>> CriteriaExpression)
        {
            Criteria = CriteriaExpression;
        }
        #endregion


        #region sorting
       public Expression<Func<TEntity, object>> OrderBy { get; private set; }

        public Expression<Func<TEntity, object>> OrderByDescending { get; private set; }

        protected void AddOrderBy(Expression<Func<TEntity, object>> orderByExpression)
        {
            OrderBy = orderByExpression;
        }

        protected void AddOrderByDescending(Expression<Func<TEntity, object>> orderByDescendingExpression)
        {
            OrderByDescending = orderByDescendingExpression;
        }
        #endregion


        #region Pagination

        public int Take { get; private set; }

        public int Skip { get; private set; }

        public bool IsPagination { get; private set; }
        protected void ApplyPagination(int Pageskip, int PageIndex)
        {
           
            IsPagination = true;
            Take = Pageskip;
            Skip = (PageIndex-1)* Pageskip;
        }

        #endregion

    }
}
