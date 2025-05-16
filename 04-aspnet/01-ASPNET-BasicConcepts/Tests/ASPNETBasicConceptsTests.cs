using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace ASPNETBasicConcepts.Tests;

public class ASPNETBasicConceptsTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public ASPNETBasicConceptsTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Test_WebApplication_Starts()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/");

        // Assert
        Assert.True(response.IsSuccessStatusCode);
    }
} 