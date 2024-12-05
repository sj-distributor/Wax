namespace Wax.Core.Extensions;

public static class MappingExtensions
{
    public static TModel ToModel<TModel>(this IEntity entity) where TModel : IBaseModel
    {
        ArgumentNullException.ThrowIfNull(entity);

        return entity.Map<TModel>();
    }

    public static TModel ToModel<TEntity, TModel>(this TEntity entity, TModel model)
        where TEntity : IEntity
        where TModel : IBaseModel
    {
        ArgumentNullException.ThrowIfNull(entity);
        ArgumentNullException.ThrowIfNull(model);

        return entity.MapTo(model);
    }

    public static TEntity ToEntity<TEntity>(this IBaseModel model) where TEntity : IEntity
    {
        ArgumentNullException.ThrowIfNull(model);

        return model.Map<TEntity>();
    }
    
    public static TEntity ToEntity<TEntity, TModel>(this TModel model, TEntity entity)
        where TEntity : IEntity where TModel : IBaseModel
    {
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(entity);

        return model.MapTo(entity);
    }
    
    private static TDestination Map<TDestination>(this object source)
    {
        return AutoMapperConfiguration.Mapper.Map<TDestination>(source);
    }

    private static TDestination MapTo<TSource, TDestination>(this TSource source, TDestination destination)
    {
        return AutoMapperConfiguration.Mapper.Map(source, destination);
    }
}