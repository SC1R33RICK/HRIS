using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Repository
{
    public interface IRepositoryQuery<TEntity> where TEntity : EntityBase
    {
        RepositoryQuery<TEntity> Filter(Expression<Func<TEntity, bool>> filter);

        RepositoryQuery<TEntity> OrderBy(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy);

        RepositoryQuery<TEntity> Include(Expression<Func<TEntity, object>> expression);

        IEnumerable<TEntity> GetPage(int page, int pageSize, out int totalCount);

        IQueryable<TEntity> Get();

        IQueryable<TEntity> Select();

        IQueryable<TResult> Select<TResult>(Expression<Func<TEntity, TResult>> selector);

        Task<IEnumerable<TEntity>> GetAsync();

        IQueryable<TEntity> SqlQuery(string query, params object[] parameters);
    }
}