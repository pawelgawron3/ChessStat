using ChessAPI.Helpers;
using System.Text.Json.Serialization;

namespace ChessAPI.Models;

public class UserGames
{
    public LastInfo Last { get; set; } = new LastInfo();
    public BestInfo Best { get; set; } = new BestInfo();
    public RecordInfo Record { get; set; } = new RecordInfo();
}

public class LastInfo
{
    public int Rating { get; set; }

    [JsonConverter(typeof(UnixDateTimeConverter))]
    public DateTime Date { get; set; }

    public int Rd { get; set; }
}

public class BestInfo
{
    public int Rating { get; set; }

    [JsonConverter(typeof(UnixDateTimeConverter))]
    public DateTime Date { get; set; }

    public string Game { get; set; }
}

public class RecordInfo
{
    public int Win { get; set; }
    public int Loss { get; set; }
    public int Draw { get; set; }
}