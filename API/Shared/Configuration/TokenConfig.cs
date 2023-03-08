namespace Shared.Configuration
{
    public class TokenConfig
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public AccessToken AccessToken { get; set; }
        public IdToken IdToken { get; set; }
        public RefreshToken RefreshToken { get; set; }
    }

    public class AccessToken
    {
        public string Secret { get; set; }
        public DateTime Expires { get; set; }
    }

    public class IdToken
    {
        public string Secret { get; set; }
        public DateTime Expires { get; set; }
    }

    public class RefreshToken
    {
        public DateTime Expires { get; set; }
    }
}