using DataLayer.EF.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.EF.Contexts
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