using LumeLaht_RoomApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumeLaht_RoomApi.Tests.Helpers
{
    public abstract class RepositoryTestBase : IDisposable
    {
        protected readonly AppDbContext context;

        protected RepositoryTestBase()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .EnableSensitiveDataLogging() // For debug
                .Options;

            context = new AppDbContext(options);
        }

        public virtual void Dispose()
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
