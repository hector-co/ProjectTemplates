using Flurl.Http;
using TplNamespace.WebApi.Tests.Common;
using Xunit;

namespace TplNamespace.WebApi.Tests;

[Collection("TestServer")]
public class TplModuleControllerTest
{
    private readonly FlurlClient _client;

    public TplModuleControllerTest(TestServerFixture factory)
    {
        _client = new FlurlClient(factory.CreateDefaultClient());
        _ = new DatabaseManager(factory.ConnectionString);
    }
}