using System.Text.Json.Serialization;

namespace ChessAPI.Models;

public class UserCountry
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}