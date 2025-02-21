using Microsoft.EntityFrameworkCore;
using SystemsData.Models;

namespace SystemsData.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<BaseComponent> Base_Components { get; set; }
        public DbSet<ComponentPart> Component_Parts { get; set; }
        public DbSet<SystemEntity> SystemEntities { get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=systemsdata;Username=postgres;Password=postgres");
            }
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BaseComponent>()
                .HasKey(b => b.ComponentId);

            modelBuilder.Entity<ComponentPart>()
                .HasKey(c => c.PartId);

            modelBuilder.Entity<SystemEntity>()
                .HasKey(s => s.SystemId);

            modelBuilder.Entity<SystemEntity>()
                .HasMany(s => s.BaseComponents)
                .WithOne(b => b.SystemEntity)
                .HasForeignKey(b => b.SystemId);

        }
    }
}
