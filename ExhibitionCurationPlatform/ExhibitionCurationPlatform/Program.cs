using ExhibitionCurationPlatform.Client.Pages;
using ExhibitionCurationPlatform.Components;
using ExhibitionCurationPlatform.Config;
using ExhibitionCurationPlatform.Context;
using ExhibitionCurationPlatform.Repository;
using ExhibitionCurationPlatform.Repository.Interfaces;
using ExhibitionCurationPlatform.Services;
using ExhibitionCurationPlatform.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using ExhibitionCurationPlatform.Components.Account;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;

namespace ExhibitionCurationPlatform
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("ApplicationDbContextConnection") ?? throw new InvalidOperationException("Connection string 'ApplicationDbContextConnection' not found.");;

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                //options.UseInMemoryDatabase("ExhibitionDb"));
                options.UseSqlServer(connectionString));

            builder.Services.Configure<HarvardArtOptions>(
                builder.Configuration.GetSection("HarvardArt"));

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents()
                .AddInteractiveWebAssemblyComponents();
            builder.Services.AddHttpClient<IMetMuseumService, MetMuseumService>();
            builder.Services.AddHttpClient<IHarvardArtService, HarvardArtService>();
            builder.Services.AddScoped<IArtworkService, ArtworkService>();
            builder.Services.AddScoped<IArtworkRepository, ArtworkRepository>();
            builder.Services.AddScoped<IArtCollectionService, ArtCollectionService>();
            builder.Services.AddScoped<IExhibitionRepository,ExhibitionRepository>();
            builder.Services.AddScoped<IExhibitionService, ExhibitionService>();

            builder.Services.AddCascadingAuthenticationState();

            builder.Services.AddScoped<IdentityUserAccessor>();

            builder.Services.AddScoped<IdentityRedirectManager>();

            builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

            builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();

            builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

            builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

            builder.Services.AddRazorPages();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseAntiforgery();

            app.MapStaticAssets();
            app.MapRazorPages();
            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode()
                .AddInteractiveWebAssemblyRenderMode()
                .AddAdditionalAssemblies(typeof(Client._Imports).Assembly);

            app.MapAdditionalIdentityEndpoints();;

            app.Run();
        }
    }
}
