using Xunit;

namespace Wax.IntegrationTests;

[CollectionDefinition("Sequential")]
public class IntegrationCollection : ICollectionFixture<IntegrationFixture>
{
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}