namespace Core.DomainModels
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public Guid LookupId { get; set; }
        public string AccessTokenId { get; set; }
        public int ProfileId { get; set; }
        public Guid Value { get; set; }
        public DateTimeOffset ExpiresOn { get; set; }

        public Profile Profile { get; set; }
    }
}