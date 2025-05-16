using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace EFChangeTrackingExample
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
    }

    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }

    public class StoreContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=StoreDb;Trusted_Connection=True;");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new StoreContext())
            {
                context.Database.EnsureCreated();

                // Пример 1: Отслеживание состояний
                var product = new Product { Name = "Телефон", Price = 1000 };
                Console.WriteLine($"Состояние после создания: {context.Entry(product).State}"); // Detached

                context.Products.Add(product);
                Console.WriteLine($"Состояние после Add: {context.Entry(product).State}"); // Added

                context.SaveChanges();
                Console.WriteLine($"Состояние после SaveChanges: {context.Entry(product).State}"); // Unchanged

                product.Price = 1200;
                Console.WriteLine($"Состояние после изменения: {context.Entry(product).State}"); // Modified

                // Пример 2: Attach/Detach
                var detachedProduct = new Product { Id = 1, Name = "Ноутбук", Price = 2000 };
                context.Products.Attach(detachedProduct);
                Console.WriteLine($"Состояние после Attach: {context.Entry(detachedProduct).State}"); // Unchanged

                context.Entry(detachedProduct).State = EntityState.Modified;
                Console.WriteLine($"Состояние после изменения: {context.Entry(detachedProduct).State}"); // Modified

                // Пример 3: Локальное кэширование
                var localProducts = context.Products.Local;
                Console.WriteLine($"Количество продуктов в кэше: {localProducts.Count}");

                // Пример 4: AsNoTracking
                var products = context.Products
                    .AsNoTracking()
                    .ToList();
                Console.WriteLine($"Состояние после AsNoTracking: {context.Entry(products[0]).State}"); // Detached

                // Пример 5: Отложенная загрузка
                var category = context.Categories.First();
                Console.WriteLine("Загрузка связанных продуктов...");
                var productsInCategory = category.Products; // Lazy Loading
                Console.WriteLine($"Количество продуктов в категории: {productsInCategory.Count}");

                // Пример 6: Явная загрузка
                var category2 = context.Categories.First();
                context.Entry(category2)
                    .Collection(c => c.Products)
                    .Load();
                Console.WriteLine($"Количество продуктов после явной загрузки: {category2.Products.Count}");

                // Пример 7: Жадная загрузка
                var categoriesWithProducts = context.Categories
                    .Include(c => c.Products)
                    .ToList();
                Console.WriteLine($"Количество категорий с продуктами: {categoriesWithProducts.Count}");
            }
        }
    }
} 