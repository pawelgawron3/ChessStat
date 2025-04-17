namespace ChessAPI.Services;

public interface IChessService
{
    public Task<HttpResponseMessage> GetUserData(string username);
}