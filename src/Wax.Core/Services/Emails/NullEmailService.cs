namespace Wax.Core.Services.Emails
{
    public class NullEmailService : IEmailService
    {
        public Task SendAsync(string from, string to, string subject, string body)
        {
            return Task.CompletedTask;
        }
    }
}
