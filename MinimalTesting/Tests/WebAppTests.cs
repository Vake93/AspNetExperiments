using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Xunit;

namespace Tests;

public class WebAppTests
{
    [Fact]
    public async Task HelloWorld()
    {
        var application = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddSingleton<IHelloService, MockHelloService>();
                });
            });

        var client = application.CreateClient();

        var response = await client.GetStringAsync("/");

        Assert.Equal("Test Hello", response);
    }

    class MockHelloService : IHelloService
    {
        public string HelloMessage => "Test Hello";
    }
}