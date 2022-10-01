using Wax.Core.Services.Identity;

namespace Wax.Api.Authentication;

public class CurrentUser : ICurrentUser
{
    public string Id => "__wax_user_id";
}