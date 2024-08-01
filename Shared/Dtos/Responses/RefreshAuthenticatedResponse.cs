namespace Shared.Dtos.Responses
{
    public class RefreshAuthenticatedResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTimeOffset RefreshTokenExpiresOn { get; set; }
    }
}