using IConfigurationProvider = AutoMapper.IConfigurationProvider;

namespace Wax.Core.Mappings;

public static class AutoMapperConfiguration
{
    public static IMapper Mapper { get; private set; }
    public static IConfigurationProvider MapperConfiguration { get; private set; }

    public static void Init(IConfigurationProvider config)
    {
        MapperConfiguration = config;
        Mapper = config.CreateMapper();
    }
}