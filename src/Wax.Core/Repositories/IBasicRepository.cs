using System.Linq.Expressions;
using Wax.Core.Domain;

namespace Wax.Core.Repositories;

public interface IBasicRepository<TEntity> where TEntity : class, IEntity
{
    Task<TEntity> GetByIdAsync<TKey>(TKey id) where TKey : notnull;

    Task<TEntity> InsertAsync(TEntity entity, bool saveNow = false);

    Task InsertRangeAsync(IEnumerable<TEntity> entity, bool saveNow = false);

    Task UpdateAsync(TEntity entity, bool saveNow = false);

    Task UpdateRangeAsync(IEnumerable<TEntity> entities, bool saveNow = false);

    Task DeleteAsync(TEntity entity, bool saveNow = false);

    Task DeleteRangeAsync(IEnumerable<TEntity> entity, bool saveNow = false);

    Task<List<TEntity>> ListAsync(Expression<Func<TEntity, bool>> predicate = null);

    IQueryable<TEntity> Table { get; }
}