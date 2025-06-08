using ChessAPI.Controllers;
using ChessAPI.Models;
using ChessAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace ApiTesting.UnitTests;

public class ChessControllerTests
{
    [Fact]
    public async Task GetPlayerInfo_ReturnsOk_WhenPlayerExists()
    {
        // Arrange
        var mockPlayerService = new Mock<IChessService>();
        mockPlayerService
            .Setup(service => service.GetUserInfo(It.IsIn("testuser")))
            .ReturnsAsync(new ChessUser { Username = "testuser", Country = "USA" });
        var controller = new ChessController(mockPlayerService.Object);

        // Act
        var result = await controller.GetUserInfo("testuser");
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<ChessUser>(okResult.Value);
        Assert.Equal("testuser", returnValue.Username);
    }

    [Fact]
    public async Task GetPlayerInfo_ReturnsNotFound_WhenPlayerDoesNotExist()
    {
        // Arrange
        var mockPlayerService = new Mock<IChessService>();
        mockPlayerService
            .Setup(service => service.GetUserInfo(It.IsIn("nonexistentuser")))
            .ReturnsAsync((ChessUser)null);
        var controller = new ChessController(mockPlayerService.Object);
        string errorMessage = "User not found!";

        // Act
        var result = await controller.GetUserInfo("nonexistentuser");
        // Assert
        var notFoundError = Assert.IsType<NotFoundObjectResult>(result);
        var error = notFoundError.Value;
        Assert.Equal(errorMessage, error);
    }
}