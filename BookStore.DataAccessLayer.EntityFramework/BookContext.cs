using BookStore.DataAccessLayer.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookStore.DataAccessLayer.EntityFramework
{
    public class BookContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<AuthorBook> AuthorBooks { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<GenreBook> GenreBooks { get; set; }
        public DbSet<UserBook> UserBooks { get; set; }
        public DbSet<Basket> UserBaskets { get; set; }

        public BookContext(DbContextOptions<BookContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AuthorBook>().HasKey(t => new { t.BookID, t.AuthorID });
            modelBuilder.Entity<AuthorBook>().HasOne(ab => ab.Book).WithMany(b => b.AuthorBook).HasForeignKey(ab => ab.BookID);
            modelBuilder.Entity<AuthorBook>().HasOne(ab => ab.Author).WithMany(a => a.AuthorBook).HasForeignKey(ab => ab.AuthorID);

            modelBuilder.Entity<GenreBook>().HasKey(k => new { k.BookID, k.GenreID });
            modelBuilder.Entity<GenreBook>().HasOne(gb => gb.Book).WithMany(b => b.GenreBook).HasForeignKey(gb => gb.BookID);
            modelBuilder.Entity<GenreBook>().HasOne(gb => gb.Genre).WithMany(g => g.GenreBook).HasForeignKey(gb => gb.GenreID);

            modelBuilder.Entity<UserBook>().HasKey(y => new { y.BookID, y.UserID });
            modelBuilder.Entity<UserBook>().HasOne(ub => ub.User).WithMany(u => u.UserBook).HasForeignKey(ub => ub.UserID);

            modelBuilder.Entity<Basket>().HasKey(g => new { g.BookID, g.UserID });
            modelBuilder.Entity<Basket>().HasOne(ub => ub.User).WithMany(u => u.UserBasket).HasForeignKey(ub => ub.UserID);

            modelBuilder.Entity<Admin>().HasData(new Admin() { Email = "admin@gmail.com", Password = "admin", ID = 1 });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }
    }
}
