namespace Domain.Mappers
{
    public static class ResponseMapper
    {
        public static Models.Profile ToModel(this DataLayer.EF.Entities.Profile entity)
        {
            return new Models.Profile
            {
                Username = entity.Username,
                CreatedOn = entity.CreatedOn,
                LookupId = entity.LookupId,
                PasswordHash = entity.PasswordHash,
                Role = entity.Role,
                Salt = entity.Salt,
            };
        }
    }
}