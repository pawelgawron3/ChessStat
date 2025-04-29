using System.Text.Json;
using ChessAPI.Models;
using ChessAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ChessAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ChessController : ControllerBase
{
    private IChessService _chessService;
    private readonly string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Users.json");

    public ChessController(IChessService chessService)
    {
        _chessService = chessService;
    }

    [HttpGet("{username}")]
    public async Task<IActionResult> GetUserInfo(string username)
    {
        var userInfo = await _chessService.GetUserInfo(username);

        if (userInfo == null)
        {
            return NotFound(new { Message = $"User {username} not found!" });
        }

        List<ChessUser> usersList = new List<ChessUser>();

        if (System.IO.File.Exists(filePath))
        {
            try
            {
                var jsonString = await System.IO.File.ReadAllTextAsync(filePath);

                if (!string.IsNullOrWhiteSpace(jsonString))
                {
                    usersList = JsonSerializer.Deserialize<List<ChessUser>>(jsonString);
                }
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");  
            }
        }

        usersList.Add(userInfo);

        var updatedJson = JsonSerializer.Serialize(usersList, new JsonSerializerOptions { WriteIndented = true });
        await System.IO.File.WriteAllTextAsync(filePath, updatedJson);

        return Ok(userInfo);
    }
}