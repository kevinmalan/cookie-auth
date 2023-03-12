namespace Core.Models
{
    public class Profile
    {
        public Guid LookupId { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }
        public string Role { get; set; }
        public DateTimeOffset CreatedOn { get; set; }

        public ICollection<RefreshToken> RefreshTokens { get; set; }
    }
}