namespace Wax.Core.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public Type EntityType { get; }
        public object EntityId { get; }

        public EntityNotFoundException()
        {
        }

        public EntityNotFoundException(Type entityType, object entityId = null) : base(entityId == null
            ? $"Entity not found. Entity type: {entityType.FullName}"
            : $"Entity not found. Entity type: {entityType.FullName}, id: {entityId}")
        {
            EntityType = entityType;
            EntityId = entityId;
        }
    }
}
