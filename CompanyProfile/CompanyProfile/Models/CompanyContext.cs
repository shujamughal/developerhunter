using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace CompanyProfile.Models
{
    public class CompanyContext : IdentityDbContext
    {
        private readonly DbContextOptions _options;
        public CompanyContext(DbContextOptions<CompanyContext> options) : base(options)
        {
            _options = options;
            
        }
        private void SeedRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole { Name = "CompanyAdmin", ConcurrencyStamp = "1", NormalizedName = "CompanyAdmin" }
            );
        }
        public DbSet<Company> Companies { get; set; }
        public DbSet<CompanyProfile> CompanyProfiles { get; set; }
        public DbSet<CompanyInsights> CompanyInsights { get; set; }
        public DbSet<CompanyDepartments> CompanyDepartments { get; set; }
        public DbSet<CompanyReview>CompanyReviews { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            this.SeedRoles(modelBuilder);
            modelBuilder.Entity<Company>()
                .HasKey(c => c.Email);

            modelBuilder.Entity<CompanyProfile>()
             .Property(cp => cp.Id)
             .UseIdentityColumn();
            modelBuilder.Entity<CompanyProfile>()
                .HasOne(cp => cp.Company)
                .WithOne(c => c.CompanyProfile)
                .HasForeignKey<CompanyProfile>(cp => cp.CompanyEmail)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CompanyInsights>()
                .HasOne(ci => ci.Company)
                .WithOne(c => c.CompanyInsights)
                .HasForeignKey<CompanyInsights>(ci => ci.CompanyEmail)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CompanyDepartments>()
               .HasOne(cd => cd.Company)
               .WithOne(c => c.CompanyDepartments)
               .HasForeignKey<CompanyDepartments>(cd => cd.CompanyEmail) 
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Company>()
               .HasMany(c => c.CompanyReviews)
               .WithOne(cr => cr.Company)
               .HasForeignKey(cr => cr.CompanyEmail)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CompanyInsights>();
            modelBuilder.Entity<CompanyProfile>();
            modelBuilder.Entity<CompanyDepartments>();
            base.OnModelCreating(modelBuilder);
        }
    }
}
