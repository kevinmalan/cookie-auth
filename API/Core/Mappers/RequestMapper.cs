namespace Core.Mappers
{
    public static class RequestMapper
    {
        public static DataLayer.EF.Entities.Profile ToEntity(this Models.Profile model)
        {
            return new DataLayer.EF.Entities.Profile
            {
                LookupId = model.LookupId,
                CreatedOn = model.CreatedOn,
                PasswordHash = model.PasswordHash,
                Role = model.Role,
                Salt = model.Salt,
                Username = model.Username,
            };
        }

        public static DataLayer.EF.Entities.RefreshToken ToEntity(this Models.RefreshToken model)
        {
            return new DataLayer.EF.Entities.RefreshToken
            {
                AccessTokenId = model.AccessTokenId,
                ExpiresOn = model.ExpiresOn,
                LookupId = model.LookupId,
                Value = model.Value,
            };
        }
    }
}