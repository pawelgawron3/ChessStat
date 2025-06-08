using System.Text.Json.Serialization;

namespace ChessAPI.Models;

public class ChessUser
{
    [JsonPropertyName("username")]
    public string Username { get; set; }

    [JsonPropertyName("avatar")]
    public string Avatar { get; set; }

    [JsonPropertyName("followers")]
    public int Followers { get; set; }

    [JsonPropertyName("is_streamer")]
    public bool Streamer { get; set; }

    [JsonPropertyName("verified")]
    public bool Verified { get; set; }

    public int FIDE { get; set; }

    public UserGames Rapid { get; set; } = new UserGames();
    public UserGames Blitz { get; set; } = new UserGames();
    public UserGames Bullet { get; set; } = new UserGames();

    [JsonPropertyName("country")]
    public string Country { get; set; }
}