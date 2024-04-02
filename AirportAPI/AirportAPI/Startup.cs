using AirportAPI.Classes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public class Startup
{
    public static string UserDBName;
    public static string DBConnectionString;

    public IConfiguration Configuration { get; } 

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;

        // Only use if DB = json file
        // UserDBName = Configuration.GetValue<string>("UserDBName");

        DBConnectionString = Configuration.GetConnectionString("MySQL");
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddScoped<MySqlAirportDatabase>();
        services.AddControllers();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}