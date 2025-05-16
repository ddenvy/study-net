using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace EFBasicConceptsExample
{
    // Модели данных
    public class Blog
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
    }

    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int BlogId { get; set; }
        public virtual Blog Blog { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }

    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int PostId { get; set; }
        public virtual Post Post { get; set; }
    }

    // DbContext
    public class BlogContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=BlogDb;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Fluent API конфигурация
            modelBuilder.Entity<Blog>()
                .HasMany(b => b.Posts)
                .WithOne(p => p.Blog)
                .HasForeignKey(p => p.BlogId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Post>()
                .HasMany(p => p.Comments)
                .WithOne(c => c.Post)
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new BlogContext())
            {
                // Создание базы данных
                context.Database.EnsureCreated();

                // Пример 1: Добавление данных
                var blog = new Blog
                {
                    Title = "Мой блог",
                    Content = "Содержание блога",
                    CreatedAt = DateTime.Now
                };
                context.Blogs.Add(blog);
                context.SaveChanges();

                // Пример 2: Запрос данных
                var blogs = context.Blogs
                    .Include(b => b.Posts)
                    .ThenInclude(p => p.Comments)
                    .ToList();

                // Пример 3: Обновление данных
                var firstBlog = context.Blogs.First();
                firstBlog.Title = "Обновленный заголовок";
                context.SaveChanges();

                // Пример 4: Удаление данных
                var blogToDelete = context.Blogs.FirstOrDefault(b => b.Title == "Обновленный заголовок");
                if (blogToDelete != null)
                {
                    context.Blogs.Remove(blogToDelete);
                    context.SaveChanges();
                }

                // Пример 5: Транзакции
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var newBlog = new Blog
                        {
                            Title = "Блог в транзакции",
                            Content = "Содержание",
                            CreatedAt = DateTime.Now
                        };
                        context.Blogs.Add(newBlog);
                        context.SaveChanges();

                        var newPost = new Post
                        {
                            Title = "Пост в транзакции",
                            Content = "Содержание поста",
                            BlogId = newBlog.Id
                        };
                        context.Posts.Add(newPost);
                        context.SaveChanges();

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
    }
} 