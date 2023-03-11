using Core.DomainModels;
using Microsoft.EntityFrameworkCore;

namespace Core.Contexts
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Profile> Profile { get; set; }
        public DbSet<RefreshToken> RefreshToken { get; set; }
    }
}