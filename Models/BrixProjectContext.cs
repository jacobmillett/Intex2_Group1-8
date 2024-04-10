using Microsoft.EntityFrameworkCore;

namespace AuroraBricks.Models
{
    public class BrixProjectContext : DbContext
    {
        public BrixProjectContext(DbContextOptions<BrixProjectContext> options) : base(options) { }

        public DbSet<Products> BrixProducts { get; set; }
    }
}
