using Microsoft.EntityFrameworkCore;
using WebApi.Entities;

namespace WebApi.DBContext
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Books> Books { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }
}
