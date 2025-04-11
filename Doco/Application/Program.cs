using Doco.Core.Interfaces;
using Doco.Services.Metadata;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Doco.Application;

internal class Program
{
    static void Main(string[] args)
    {
        IHost host = Host.CreateDefaultBuilder(args)
            .ConfigureServices(ConfigureServices)
            .Build();
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        // Register your services here

        services.AddSingleton<MetaDatabase>(_ =>
        {
            string connectionString = "Filename=metadata.db;Connection=shared;Password=Doco;";

            return new MetaDatabase(connectionString);
        });

        services.AddTransient<IMetadataService, MetadataService>();

        // services.AddScoped<IMyRepository, MyRepository>();
    }
}