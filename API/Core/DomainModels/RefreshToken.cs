namespace Core.DomainModels
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public Guid LookupId { get; set; }
        public Guid AccessTokenId { get; set; }
        public int ProfileId { get; set; }
        public string Value { get; set; }
        public DateTimeOffset ExpiresOn { get; set; }

        public Profile Profile { get; set; }
    }
}