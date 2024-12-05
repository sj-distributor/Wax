namespace Wax.IntegrationTests;

public class IntegrationTestUser : IUserContext
{
    public string GetCurrentUserId()
    {
        return "__wax_tester";
    }

    public void SetCurrentUserId(string userId = null)
    {
    }
}