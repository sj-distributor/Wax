namespace Wax.Infrastructure.Settings;

public class SettingModule : Module
{
    private readonly IConfiguration _configuration;
    private readonly Assembly[] _assemblies;

    public SettingModule(IConfiguration configuration, params Assembly[] assemblies)
    {
        _configuration = configuration;
        _assemblies = assemblies;
    }

    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterInstance(_configuration)
            .As<IConfiguration>()
            .SingleInstance();

        var settingTypes = _assemblies.SelectMany(a => a.GetTypes())
            .Where(t => t.IsClass && typeof(IConfigurationSetting).IsAssignableFrom(t))
            .ToArray();

        builder.RegisterTypes(settingTypes).AsSelf().SingleInstance();
    }
}