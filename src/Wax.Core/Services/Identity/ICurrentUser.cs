using Wax.Core.DependencyInjection;

namespace Wax.Core.Services.Identity
{
    public interface ICurrentUser : IScopedDependency
    {
        string Id { get; }
    }
}