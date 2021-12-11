using Entities.Models;
using Microsoft.EntityFrameworkCore;


namespace Entities
{
    public class RepoContext : DbContext
    {
        public RepoContext(DbContextOptions<RepoContext> contextOptions) : base(contextOptions)
        {

        }

        public DbSet<Application> Applications { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Application>(entity =>
            {
                entity.ToTable("Application");
                entity.HasKey(p => p.Id).HasName("PK_ApplicationId");
                entity.Property(p => p.Id).HasColumnName("ApplicationId");
                entity.Property(p => p.Url).IsRequired();
                entity.Property(p => p.PathLocal).IsRequired();
            });
                
            
        }
    }
}
