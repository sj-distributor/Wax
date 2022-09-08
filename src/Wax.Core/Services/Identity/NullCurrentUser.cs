namespace Wax.Core.Services.Identity
{
    public class NullCurrentUser : ICurrentUser
    {
        public string Id => "__wax_user";
    }
}
