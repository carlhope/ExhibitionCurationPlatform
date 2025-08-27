using ExhibitionCurationPlatform.Config;
using ExhibitionCurationPlatform.Services;
using ExhibitionCurationPlatform.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ExhibitionCurationPlatform.TestApiCalls
{
   

    public class Program
    {
        internal static async Task Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.AddUserSecrets<Program>();
                })
                .ConfigureServices((context, services) =>
                {
                    var config = context.Configuration;
                    services.Configure<HarvardArtOptions>(
                    context.Configuration.GetSection("HarvardArt"));
                
                    var options = context.Configuration.GetSection("HarvardArt").Get<HarvardArtOptions>();
                    Console.WriteLine($"Console App API Key: {options.ApiKey ?? "null"}");

                    services.AddHttpClient<IMetMuseumService, MetMuseumService>();
                    services.AddHttpClient<IHarvardArtService, HarvardArtService>();
                    services.AddScoped<IArtCollectionService, ArtCollectionService>();
                })
                .Build();

            var service = host.Services.GetRequiredService<IArtCollectionService>();
            var results = await service.SearchAsync("Monet", 1,12, null, null);

            foreach (var art in results.Items)
            {
                Console.WriteLine($"[{art.Source}] {art.Title} by {art.Artist}");
            }
        }
    }
}
