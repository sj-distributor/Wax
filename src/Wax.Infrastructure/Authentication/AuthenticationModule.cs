namespace Wax.Infrastructure.Authentication;

public class AuthenticationModule : Module
{
    private readonly IUserContext _userContext;

    public AuthenticationModule(IUserContext userContext)
    {
        _userContext = userContext;
    }

    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterInstance(_userContext);
    }
}