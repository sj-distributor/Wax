using System.Linq.Expressions;
using Wax.Core.Domain;

namespace Wax.Core.Repositories;

public interface IBasicRepository<TEntity> where TEntity : class, IEntity
{
    Task<TEntity> GetByIdAsync<TKey>(TKey id, CancellationToken cancellationToken = default) where TKey : notnull;

    Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task<IEnumerable<TEntity>> InsertRangeAsync(IEnumerable<TEntity> entity,
        CancellationToken cancellationToken = default);

    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);

    IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> predicate = null);
}