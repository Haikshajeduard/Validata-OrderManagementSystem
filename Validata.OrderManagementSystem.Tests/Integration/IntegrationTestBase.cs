using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Net.Http;

namespace Validata.OrderManagementSystem.Tests.Integration;

public abstract class IntegrationTestBase
{
    private WebApplicationFactory<Program> _factory;

    [SetUp]
    public void Setup()
    {
        _factory = new WebApplicationFactory<Program>();
    }

    protected HttpClient CreateClient() => _factory.CreateClient();

    protected IServiceScope CreateScope()
    {
        var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
        return scopeFactory.CreateScope();
    }
}
