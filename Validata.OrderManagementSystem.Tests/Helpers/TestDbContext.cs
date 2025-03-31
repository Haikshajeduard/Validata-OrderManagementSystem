using Microsoft.EntityFrameworkCore;
using Validata.OrderManagementSystem.Persistence;

namespace Validata.OrderManagementSystem.Tests.Helpers
{
    public static class TestDbContext
    {
        public static ApplicationDBContext Create()
        {
            var options = new DbContextOptionsBuilder<ApplicationDBContext>()
                .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
                .Options;

            var context = new ApplicationDBContext(options);
            context.Database.EnsureCreated();
            return context;
        }

        public static void Destroy(ApplicationDBContext context)
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}