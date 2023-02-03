using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Wax.Core.Data;
using Wax.Core.Domain;
using Wax.Core.Exceptions;

namespace Wax.Core.Repositories;

public class EfCoreBasicRepository<TEntity> : IBasicRepository<TEntity> where TEntity : class, IEntity
{
    private readonly ApplicationDbContext _dbContext;

    public EfCoreBasicRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<TEntity> GetByIdAsync<TKey>(TKey id, CancellationToken cancellationToken = default)
        where TKey : notnull
    {
        var entity = await _dbContext.Set<TEntity>()
            .FindAsync(new object[] { id }, cancellationToken).ConfigureAwait(false);

        if (entity == null)
        {
            throw new EntityNotFoundException(typeof(TEntity), id);
        }

        return entity;
    }

    public async Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await _dbContext.Set<TEntity>().AddAsync(entity, cancellationToken).ConfigureAwait(false);
        return entity;
    }

    public async Task<IEnumerable<TEntity>> InsertRangeAsync(IEnumerable<TEntity> entity,
        CancellationToken cancellationToken = default)
    {
        await _dbContext.Set<TEntity>().AddRangeAsync(entity, cancellationToken).ConfigureAwait(false);
        return entity;
    }

    public Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        _dbContext.Entry(entity).State = EntityState.Modified;
        return Task.CompletedTask;
    }

    public Task UpdateRangeAsync(IEnumerable<TEntity> entities,
        CancellationToken cancellationToken = default)
    {
        _dbContext.Set<TEntity>().UpdateRange(entities);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        _dbContext.Set<TEntity>().Remove(entity);
        return Task.CompletedTask;
    }

    public IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> predicate = null)
    {
        return predicate != null ? _dbContext.Set<TEntity>().Where(predicate) : _dbContext.Set<TEntity>();
    }
}