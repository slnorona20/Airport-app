using AirportAPI.Classes;
using AirportAPI.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

public class Startup
{
    public static string UserDBName;

    public IConfiguration Configuration { get; } 

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;

        UserDBName = Configuration.GetValue<string>("UserDBName");
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddScoped<JsonAirportDatabase>();
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