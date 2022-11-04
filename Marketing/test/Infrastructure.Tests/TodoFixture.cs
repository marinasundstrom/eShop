using System.Data.Common;
using System.Xml.Linq;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using NSubstitute;
using YourBrand.Marketing.Application.Services;
using YourBrand.Marketing.Infrastructure.Persistence;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace YourBrand.Marketing.Infrastructure
{
    public class TodoFixture : IDisposable
    {
        private readonly IDomainEventDispatcher fakeDomainEventDispatcher;
        private readonly ICurrentUserService fakeCurrentUserService;
        private readonly IDateTime fakeDateTimeService;
        private SqliteConnection connection = null!;

        public TodoFixture()
        {
            fakeDomainEventDispatcher = Substitute.For<IDomainEventDispatcher>();
            fakeCurrentUserService = Substitute.For<ICurrentUserService>();
            fakeDateTimeService = Substitute.For<IDateTime>();
        }

        public ApplicationDbContext CreateDbContext()
        {
            string dbName = $"testdb";

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseSqlite(GetDbConnection())
               .Options;

            var context = new ApplicationDbContext(options,
                new YourBrand.Marketing.Infrastructure.Persistence.Interceptors.AuditableEntitySaveChangesInterceptor(fakeCurrentUserService, fakeDateTimeService));

            context.Database.EnsureCreated();

            return context;
        }

        private DbConnection GetDbConnection()
        {
            connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            return connection;
        }

        public void Dispose()
        {
            connection.Close();
        }
    }
}