using NSubstitute;

namespace YourBrand.Catalog.UI.Tests;

public class UnitTest1
{
    [Fact]
    public async Task Test1()
    {
        var productsClient = Substitute.For<IProductsClient>();
        productsClient
            .GetProductAsync(Arg.Any<string>())
            .ReturnsForAnyArgs(Task.FromResult(new ProductDto()
            {
                Id = "",
                Name = "",
                Description = "",
                Group = new ProductGroupDto
                {
                    Id = "",
                    Name = "",
                    Description = ""
                },
                Parent = null,
                Price = 1,
                CompareAtPrice = null,
                QuantityAvailable = null,
                Image = "",
                Visibility = ProductVisibility.Listed
            }));

        var attributesClient = Substitute.For<IAttributesClient>();

        ProductEditViewModel viewModel = new ProductEditViewModel(productsClient, attributesClient);
        await viewModel.InitializeAsync("");
    }
}
