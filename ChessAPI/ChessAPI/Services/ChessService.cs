using System.Text.Json;
using ChessAPI.Models;

namespace ChessAPI.Services;

public class ChessService : IChessService
{
    private readonly HttpClient _httpClient;
    private const string UserDataUrl = "https://api.chess.com/pub/player/";

    public ChessService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    private async Task<T?> GetJsonDoc<T>(HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"{response.StatusCode} - {response.ReasonPhrase}");   // Logowanie błędu
            return default;
        }
        else
        {
            string content = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                AllowTrailingCommas = true
            };
            var deserialisedData = JsonSerializer.Deserialize<T>(content, options);
            return deserialisedData;
        }
    }

    private async Task<HttpResponseMessage> SendRequest(string url)
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);

        requestMessage.Headers.Add("Accept", "application/json");
        requestMessage.Headers.Add("User-Agent", "ChessInfoAPI/1.0 (http://localhost:7281)");   // Wymagane przez API chess.com

        return await _httpClient.SendAsync(requestMessage);
    }

    public async Task<HttpResponseMessage> GetUserData(string username)
    {
        string url = UserDataUrl + username;

        return await SendRequest(url);
    }

    public async Task<HttpResponseMessage> GetUserStats(string username)
    {
        string url = UserDataUrl + username + "/stats";

        return await SendRequest(url);
    }

    public async Task<HttpResponseMessage> GetUserCountry(string url)
    {
        return await SendRequest(url);
    }

    public async Task<ChessUser?> GetUserInfo(string username)
    {
        var userDataResponse = await GetUserData(username);
        var userStatsResponse = await GetUserStats(username);

        var userData = await GetJsonDoc<ChessUser>(userDataResponse);
        var userStats = await GetJsonDoc<GamesData>(userStatsResponse);

        if (userData == null || userStats == null)
        {
            return null;
        }

        var userCountryResponse = await GetUserCountry(userData.Country);
        var userCountry = await GetJsonDoc<UserCountry>(userCountryResponse);

        var chessUser = new ChessUser
        {
            Username = userData.Username,
            Avatar = userData.Avatar,
            Followers = userData.Followers,
            Streamer = userData.Streamer,
            Verified = userData.Verified,
            FIDE = userStats.FIDE,
            Rapid = userStats.ChessRapid ?? new UserGames(),
            Blitz = userStats.ChessBlitz ?? new UserGames(),
            Bullet = userStats.ChessBullet ?? new UserGames(),
            Country = userCountry?.Name ?? "Unknown"
        };

        return chessUser;
    }
}