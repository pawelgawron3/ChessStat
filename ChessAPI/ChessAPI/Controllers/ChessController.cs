using ChessAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace ChessAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ChessController : ControllerBase
{
    private IChessService _chessService;
    public ChessController(IChessService chessService)
    {
        _chessService = chessService;
    }

    [HttpGet("{username}")]
    public async Task<IActionResult> GetUserInfo(string username)
    {

        var response = await _chessService.GetUserData(username);

        if (response.IsSuccessStatusCode)
        {
            var userInfo = await response.Content.ReadAsStringAsync();
            var jsonInfo = JsonDocument.Parse(userInfo);

            return Ok(jsonInfo);
        }
        else
        {
            return NotFound(new { Message = $"User {username} not found!" });
        }
    }
}