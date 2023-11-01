using System.Linq.Expressions;
using Wax.Core.Domain;
using Wax.Messages;

namespace Wax.Core.Repositories;

public interface IBasicRepository<TEntity> where TEntity : class, IEntity
{
    Task<TEntity> GetByIdAsync<TKey>(TKey id, CancellationToken cancellationToken = default) where TKey : notnull;

    Task<TEntity> FirstOrDefaultAsync(
        Expression<Func<TEntity, bool>> filter = null,
        Expression<Func<TEntity, object>> orderBy = null,
        bool descending = true,
        CancellationToken cancellationToken = default);

    Task<TEntity> SingleOrDefaultAsync(
        Expression<Func<TEntity, bool>> filter = null,
        CancellationToken cancellationToken = default);

    Task<List<TEntity>> ListAsync(
        Expression<Func<TEntity, bool>> filter = null,
        Expression<Func<TEntity, object>> orderBy = null,
        bool descending = true,
        CancellationToken cancellationToken = default);

    Task<TProjection> GetByProjectionAsync<TProjection>(
        Expression<Func<TEntity, TProjection>> selector,
        Expression<Func<TEntity, bool>> filter = null,
        CancellationToken cancellationToken = default);

    Task<List<TProjection>> GetListByProjectionAsync<TProjection>(
        Expression<Func<TEntity, TProjection>> selector,
        Expression<Func<TEntity, bool>> filter = null,
        Expression<Func<TEntity, object>> orderBy = null,
        bool descending = true,
        CancellationToken cancellationToken = default);

    Task<IPaginatedList<TProjection>> GetPaginatedListByProjectionAsync<TProjection>(
        Expression<Func<TEntity, TProjection>> selector,
        Expression<Func<TEntity, bool>> filter = null,
        Expression<Func<TEntity, object>> orderBy = null,
        bool descending = true,
        int pageIndex = 1,
        int pageSize = 15,
        CancellationToken cancellationToken = default);

    Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task InsertRangeAsync(IEnumerable<TEntity> entity, CancellationToken cancellationToken = default);

    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task DeleteRangeAsync(IEnumerable<TEntity> entity, CancellationToken cancellationToken = default);

    IQueryable<TEntity> Table { get; }
}