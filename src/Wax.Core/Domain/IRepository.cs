using System.Linq.Expressions;

namespace Wax.Core.Domain;

public interface IRepository<TEntity> where TEntity : class, IEntity
{
    Task<TEntity> GetByIdAsync<TKey>(TKey id, CancellationToken cancellationToken = default) where TKey : notnull;

    Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task<IEnumerable<TEntity>> InsertRangeAsync(IEnumerable<TEntity> entity,
        CancellationToken cancellationToken = default);

    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);

    IQueryable<TEntity> Query { get; }
}