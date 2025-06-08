using System.Text.Json.Serialization;

namespace ChessAPI.Models;

public class GamesData
{
    [JsonPropertyName("fide")]
    public int FIDE { get; set; }

    [JsonPropertyName("chess_rapid")]
    public UserGames ChessRapid { get; set; }

    [JsonPropertyName("chess_bullet")]
    public UserGames ChessBullet { get; set; }

    [JsonPropertyName("chess_blitz")]
    public UserGames ChessBlitz { get; set; }
}