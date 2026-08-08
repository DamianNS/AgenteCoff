using AgenteCoff.ApiService.Data;
using Microsoft.EntityFrameworkCore;

namespace AgenteCoff.ApiService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 1. Configuraciones del Contenedor de Dependencias (Servicios)
            builder.AddServiceDefaults();
            builder.Services.AddProblemDetails();
            builder.Services.AddOpenApi();
            builder.Services.AddControllers(); //  MOVIDO ACÁ: Antes del Build

            // Configurar Entity Framework Core con SQLite
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=/app/data/agentecoff.db";

            var dbFilePath = connectionString.Replace("Data Source=", "").Trim();

            // Si contiene una ruta de carpetas (como en Docker), asegura que la carpeta exista antes de arrancar
            if (dbFilePath.Contains('/') || dbFilePath.Contains('\\'))
            {
                var directory = Path.GetDirectoryName(dbFilePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
            }

            builder.Services.AddDbContext<Data.AppDbContext>(options =>
                options.UseSqlite(connectionString));

            // 2. Construcción de la aplicación
            var app = builder.Build(); // A partir de acá, los servicios son de solo lectura

            // 🚀 TRUCO AUTOMÁTICO: Crea la base de datos y la tabla si no existen al iniciar
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.EnsureCreated(); // Ideal para SQLite en entornos hogareños/Raspberry
            }

            // 3. Configuración del Pipeline HTTP (Middlewares y Endpoints)
            app.UseExceptionHandler();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.MapControllers();
            app.MapDefaultEndpoints();
            app.MapStaticAssets();
            app.Run();
        }
    }
}
