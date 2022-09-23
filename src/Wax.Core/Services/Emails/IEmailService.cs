using Wax.Core.DependencyInjection;

namespace Wax.Core.Services.Emails
{
    public interface IEmailService : IScopedDependency
    {
        Task SendAsync(string from, string to, string subject, string body);
    }
}