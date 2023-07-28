using Microsoft.EntityFrameworkCore;
using WebApi.Entities;

namespace WebApi.DBContext
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<AuthorsBooks>().HasKey(ab => new { ab.AuthorId, ab.BooksId } );// combinación de llaves

        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Books> Books { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<AuthorsBooks> AuthorsBooks { get; set; }
    }
}
