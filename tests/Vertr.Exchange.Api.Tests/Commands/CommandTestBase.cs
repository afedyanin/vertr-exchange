using Microsoft.Extensions.DependencyInjection;
using Vertr.Exchange.Api.Tests.Stubs;

namespace Vertr.Exchange.Api.Tests.Commands;
public abstract class CommandTestBase
{
    protected IServiceProvider ServiceProvider { get; private set; }

    protected IExchangeApi Api { get; private set; }

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        ServiceProvider = ServiceProviderStub.BuildServiceProvider();
    }

    [SetUp]
    public void Setup()
    {
        Api = ServiceProvider.GetRequiredService<IExchangeApi>();
    }

    [TearDown]
    public void TearDown()
    {
        Api?.Dispose();
    }
}
