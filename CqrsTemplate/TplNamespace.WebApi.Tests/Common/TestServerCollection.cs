using Xunit;

namespace TplNamespace.WebApi.Tests.Common;

[CollectionDefinition("TestServer")]
public class TestServerCollection : ICollectionFixture<TestServerFixture>
{
}
