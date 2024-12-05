using System.Security.Claims;

namespace Wax.Api;

public class UserContext : IUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    private string _cachedUserId;

    public UserContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetCurrentUserId()
    {
        if (!string.IsNullOrEmpty(_cachedUserId))
        {
            return _cachedUserId;
        }

        SetCurrentUserId();

        return _cachedUserId;
    }

    public void SetCurrentUserId(string userId = null)
    {
        if (userId == null)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user is null)
            {
                throw new InvalidOperationException("HttpContext must be present to inspect auth info.");
            }

            var userIdClaim = user.FindFirst("sub") ?? user.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim is null)
            {
                throw new InvalidOperationException("User Id was not present in the request token.");
            }

            _cachedUserId = userIdClaim.Value;
        }
        else
        {
            _cachedUserId = userId;
        }
    }
}