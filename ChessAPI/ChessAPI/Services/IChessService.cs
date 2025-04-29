namespace ChessAPI.Services;

public interface IChessService
{
    public Task<HttpResponseMessage> GetUserData(string username);

    public Task<HttpResponseMessage> GetUserStats(string username);

    public Task<HttpResponseMessage> GetUserCountry(string username);

    public Task<object?> GetUserInfo(string username);
}