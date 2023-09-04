using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Wax.Core.Data;
using Wax.Core.Domain;
using Wax.Core.Exceptions;

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
        var entity = await _dbContext.Set<TEntity>().FindAsync(id);

        if (entity == null)
        {
            throw new EntityNotFoundException(typeof(TEntity), id);
        }

        return entity;
    }

    public async Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        var entry = await _dbContext.Set<TEntity>().AddAsync(entity, cancellationToken);
        return entry.Entity;
    }

    public Task InsertRangeAsync(IEnumerable<TEntity> entity, CancellationToken cancellationToken = default)
    {
        return _dbContext.Set<TEntity>().AddRangeAsync(entity, cancellationToken);
    }

    public Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        _dbContext.Entry(entity).State = EntityState.Modified;
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

    public Task<List<TEntity>> ListAsync(Expression<Func<TEntity, bool>> predicate = null,
        CancellationToken cancellationToken = default)
    {
        return predicate == null
            ? _dbContext.Set<TEntity>().ToListAsync(cancellationToken)
            : _dbContext.Set<TEntity>().Where(predicate).ToListAsync(cancellationToken);
    }


    public IQueryable<TEntity> Table => _dbContext.Set<TEntity>();
}