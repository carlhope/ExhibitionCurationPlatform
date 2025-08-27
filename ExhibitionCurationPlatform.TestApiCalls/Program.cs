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
                    var apiKey = config["HarvardApiKey"];

                    //services.AddSingleton<IHarvardArtService>(new HarvardArtService(new HttpClient(), apiKey));
                    //services.AddSingleton<IMetMuseumService>(new MetMuseumService(new HttpClient()));
                    //services.AddScoped<IArtCollectionService, ArtCollectionService>();
                })
                .Build();

            var service = host.Services.GetRequiredService<IArtCollectionService>();
            var results = await service.SearchAsync("Monet", 1,12);

            foreach (var art in results.Items)
            {
                Console.WriteLine($"[{art.Source}] {art.Title} by {art.Artist}");
            }
        }
    }
}
