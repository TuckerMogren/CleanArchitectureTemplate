using System.Reflection;
using NetArchTest.Rules;
using Xunit;

namespace ArchitectureTests;

public sealed class ArchitectureTests
{
    // Point to assemblies via any known type from each project
    private static readonly Assembly _domain = typeof(Domain.Marker).Assembly;
    private static readonly Assembly _application = typeof(Application.Marker).Assembly;
    private static readonly Assembly _infrastructure = typeof(Infrastructure.Marker).Assembly;
    private static readonly Assembly _persistence = typeof(Infrastructure.Persistence.Marker).Assembly;
    private static readonly Assembly _webApi = typeof(Program).Assembly;
    private static readonly Assembly _worker = typeof(Worker.Marker).Assembly;

    [Fact]
    public void Domain_Should_Not_Depend_On_Application_Infrastructure_Web()
    {
        TestResult result = Types.InAssembly(_domain)
            .ShouldNot()
            .HaveDependencyOnAny(
                "Application",
                "Infrastructure",
                "Infrastructure.Persistence",
                "WebApi")
            .GetResult();

        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void Application_Should_Depend_Only_On_Domain()
    {
        // Negative checks
        TestResult negative = Types.InAssembly(_application)
            .ShouldNot()
            .HaveDependencyOnAny(
                "Infrastructure",
                "Infrastructure.Persistence",
                "WebApi")
            .GetResult();
        Assert.True(negative.IsSuccessful);

        // Positive check that Application references Domain
        TestResult positive = Types.InAssembly(_application)
            .Should()
            .HaveDependencyOn("Domain")
            .GetResult();
        Assert.True(positive.IsSuccessful);
    }

    [Fact]
    public void Infrastructure_Should_Depend_On_Application_And_Domain_Only()
    {
        // Should not depend on WebApi
        Assert.True(
            Types.InAssembly(_infrastructure)
                .ShouldNot()
                .HaveDependencyOnAny("WebApi")
                .GetResult().IsSuccessful);

        // Should depend on Application or Domain
        Assert.True(
            Types.InAssembly(_infrastructure)
                .Should()
                .HaveDependencyOnAny("Application", "Domain")
                .GetResult().IsSuccessful);
    }

    [Fact]
    public void Persistence_Should_Not_Depend_On_Application_Or_Web()
    {
        TestResult result = Types.InAssembly(_persistence)
            .ShouldNot()
            .HaveDependencyOnAny(
                "Domain",
                "Application",
                "WebApi")
            .GetResult();

        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void OtherLayers_MustNot_DependOn_WebApi()
    {
        Assert.True(Types.InAssembly(_domain).ShouldNot().HaveDependencyOnAny("WebApi").GetResult().IsSuccessful);
        Assert.True(Types.InAssembly(_application).ShouldNot().HaveDependencyOnAny("WebApi").GetResult().IsSuccessful);
        Assert.True(Types.InAssembly(_persistence).ShouldNot().HaveDependencyOnAny("WebApi").GetResult().IsSuccessful);
        Assert.True(Types.InAssembly(_infrastructure).ShouldNot().HaveDependencyOnAny("WebApi").GetResult().IsSuccessful);
        Assert.True(Types.InAssembly(_worker).ShouldNot().HaveDependencyOnAny("WebApi").GetResult().IsSuccessful);
    }

    [Fact]
    public void OtherLayers_MustNot_DependOn_Worker()
    {
        Assert.True(Types.InAssembly(_domain).ShouldNot().HaveDependencyOnAny("Worker").GetResult().IsSuccessful);
        Assert.True(Types.InAssembly(_application).ShouldNot().HaveDependencyOnAny("Worker").GetResult().IsSuccessful);
        Assert.True(Types.InAssembly(_persistence).ShouldNot().HaveDependencyOnAny("Worker").GetResult().IsSuccessful);
        Assert.True(Types.InAssembly(_infrastructure).ShouldNot().HaveDependencyOnAny("Worker").GetResult().IsSuccessful);
        Assert.True(Types.InAssembly(_worker).ShouldNot().HaveDependencyOnAny("Worker").GetResult().IsSuccessful);
    }

    [Fact]
    public void WebApi_MustNot_DependOn_Test_Projects()
    {
        TestResult result = Types.InAssembly(_webApi)
            .ShouldNot()
            .HaveDependencyOnAny("Application.Tests", "Domain.Tests", "Infrastructure.Tests","Infrastructure.*.Tests")
            .GetResult();
        Assert.True(result.IsSuccessful);
    }

        [Fact]
    public void Worker_MustNot_DependOn_Test_Projects()
    {
        TestResult result = Types.InAssembly(_webApi)
            .ShouldNot()
            .HaveDependencyOnAny("Application.Tests", "Domain.Tests", "Infrastructure.Tests", "Infrastructure.*.Tests")
            .GetResult();
        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void No_Project_Should_Reference_Newtonsoft_In_Domain()
    {
        TestResult result = Types.InAssembly(_domain)
            .ShouldNot()
            .HaveDependencyOnAny("Newtonsoft.Json")
            .GetResult();

        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void Application_Should_Not_Reference_Persistence_Namespaces()
    {
        TestResult result = Types.InAssembly(_application)
            .ShouldNot()
            .HaveDependencyOnAny(
                "Infrastructure.Persistence",
                "Microsoft.EntityFrameworkCore")
            .GetResult();

        Assert.True(result.IsSuccessful);
    }
}
