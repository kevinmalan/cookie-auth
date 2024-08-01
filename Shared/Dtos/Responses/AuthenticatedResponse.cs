namespace Shared.Dtos.Responses
{
    public class AuthenticatedResponse
    {
        public string AccessToken { get; set; }
        public string IdToken { get; set; }
        public string RefreshToken { get; set; }
    }
}