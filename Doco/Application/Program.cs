using Doco.Core;
using Doco.Core.Interfaces;
using Doco.Core.Models;
using Doco.Services.Metadata;
using Doco.Services.Search;
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

        var searchService = host.Services.GetRequiredService<ISearchService>();
        var metadataService = host.Services.GetRequiredService<IMetadataService>();

        string documentPath = @"D:\Downloads\Test.txt";
        metadataService.SetMetadata(documentPath, new DocumentMetadata()
        {
            CreatedAt = DateTime.Now,
            LastModifiedAt = DateTime.Now,
            FileName = "Test.txt",
            Type = DocumentType.PlainText,
            Tags = new Dictionary<string, string>() {{"TestKey", "TestValue"}}
        });

        searchService.IndexDocument(documentPath);

        var result = searchService.Search("lorem");

    }

    private static void ConfigureServices(IServiceCollection services)
    {
        // Register your services here

        services.AddSingleton<IMetaDatabase>(_ =>
        {
            string connectionString = "Filename=metadata.db;Connection=shared;Password=Doco;";

            return new MetaDatabase(connectionString);
        });

        services.AddSingleton<ISearchService, InMemorySearchService>();

        services.AddTransient<IMetadataService, MetadataService>();

        // services.AddScoped<IMyRepository, MyRepository>();
    }
}