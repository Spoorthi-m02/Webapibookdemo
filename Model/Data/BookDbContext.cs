using Microsoft.EntityFrameworkCore;
using BookStoreAPI.Model.Entities;

namespace BookStoreAPI.Model.Data
{
    public class BookDbContext : DbContext
    {
        public BookDbContext(DbContextOptions<BookDbContext> options) : base(options)
        {
            // config db
        }
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Category> Categories { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seed Categories
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Fiction" },
                new Category { Id = 2, Name = "Non-Fiction" },
                new Category { Id = 3, Name = "Science Fiction" }
            );

            // Seed Authors
            modelBuilder.Entity<Author>().HasData(
                new Author { Id = 1, Name = "George Orwell", Biography = "English novelist and essayist, journalist and critic." },
                new Author { Id = 2, Name = "Isaac Asimov", Biography = "American writer and professor of biochemistry, known for his works of science fiction." }
            );

            // Seed Books
            modelBuilder.Entity<Book>().HasData(
                new Book { Id = 1, Title = "Technology", ISBN = "978-0451524935", AuthorId = 1, CategoryId = 1, PublishedDate = new DateTime(1949, 6, 8) },
                new Book { Id = 2, Title = "Robot", ISBN = "978-0553294385", AuthorId = 2, CategoryId = 3, PublishedDate = new DateTime(1950, 12, 2) }
            );
        }

    }
}
