using CMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CMS.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Site> Sites { get; set; } // Added this line

        // Optional: If you need to configure the relationship explicitly,
        // you can do it here using OnModelCreating.
        // However, EF Core often infers relationships correctly from ForeignKey attributes.
        // protected override void OnModelCreating(ModelBuilder modelBuilder)
        // {
        //     base.OnModelCreating(modelBuilder);
        //     // Example: If you needed to define a composite key or a more complex relationship
        //     // modelBuilder.Entity<Site>()
        //     //     .HasOne(s => s.Company)
        //     //     .WithMany() // Assuming a Company can have many Sites (add a collection in Company model if so)
        //     //     .HasForeignKey(s => s.CompanyId);
        // }
    }
}
