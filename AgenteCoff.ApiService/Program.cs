using System.Text;
using AgenteCoff.ApiService.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace AgenteCoff.ApiService
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.AddServiceDefaults();
            builder.Services.AddProblemDetails();
            builder.Services.AddOpenApi();
            builder.Services.AddControllers();

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=/app/data/agentecoff.db";
            var dbFilePath = connectionString.Replace("Data Source=", "").Trim();

            if (dbFilePath.Contains('/') || dbFilePath.Contains('\\'))
            {
                var directory = Path.GetDirectoryName(dbFilePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
            }

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(connectionString));

            builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequiredLength = 6;
                })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            var jwtKey = builder.Configuration["JwtSettings:Key"] ?? "AgenteCoffLocalDevelopmentKey!2026-2027-ValidKey";
            var jwtIssuer = builder.Configuration["JwtSettings:Issuer"] ?? "AgenteCoff";
            var jwtAudience = builder.Configuration["JwtSettings:Audience"] ?? "AgenteCoff-Users";

            builder.Services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = jwtIssuer,
                        ValidateAudience = true,
                        ValidAudience = jwtAudience,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromMinutes(1)
                    };
                });

            builder.Services.AddAuthorization();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.EnsureCreated();

                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                var defaultEmail = "admin@agentecoff.local";
                var defaultPassword = "Password123!";

                if (await userManager.FindByEmailAsync(defaultEmail) is null)
                {
                    var defaultUser = new IdentityUser
                    {
                        UserName = defaultEmail,
                        Email = defaultEmail,
                        EmailConfirmed = true
                    };

                    await userManager.CreateAsync(defaultUser, defaultPassword);
                }
            }

            app.UseExceptionHandler();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.MapDefaultEndpoints();
            app.MapStaticAssets();
            app.Run();
        }
    }
}
