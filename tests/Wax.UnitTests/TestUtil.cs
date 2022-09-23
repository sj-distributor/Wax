using AutoMapper;

namespace Wax.UnitTests;

public class TestUtil
{
    public static IMapper CreateMapper(params Profile[] profiles)
    {
        var mapperConfiguration = new MapperConfiguration(x =>
        {
            foreach (var profile in profiles)
            {
                x.AddProfile(profile);
            }
        });

        return mapperConfiguration.CreateMapper();
    }
}