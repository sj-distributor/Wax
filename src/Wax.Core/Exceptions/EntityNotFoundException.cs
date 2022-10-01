namespace Wax.Core.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException()
        {
        }

        public EntityNotFoundException(Type entityType, object entityId = null) : base(entityId == null
            ? $"Entity not found. Entity type: {entityType.FullName}"
            : $"Entity not found. Entity type: {entityType.FullName}, id: {entityId}")
        {
        }
    }
}