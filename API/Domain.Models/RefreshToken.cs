﻿namespace Domain.Models
{
    public class RefreshToken
    {
        public Guid LookupId { get; set; }
        public Guid AccessTokenId { get; set; }
        public int ProfileId { get; set; }
        public string Value { get; set; }
        public DateTimeOffset ExpiresOn { get; set; }
    }
}