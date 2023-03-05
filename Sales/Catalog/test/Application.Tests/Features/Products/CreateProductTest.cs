using System.Linq;
using System.Threading.Tasks;
using NSubstitute;
using YourBrand.Catalog.Services;
using YourBrand.Catalog.Domain.Events;
using Xunit;
using Microsoft.EntityFrameworkCore;
using System;
using YourBrand.Catalog.Infrastructure.Persistence;
using YourBrand.Catalog.Infrastructure.Persistence.Interceptors;
using Microsoft.Data.Sqlite;

namespace YourBrand.Catalog.Features.Products;

public class CreateProductTest
{
    [Fact]
    public async Task CreateProduct_ProductCreated()
    {
        // Arrange

        var fakeCurrentUserService = Substitute.For<ICurrentUserService>();
        fakeCurrentUserService.UserId.Returns("foo");

        var fakeDateTimeService = Substitute.For<IDateTime>();
        fakeDateTimeService.Now.Returns(DateTime.UtcNow);

        var fakeDomainEventDispatcher = Substitute.For<IDomainEventDispatcher>();
        var fakeTodoNotificationService = Substitute.For<ITodoNotificationService>();

        using (var connection = new SqliteConnection("Data Source=:memory:"))
        {
            connection.Open();

            var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .AddInterceptors(new AuditableEntitySaveChangesInterceptor(fakeCurrentUserService, fakeDateTimeService), new FakeOutboxSaveChangesInterceptor(fakeDomainEventDispatcher))
                .UseSqlite(connection)
                .Options;

            using var unitOfWork = new ApplicationDbContext(dbContextOptions);

            await unitOfWork.Database.EnsureCreatedAsync();

            unitOfWork.Users.Add(new Domain.Entities.User("foo", "Test Tesston", "test@foo.com"));

            await unitOfWork.SaveChangesAsync();

            var todoRepository = new TodoRepository(unitOfWork);

            var commandHandler = new CreateProduct.Handler(todoRepository, unitOfWork, fakeDomainEventDispatcher);

            var todos = todoRepository.GetAll();

            var initialTodoCount = todos.Count();

            string title = "test";

            // Act

            var createTodoCommand = new CreateProduct("Wheelbarrow", "wheelbarrow", false, "A lot of capacity", null, null, null, ProductVisibility.Listed);

            var result = await commandHandler.Handle(createTodoCommand, default);

            // Assert

            Assert.True(result.IsSuccess);

            var todo = result.GetValue();

            todos = todoRepository.GetAll();

            var newTodoCount = todos.Count();

            newTodoCount.Should().BeGreaterThan(initialTodoCount);

            // Has Domain Event been published ?

            await fakeDomainEventDispatcher
                .Received(1)
                .Dispatch(Arg.Is<TodoCreated>(d => d.TodoId == todo.Id));
        }
    }
}
