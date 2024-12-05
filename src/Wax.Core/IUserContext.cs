namespace Wax.Core;

public interface IUserContext
{
    string GetCurrentUserId();
    void SetCurrentUserId(string userId = null);
}