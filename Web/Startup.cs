using EFCore;
using EFCore.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Web.Configuration;
using Web.Db;

namespace Web;

public class Startup
{
    public IConfiguration ConfigRoot { get; }
    private readonly string _connectionString;

    public Startup(IConfiguration configuration, string connectionString)
    {
        ConfigRoot = configuration;
        _connectionString = connectionString;
    }

    public void ConfigureService(IServiceCollection services)
    {
        services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(_connectionString));
        
        services.AddIdentity<User, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<ApplicationDbContext>();
        services.AddIdentityServer().AddApiAuthorization<User, ApplicationDbContext>();
        
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddApiVersioning(options => options.ReportApiVersions = true);
        services.AddVersionedApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });
        services.AddSwaggerGen();
        services.ConfigureOptions<ConfigureSwaggerOptions>();
    }

    public void Configure(WebApplication app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/v{description.ApiVersion.ToString()}/swagger.json", 
                        $"V{description.ApiVersion.ToString()}");
                }
            });
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.UseAuthentication();
        app.MapControllers();

        CreateDbIfNotExists(app);
        
        app.Run();
    }

    private static void CreateDbIfNotExists(IHost host)
    {
        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            try
            {
                var context = services.GetRequiredService<ApplicationDbContext>();
                context.Database.EnsureCreated();
                context.SaveChanges();
            }
            catch (Exception e)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(e, "An error occured creating the database!");
            }
        }
    }
}