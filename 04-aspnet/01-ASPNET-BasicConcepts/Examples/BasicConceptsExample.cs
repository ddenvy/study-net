using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BasicConceptsExample
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Регистрация сервисов
            services.AddControllers();
            services.AddSingleton<IMyService, MyService>();
            services.AddTransient<IMyRepository, MyRepository>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Настройка middleware
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();

            // Пользовательский middleware
            app.Use(async (context, next) =>
            {
                logger.LogInformation("Request started");
                await next();
                logger.LogInformation("Request completed");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }

    // Пример сервиса
    public interface IMyService
    {
        string GetData();
    }

    public class MyService : IMyService
    {
        private readonly IMyRepository _repository;
        private readonly ILogger<MyService> _logger;

        public MyService(IMyRepository repository, ILogger<MyService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public string GetData()
        {
            _logger.LogInformation("Getting data from service");
            return _repository.GetData();
        }
    }

    // Пример репозитория
    public interface IMyRepository
    {
        string GetData();
    }

    public class MyRepository : IMyRepository
    {
        private readonly ILogger<MyRepository> _logger;

        public MyRepository(ILogger<MyRepository> logger)
        {
            _logger = logger;
        }

        public string GetData()
        {
            _logger.LogInformation("Getting data from repository");
            return "Sample Data";
        }
    }

    // Пример контроллера
    [ApiController]
    [Route("[controller]")]
    public class SampleController : ControllerBase
    {
        private readonly IMyService _service;
        private readonly ILogger<SampleController> _logger;

        public SampleController(IMyService service, ILogger<SampleController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            _logger.LogInformation("Handling GET request");
            return Ok(_service.GetData());
        }
    }
} 