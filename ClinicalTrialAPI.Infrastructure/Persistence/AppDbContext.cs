using Microsoft.EntityFrameworkCore;

namespace ClinicalTrialAPI.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<ClinicalTrial> ClinicalTrials { get; set; }
    }
}
