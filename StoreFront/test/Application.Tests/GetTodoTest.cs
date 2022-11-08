using System.Threading.Tasks;
using NSubstitute;
using YourBrand.StoreFront.Application.Services;
using YourBrand.StoreFront.Application.Todos.Queries;
using YourBrand.StoreFront.Domain;
using YourBrand.StoreFront.Infrastructure.Persistence.Repositories.Mocks;
using Xunit;

namespace YourBrand.StoreFront.Application.Todos.Commands;

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
