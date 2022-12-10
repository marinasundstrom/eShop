using Bunit;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;
using NSubstitute;
using YourBrand.Catalog;
using YourBrand.Catalog.Pages;

namespace YourBrand.Catalog.Tests;

public class TodosPageTest
{
    [Fact]
    public void ProductsShouldLoadOnInitializedSuccessful()
    {
        // Arrange
        using var ctx = new TestContext();
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;

        ctx.Services.AddMudServices();

        ctx.Services.AddLocalization();

        var fakeAccessTokenProvider = Substitute.For<YourBrand.Catalog.Services.IAccessTokenProvider>();

        ctx.Services.AddSingleton(fakeAccessTokenProvider);

        var fakeTodosClient = Substitute.For<ITodosClient>();
        fakeTodosClient.GetTodosAsync(Arg.Any<TodoStatusDto>(), null, null, null, null, default)
            .ReturnsForAnyArgs(t => new ItemsResultOfTodoDto()
            {
                Products = new[]
                {
                    new TodoDto
                    {
                        Id = 1,
                        Title = "Product 1",
                        Description = "Description",
                        Status = TodoStatusDto.InProgress,
                        Created = DateTimeOffset.Now.AddMinutes(-3)
                    },
                    new TodoDto
                    {
                        Id = 2,
                        Title = "Product 2",
                        Description = "Description",
                        Status = TodoStatusDto.InProgress,
                        Created = DateTimeOffset.Now.AddMinutes(-1)
                    },
                    new TodoDto
                    {
                        Id = 3,
                        Title = "Product 2",
                        Description = "Description",
                        Status = TodoStatusDto.InProgress,
                        Created = DateTimeOffset.Now
                    }
                },
                TotalProducts = 3
            });

        ctx.Services.AddSingleton<ITodosClient>(fakeTodosClient);

        var cut = ctx.RenderComponent<TodosPage>();

        // Act
        //cut.Find("button").Click();

        // Assert
        cut.WaitForState(() => cut.Find("tr") != null);

        int expectedNoOfTr = 4; // incl <td> in <thead>

        cut.FindAll("tr").Count.Should().Be(expectedNoOfTr);
    }
}
