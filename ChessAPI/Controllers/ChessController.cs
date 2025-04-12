using Microsoft.AspNetCore.Mvc;

namespace ChessAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ChessController : ControllerBase
{
    private readonly HttpClient _httpClient;

    public ChessController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    [HttpGet("{username}")]
    public async Task<IActionResult> GetUserInfo(string username)
    {
        var url = $"https://api.chess.com/pub/player/{username}";

        // Logowanie URL w celu sprawdzenia poprawności URL
        Console.WriteLine($"Requesting Chess.com API: {url}");

        // Tworzymy zapytanie HTTP
        var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
        requestMessage.Headers.Add("Accept", "application/json");
        requestMessage.Headers.Add("User-Agent", "ChessInfoAPI/1.0 (http://localhost:7281)"); // Wymagana identyfikacja przez API chess.com

        var response = await _httpClient.SendAsync(requestMessage);

        if (response.IsSuccessStatusCode)
        {
            var userInfo = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Response from Chess.com: {userInfo}"); //Logowanie odpowiedzi
            return Ok(userInfo);
        }
        else
        {
            Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}"); // Logowanie błędu
            return NotFound(new { Message = $"User {username} not found!" });
        }
    }
}