namespace ChessAPI.Services
{
    public class ChessService : IChessService
    {
        private readonly HttpClient _httpClient;
        private const string UserDataUrl= "https://api.chess.com/pub/player/";
        public ChessService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HttpResponseMessage> GetUserData(string username)

        {
            string url = UserDataUrl + username;

            var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);

            requestMessage.Headers.Add("Accept", "application/json");
            requestMessage.Headers.Add("User-Agent", "ChessInfoAPI/1.0 (http://localhost:7281)"); 

            var response = await _httpClient.SendAsync(requestMessage);
            return response;

        }
    }
}
