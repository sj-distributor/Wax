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

    public async Task<TEntity> GetByIdAsync<TKey>(TKey id)
        where TKey : notnull
    {
        var entity = await _dbContext.Set<TEntity>().FindAsync(new object[] { id }).ConfigureAwait(false);

        if (entity == null)
        {
            throw new EntityNotFoundException(typeof(TEntity), id);
        }

        return entity;
    }

    public async Task<TEntity> InsertAsync(TEntity entity, bool saveNow = false)
    {
        var entry = await _dbContext.Set<TEntity>().AddAsync(entity).ConfigureAwait(false);

        await _dbContext.ChangeEntitiesAsync(saveNow);

        return entry.Entity;
    }

    public async Task InsertRangeAsync(IEnumerable<TEntity> entity, bool saveNow = false)
    {
        await _dbContext.Set<TEntity>().AddRangeAsync(entity).ConfigureAwait(false);

        await _dbContext.ChangeEntitiesAsync(saveNow);
    }

    public Task UpdateAsync(TEntity entity, bool saveNow = false)
    {
        _dbContext.Entry(entity).State = EntityState.Modified;
        return _dbContext.ChangeEntitiesAsync(saveNow);
    }

    public Task UpdateRangeAsync(IEnumerable<TEntity> entities, bool saveNow = false)
    {
        _dbContext.Set<TEntity>().UpdateRange(entities);
        return _dbContext.ChangeEntitiesAsync(saveNow);
    }

    public Task DeleteAsync(TEntity entity, bool saveNow = false)
    {
        _dbContext.Set<TEntity>().Remove(entity);
        return _dbContext.ChangeEntitiesAsync(saveNow);
    }

    public Task DeleteRangeAsync(IEnumerable<TEntity> entity, bool saveNow = false)
    {
        _dbContext.Set<TEntity>().RemoveRange(entity);
        return _dbContext.ChangeEntitiesAsync(saveNow);
    }

    public Task<List<TEntity>> ListAsync(Expression<Func<TEntity, bool>> predicate = null)
    {
        return predicate == null
            ? _dbContext.Set<TEntity>().ToListAsync()
            : _dbContext.Set<TEntity>().Where(predicate).ToListAsync();
    }


    public IQueryable<TEntity> Table => _dbContext.Set<TEntity>();
}