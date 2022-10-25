using FluentAssertions;
using NetArchTest.Rules;

namespace YourBrand.Inventory.Architecture.Tests;

public class ArchitectureTests
{
    private const string DomainNamespace = "YourBrand.Inventory.Domain";
    private const string ApplicationNamespace = "YourBrand.Inventory.Application";
    private const string InfrastructureNamespace = "YourBrand.Inventory.Infrastructure";
    private const string PresentationNamespace = "YourBrand.Inventory.Presentation";
    private const string WebNamespace = "YourBrand.Inventory.Web";

    [Fact]
    public void Domain_Should_Not_HaveDependencyOnOtherProjects()
    {
        // Arrange
        var assembly = typeof(YourBrand.Inventory.Domain.Extensions).Assembly;

        var otherProjects = new[]
        {
            ApplicationNamespace,
            InfrastructureNamespace,
            PresentationNamespace,
            WebNamespace
        };

        // Act
        var testResult = Types
            .InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOnAll(otherProjects)
            .GetResult();

        // Assert
        testResult.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Application_Should_Not_HaveDependencyOnOtherProjects()
    {
        // Arrange
        var assembly = typeof(YourBrand.Inventory.Application.ServiceExtensions).Assembly;

        var otherProjects = new[]
        {
            InfrastructureNamespace,
            PresentationNamespace,
            WebNamespace
        };

        // Act
        var testResult = Types
            .InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOnAll(otherProjects)
            .GetResult();

        // Assert
        testResult.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Handlers_Should_Have_DependencyOnDomain()
    {
        // Arrange
        var assembly = typeof(YourBrand.Inventory.Application.ServiceExtensions).Assembly;

        // Act
        var testResult = Types
            .InAssembly(assembly)
            .That()
            .HaveNameEndingWith("Handler")
            .Should()
            .HaveDependencyOn(DomainNamespace)
            .GetResult();

        // Assert
        testResult.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Infrastructure_Should_Not_HaveDependencyOnOtherProjects()
    {
        // Arrange
        var assembly = typeof(YourBrand.Inventory.Application.ServiceExtensions).Assembly;

        var otherProjects = new[]
        {
            PresentationNamespace,
            WebNamespace
        };

        // Act
        var testResult = Types
            .InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOnAll(otherProjects)
            .GetResult();

        // Assert
        testResult.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Presentation_Should_Not_HaveDependencyOnOtherProjects()
    {
        // Arrange
        var assembly = typeof(YourBrand.Inventory.Application.ServiceExtensions).Assembly;

        var otherProjects = new[]
        {
            InfrastructureNamespace,
            PresentationNamespace,
            WebNamespace
        };

        // Act
        var testResult = Types
            .InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOnAll(otherProjects)
            .GetResult();

        // Assert
        testResult.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Controllers_Should_Not_HaveDependencyOnMediatR()
    {
        // Arrange
        var assembly = typeof(YourBrand.Inventory.Presentation.ServiceExtensions).Assembly;

        // Act
        var testResult = Types
            .InAssembly(assembly)
            .That()
            .HaveNameEndingWith("Controller")
            .Should()
            .HaveDependencyOn("MediatR")
            .GetResult();

        // Assert
        testResult.IsSuccessful.Should().BeTrue();
    }
}
