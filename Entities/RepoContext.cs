using Entities.Models;
using Microsoft.EntityFrameworkCore;


namespace Entities
{
    public class RepoContext : DbContext
    {
        public RepoContext(DbContextOptions contextOptions) : base(contextOptions)
        {

        }

        public DbSet<Application> Applications { get; set; }
    }
}
