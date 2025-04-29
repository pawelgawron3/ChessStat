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

    private async Task<JsonDocument?> GetJsonDoc(HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"{response.StatusCode} - {response.ReasonPhrase}");   // Logowanie błędu
            return null;
        }
        else
        {
            string content = await response.Content.ReadAsStringAsync();
            var jsonInfo = JsonDocument.Parse(content);
            return jsonInfo;
        }
    }

    private async Task<HttpResponseMessage> SendRequest(string url)
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);

        requestMessage.Headers.Add("Accept", "application/json");
        requestMessage.Headers.Add("User-Agent", "ChessInfoAPI/1.0 (http://localhost:7281)");   // Wymagane przez API chess.com

        return await _httpClient.SendAsync(requestMessage);
    }

    private UserGames GetUserGames(JsonElement element)
    {
        UserGames userGames = new UserGames();

        if (element.TryGetProperty("last", out var Last))
        {
            userGames.Last.Rating = Last.GetProperty("rating").GetInt32();
            long timestamp = Last.GetProperty("date").GetInt64();
            userGames.Last.Date = DateTimeOffset.FromUnixTimeSeconds(timestamp).UtcDateTime;
            userGames.Last.Rd = Last.GetProperty("rd").GetInt32();
        }

        if (element.TryGetProperty("best", out var Best))
        {
            userGames.Best.Rating = Best.GetProperty("rating").GetInt32();
            long timestamp = Best.GetProperty("date").GetInt64();
            userGames.Best.Date = DateTimeOffset.FromUnixTimeSeconds(timestamp).UtcDateTime;
            userGames.Best.Game = Best.GetProperty("game").GetString()!;
        }

        if (element.TryGetProperty("record", out var Record))
        {
            userGames.Record.Win = Record.GetProperty("win").GetInt32();
            userGames.Record.Loss = Record.GetProperty("loss").GetInt32();
            userGames.Record.Draw = Record.GetProperty("draw").GetInt32();
        }
        return userGames;
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

        var userData = await GetJsonDoc(userDataResponse);
        var userStats = await GetJsonDoc(userStatsResponse);

        if (userData == null || userStats == null)
        {
            return null;
        }

        string countryUrl = userData.RootElement.GetProperty("country").GetString()!;

        var userCountryResponse = await GetUserCountry(countryUrl);
        var userCountry = await GetJsonDoc(userCountryResponse);

        ChessUser? User = new ChessUser()
        {
            Username = userData.RootElement.GetProperty("username").GetString()!,
            Avatar = userData.RootElement.GetProperty("avatar").GetString()!,
            Followers = userData.RootElement.GetProperty("followers").GetInt32(),
            Streamer = userData.RootElement.GetProperty("is_streamer").GetBoolean(),
            Verified = userData.RootElement.GetProperty("verified").GetBoolean(),
            Rapid = GetUserGames(userStats.RootElement.GetProperty("chess_rapid")),
            Bullet = GetUserGames(userStats.RootElement.GetProperty("chess_bullet")),
            Blitz = GetUserGames(userStats.RootElement.GetProperty("chess_blitz")),
            FIDE = userStats.RootElement.GetProperty("fide").GetInt32(),
            Country = userCountry?.RootElement.GetProperty("name").GetString() ?? "Unknow",
        };

        return User;
    }
}