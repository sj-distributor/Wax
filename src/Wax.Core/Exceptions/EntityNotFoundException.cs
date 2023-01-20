namespace Wax.Core.Exceptions
{
    public class EntityNotFoundException : BusinessException
    {
        public EntityNotFoundException(Type entityType, object entityId = null) : base(
            Messages.Enums.ErrorCode.NotFound, entityId == null
                ? $"Entity not found. Entity type: {entityType.FullName}"
                : $"Entity not found. Entity type: {entityType.FullName}, id: {entityId}")
        {
        }
    }
}