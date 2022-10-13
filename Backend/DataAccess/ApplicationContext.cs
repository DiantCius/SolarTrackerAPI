using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.DataAccess
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Powerplant> Powerplants { get; set; }
        public DbSet<Code> Codes { get; set; }
        public DbSet<EnergyProduction> EnergyProductions { get; set; }

    }
}
