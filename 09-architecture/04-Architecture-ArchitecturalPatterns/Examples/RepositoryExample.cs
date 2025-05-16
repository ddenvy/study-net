using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RepositoryPattern
{
    // Модель данных
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

    // Интерфейс репозитория
    public interface IRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task SaveChangesAsync();
    }

    // Конкретная реализация репозитория
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        protected readonly Microsoft.EntityFrameworkCore.DbSet<T> _dbSet;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }

    // Специализированный репозиторий для продуктов
    public interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId);
        Task<IEnumerable<Product>> GetExpensiveProductsAsync(decimal minPrice);
    }

    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId)
        {
            return await _dbSet
                .Where(p => p.CategoryId == categoryId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetExpensiveProductsAsync(decimal minPrice)
        {
            return await _dbSet
                .Where(p => p.Price >= minPrice)
                .ToListAsync();
        }
    }

    // Unit of Work
    public interface IUnitOfWork : IDisposable
    {
        IProductRepository Products { get; }
        Task<int> SaveChangesAsync();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public IProductRepository Products { get; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Products = new ProductRepository(context);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }

    // Пример использования
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Демонстрация паттерна Repository\n");

            using (var context = new ApplicationDbContext())
            {
                using (var unitOfWork = new UnitOfWork(context))
                {
                    // Создание категории
                    var category = new Category { Name = "Электроника" };
                    context.Categories.Add(category);
                    await context.SaveChangesAsync();

                    // Добавление продуктов
                    var product1 = new Product { Name = "Смартфон", Price = 1000, CategoryId = category.Id };
                    var product2 = new Product { Name = "Ноутбук", Price = 2000, CategoryId = category.Id };

                    await unitOfWork.Products.AddAsync(product1);
                    await unitOfWork.Products.AddAsync(product2);
                    await unitOfWork.SaveChangesAsync();

                    // Получение всех продуктов
                    Console.WriteLine("Все продукты:");
                    var allProducts = await unitOfWork.Products.GetAllAsync();
                    foreach (var product in allProducts)
                    {
                        Console.WriteLine($"- {product.Name} (${product.Price})");
                    }

                    // Получение продуктов по категории
                    Console.WriteLine("\nПродукты в категории 'Электроника':");
                    var categoryProducts = await unitOfWork.Products.GetByCategoryAsync(category.Id);
                    foreach (var product in categoryProducts)
                    {
                        Console.WriteLine($"- {product.Name}");
                    }

                    // Получение дорогих продуктов
                    Console.WriteLine("\nДорогие продукты (от $1500):");
                    var expensiveProducts = await unitOfWork.Products.GetExpensiveProductsAsync(1500);
                    foreach (var product in expensiveProducts)
                    {
                        Console.WriteLine($"- {product.Name} (${product.Price})");
                    }

                    // Обновление продукта
                    var productToUpdate = await unitOfWork.Products.GetByIdAsync(1);
                    if (productToUpdate != null)
                    {
                        productToUpdate.Price = 1200;
                        await unitOfWork.Products.UpdateAsync(productToUpdate);
                        await unitOfWork.SaveChangesAsync();
                        Console.WriteLine("\nЦена смартфона обновлена");
                    }

                    // Удаление продукта
                    var productToDelete = await unitOfWork.Products.GetByIdAsync(2);
                    if (productToDelete != null)
                    {
                        await unitOfWork.Products.DeleteAsync(productToDelete);
                        await unitOfWork.SaveChangesAsync();
                        Console.WriteLine("Ноутбук удален");
                    }
                }
            }
        }
    }
}