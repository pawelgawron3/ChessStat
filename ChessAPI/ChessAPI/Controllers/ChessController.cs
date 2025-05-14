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
            return NotFound("User not found!");
        }

        return Ok(userInfo);
    }
}