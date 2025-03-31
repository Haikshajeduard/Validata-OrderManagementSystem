using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Net.Http;
using Validata.OrderManagementSystem.Persistence;

namespace Validata.OrderManagementSystem.Tests.Integration
{
    public abstract class IntegrationTestBase
    {
        protected WebApplicationFactory<Program> Factory;
        protected ApplicationDBContext DbContext;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            Factory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        var descriptor = services.SingleOrDefault(
                            d => d.ServiceType == typeof(DbContextOptions<ApplicationDBContext>));

                        if (descriptor != null)
                        {
                            services.Remove(descriptor);
                        }

                        services.AddDbContext<ApplicationDBContext>(options =>
                        {
                            options.UseInMemoryDatabase("TestDb");
                        });
                    });
                });

            DbContext = Factory.Services.CreateScope()
                .ServiceProvider.GetRequiredService<ApplicationDBContext>();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            DbContext.Database.EnsureDeleted();
            Factory.Dispose();
        }

        [SetUp]
        public void Setup()
        {
            // Clear database before each test
            DbContext.Database.EnsureCreated();
        }

        [TearDown]
        public void TearDown()
        {
            // Clean up after each test
            var tables = DbContext.Model.GetEntityTypes()
                .Select(t => t.GetTableName())
                .Distinct();

            foreach (var table in tables)
            {
                if (table != null)
                {
                    DbContext.Database.ExecuteSqlRaw($"DELETE FROM {table}");
                }
            }
        }

        protected HttpClient CreateClient() => Factory.CreateClient();
    }
}
