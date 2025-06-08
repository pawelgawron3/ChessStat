using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiTesting.IntegrationTests;

public class ChessControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ChessControllerIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetUserInfo_ReturnsOk_WhenUserExists()
    {
        // Arrange
        var username = "byniolus";
        // Act
        var response = await _client.GetAsync($"/api/Chess/{username}");
        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains(username, content);
    }

    [Fact]
    public async Task GetUserInfo_ReturnsNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        var username = "'";
        // Act
        var response = await _client.GetAsync($"/api/Chess/{username}");
        // Assert
        Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
    }
}