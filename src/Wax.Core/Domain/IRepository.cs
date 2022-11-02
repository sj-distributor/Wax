namespace Wax.Core.Domain;

public interface IRepository<TEntity, in TKey> where TEntity : class, IEntity<TKey>
{
    Task<TEntity> GetByIdAsync(TKey id);
    Task<TEntity> InsertAsync(TEntity entity);
    Task<TEntity> UpdateAsync(TEntity entity);
    Task DeleteAsync(TEntity entity);
}