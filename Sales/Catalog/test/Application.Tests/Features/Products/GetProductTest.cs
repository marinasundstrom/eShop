using System.Threading.Tasks;
using NSubstitute;
using YourBrand.Catalog.Services;
using YourBrand.Catalog.Domain;
using Xunit;
using Microsoft.EntityFrameworkCore;
using YourBrand.Catalog.Infrastructure.Persistence;
using Microsoft.Data.Sqlite;

namespace YourBrand.Catalog.Features.Products;

public class GetProductTest
{
    [Fact]
    public async Task GetProduct_ProductNotFound()
    {
        /// Arrange

        var fakeDomainEventDispatcher = Substitute.For<IDomainEventDispatcher>();

        using (var connection = new SqliteConnection("Data Source=:memory:"))
        {
            connection.Open();

            var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connection)
                .Options;

            using var unitOfWork = new ApplicationDbContext(dbContextOptions);

            await unitOfWork.Database.EnsureCreatedAsync();

            var todoRepository = new TodoRepository(unitOfWork);

            var commandHandler = new GetProduct.Handler(todoRepository);

            int nonExistentTodoId = 9999;

            // Act

            var getTodoByIdCommand = new GetProduct("");

            var result = await commandHandler.Handle(getTodoByIdCommand, default);

            // Assert

            Assert.True(result.HasError(Domain.Errors.Todos.TodoNotFound));
        }
    }
}
