using System.Threading.Tasks;
using NSubstitute;
using YourBrand.Analytics.Application.Services;
using YourBrand.Analytics.Application.Todos.Queries;
using YourBrand.Analytics.Domain;
using YourBrand.Analytics.Infrastructure.Persistence.Repositories.Mocks;
using Xunit;

namespace YourBrand.Analytics.Application.Todos.Commands;

public class GetTodoTest
{
    [Fact]
    public async Task GetTodo_TodoNotFound()
    {
        // Arrange

        var fakeDomainEventDispatcher = Substitute.For<IDomainEventDispatcher>();

        // TODO: Fix with EF Core Sqlite provider
        var unitOfWork = new MockUnitOfWork(fakeDomainEventDispatcher);
        var todoRepository = new MockTodoRepository(unitOfWork);
        var commandHandler = new GetTodoById.Handler(todoRepository);

        int nonExistentTodoId = 9999;

        // Act

        var getTodoByIdCommand = new GetTodoById(nonExistentTodoId);

        var result = await commandHandler.Handle(getTodoByIdCommand, default);

        // Assert

        Assert.True(result.HasError(Errors.Todos.TodoNotFound));
    }
}
