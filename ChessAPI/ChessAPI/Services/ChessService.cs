using System.Text.Json;

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

    public async Task<object?> GetUserInfo(string username)
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

        var User = new
        {
            Username = userData.RootElement.GetProperty("username"),
            Avatar = userData.RootElement.GetProperty("avatar"),
            Followers = userData.RootElement.GetProperty("followers"),
            Streamer = userData.RootElement.GetProperty("is_streamer"),
            Verified = userData.RootElement.GetProperty("verified"),
            Chess_Rapid = userStats.RootElement.GetProperty("chess_rapid"),
            Chess_Bullet = userStats.RootElement.GetProperty("chess_bullet"),
            Chess_Blitz = userStats.RootElement.GetProperty("chess_blitz"),
            FIDE = userStats.RootElement.GetProperty("fide"),
            Country = userCountry?.RootElement.GetProperty("name").ToString() ?? "Unknow",
        };

        return User;
    }
}