namespace ChessAPI.Models;

public class ChessUser
{
    public string Username { get; set; }
    public string Avatar { get; set; }          
    public int Followers { get; set; }          
    public bool Streamer { get; set; }
    public bool Verified { get; set; }
    public UserGames Rapid { get; set; } = new UserGames();
    public UserGames Blitz { get; set; } = new UserGames();
    public UserGames Bullet { get; set; } = new UserGames();
    public int FIDE { get; set; }               
    public string Country { get; set; }
}