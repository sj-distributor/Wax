using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Wax.Core.Data;
using Wax.Core.Domain;
using Wax.Core.Exceptions;
using Wax.Messages;

namespace Wax.Core.Repositories;

public class BasicRepository<TEntity> : IBasicRepository<TEntity> where TEntity : class, IEntity
{
    private readonly ApplicationDbContext _dbContext;

    public BasicRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<TEntity> GetByIdAsync<TKey>(TKey id, CancellationToken cancellationToken = default)
        where TKey : notnull
    {
        var entity = await _dbContext.Set<TEntity>().FindAsync(new object[] { id }, cancellationToken)
            .ConfigureAwait(false);

        if (entity == null)
        {
            throw new EntityNotFoundException(typeof(TEntity), id);
        }

        return entity;
    }

    public Task<TEntity> FirstOrDefaultAsync(
        Expression<Func<TEntity, bool>> filter = null,
        Expression<Func<TEntity, object>> orderBy = null,
        bool descending = true,
        CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Set<TEntity>().AsQueryable();

        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (orderBy != null)
        {
            query = descending
                ? query.OrderByDescending(orderBy)
                : query.OrderBy(orderBy);
        }

        return query.FirstOrDefaultAsync(cancellationToken);
    }

    public Task<TEntity> SingleOrDefaultAsync(
        Expression<Func<TEntity, bool>> filter = null,
        CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Set<TEntity>().AsQueryable();

        if (filter != null)
        {
            query = query.Where(filter);
        }

        return query.SingleOrDefaultAsync(cancellationToken);
    }

    public Task<List<TEntity>> ListAsync(
        Expression<Func<TEntity, bool>> filter = null,
        Expression<Func<TEntity, object>> orderBy = null,
        bool descending = true,
        CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Set<TEntity>().AsNoTracking().AsQueryable();

        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (orderBy != null)
        {
            query = descending
                ? query.OrderByDescending(orderBy)
                : query.OrderBy(orderBy);
        }

        return query.ToListAsync(cancellationToken);
    }

    public Task<TProjection> GetByProjectionAsync<TProjection>(
        Expression<Func<TEntity, TProjection>> selector,
        Expression<Func<TEntity, bool>> filter = null,
        CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Set<TEntity>().AsNoTracking().AsQueryable();

        if (filter != null)
        {
            query = query.Where(filter);
        }

        return query.Select(selector).SingleOrDefaultAsync(cancellationToken);
    }

    public Task<List<TProjection>> GetListByProjectionAsync<TProjection>(
        Expression<Func<TEntity, TProjection>> selector,
        Expression<Func<TEntity, bool>> filter = null,
        Expression<Func<TEntity, object>> orderBy = null,
        bool descending = true,
        CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Set<TEntity>().AsNoTracking().AsQueryable();

        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (orderBy != null)
        {
            query = descending
                ? query.OrderByDescending(orderBy)
                : query.OrderBy(orderBy);
        }

        return query.Select(selector).ToListAsync(cancellationToken);
    }

    public Task<IPaginatedList<TProjection>> GetPaginatedListByProjectionAsync<TProjection>(
        Expression<Func<TEntity, TProjection>> selector,
        Expression<Func<TEntity, bool>> filter = null,
        Expression<Func<TEntity, object>> orderBy = null,
        bool descending = true,
        int pageIndex = 1,
        int pageSize = 15,
        CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Set<TEntity>().AsNoTracking().AsQueryable();

        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (orderBy != null)
        {
            query = descending
                ? query.OrderByDescending(orderBy)
                : query.OrderBy(orderBy);
        }

        return query.Select(selector).ToPaginatedListAsync(pageIndex, pageSize, cancellationToken);
    }

    public async Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        var entry = await _dbContext.Set<TEntity>().AddAsync(entity, cancellationToken).ConfigureAwait(false);

        return entry.Entity;
    }

    public async Task InsertRangeAsync(IEnumerable<TEntity> entity, CancellationToken cancellationToken = default)
    {
        await _dbContext.Set<TEntity>().AddRangeAsync(entity, cancellationToken).ConfigureAwait(false);
    }

    public Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public Task UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        _dbContext.Set<TEntity>().UpdateRange(entities);

        return Task.CompletedTask;
    }

    public Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        _dbContext.Set<TEntity>().Remove(entity);

        return Task.CompletedTask;
    }

    public Task DeleteRangeAsync(IEnumerable<TEntity> entity, CancellationToken cancellationToken = default)
    {
        _dbContext.Set<TEntity>().RemoveRange(entity);

        return Task.CompletedTask;
    }

    public IQueryable<TEntity> Table => _dbContext.Set<TEntity>();
}