using AirportAPI.Classes;
using AirportAPI.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

public class Startup
{ 
    public IConfiguration Configuration { get; } 

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
        var ApiKeyHaderName = Configuration.GetValue<string>("ApiKeyName");
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddScoped<FileDatabase>();
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