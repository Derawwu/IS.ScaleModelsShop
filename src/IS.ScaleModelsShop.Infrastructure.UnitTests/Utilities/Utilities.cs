using IS.ScaleModelsShop.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace IS.ScaleModelsShop.Infrastructure.UnitTests.Utilities
{
    public static class Utilities
    {
        public static DbContextOptions<T> TestDbContextOptions<T>() where T : AppDbContext
        {
            // Create a new service provider to create a new in-memory database.
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            // Create a new options instance using an in-memory database and
            // IServiceProvider that the context should resolve all of its
            // services from.
            var builder = new DbContextOptionsBuilder<T>()
                .UseInMemoryDatabase("InMemoryDb")
                .UseInternalServiceProvider(serviceProvider)
                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));

            return builder.Options;
        }
    }
}