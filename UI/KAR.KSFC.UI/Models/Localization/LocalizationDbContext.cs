using Microsoft.EntityFrameworkCore;

namespace KAR.KSFC.UI.Models.Localization
{
    public class LocalizationDbContext : DbContext
    {
        public DbSet<Culture> Cultures { get; set; }
        public DbSet<Resource> Resources { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(databaseName: "LocalizationDb1Context");
        }
    }
}
