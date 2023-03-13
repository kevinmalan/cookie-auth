namespace DataLayer.EF
{
    public static class Mapper
    {
        public static Entities.Profile ToEntity(this Domain.Models.Profile model)
        {
            return new Entities.Profile
            {
                LookupId = model.LookupId,
                CreatedOn = model.CreatedOn,
                PasswordHash = model.PasswordHash,
                Role = model.Role,
                Salt = model.Salt,
                Username = model.Username,
            };
        }

        public static Entities.RefreshToken ToEntity(this Domain.Models.RefreshToken model)
        {
            return new Entities.RefreshToken
            {
                AccessTokenId = model.AccessTokenId,
                ExpiresOn = model.ExpiresOn,
                LookupId = model.LookupId,
                Value = model.Value,
            };
        }

        public static Domain.Models.Profile ToModel(this Entities.Profile entity)
        {
            return new Domain.Models.Profile
            {
                Username = entity.Username,
                CreatedOn = entity.CreatedOn,
                LookupId = entity.LookupId,
                PasswordHash = entity.PasswordHash,
                Role = entity.Role,
                Salt = entity.Salt,
            };
        }

        public static Domain.Models.RefreshToken ToModel(this Entities.RefreshToken entity)
        {
            return new Domain.Models.RefreshToken
            {
                AccessTokenId = entity.AccessTokenId,
                ExpiresOn = entity.ExpiresOn,
                LookupId = entity.LookupId,
                Value = entity.Value
            };
        }
    }
}