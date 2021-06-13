using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using BlazorPracticeApp.Client.Auth;
using BlazorPracticeApp.Client.Helpers;
using BlazorPracticeApp.Client.Repositores;
using Microsoft.AspNetCore.Components.Authorization;
using Tewr.Blazor.FileReader;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using Microsoft.AspNetCore.Components;
using ShareMovieProto;
using Syncfusion.Blazor;

namespace BlazorPracticeApp.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {

            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NjU5MUAzMTM5MmUzMTJlMzBmYlFPZXRJVThMS20zaFlBdjdKMnlKeGJRQng4b0lURDZ1Rk40akFHbnVrPQ==");

            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddTransient<IRepository, Repository>();

            builder.Services.AddSingleton<SingletonService>();

            builder.Services.AddTransient<TransientService>();

            builder.Services.AddScoped<IHttpService, HttpService>();

            builder.Services.AddScoped<IGenreRepo, GenreRepo>();

            builder.Services.AddScoped<IMovieRepo, MovieRepo>();

            builder.Services.AddScoped<IPersonRepo, PersonRepo>();

            builder.Services.AddScoped<IAccountRepo, AccountRepo>();

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            var httpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()));
           
            builder.Services.AddSingleton(services =>
            {
                var baseUri = services.GetRequiredService<NavigationManager>().BaseUri;
                var channel = GrpcChannel.ForAddress(baseUri, new GrpcChannelOptions { HttpClient = httpClient });
                return new GenreProtoService.GenreProtoServiceClient(channel);
            });
            builder.Services.AddSingleton(services =>
            {
                var baseUri = services.GetRequiredService<NavigationManager>().BaseUri;
                var channel = GrpcChannel.ForAddress(baseUri, new GrpcChannelOptions { HttpClient = httpClient });
                return new PersonProtoService.PersonProtoServiceClient(channel);
            });
            builder.Services.AddSingleton(services =>
            {
                var baseUri = services.GetRequiredService<NavigationManager>().BaseUri;
                var channel = GrpcChannel.ForAddress(baseUri, new GrpcChannelOptions { HttpClient = httpClient });
                return new MovieProtoService.MovieProtoServiceClient(channel);
            });

            //builder.Services.AddScoped<AuthenticationStateProvider, TestAuthenticationStateProvider>();

            builder.Services.AddOptions();

            builder.Services.AddAuthorizationCore();

            builder.Services.AddScoped<JWTAuthenticationStateProvider>();

            builder.Services.AddScoped<AuthenticationStateProvider, JWTAuthenticationStateProvider>
                (provider => provider.GetRequiredService<JWTAuthenticationStateProvider>());

            builder.Services.AddScoped<ILoginService, JWTAuthenticationStateProvider>
                (provider => provider.GetRequiredService<JWTAuthenticationStateProvider>());

            //builder.Services.AddBaseAddressHttpClient();

            builder.Services.AddSyncfusionBlazor();
            builder.Services.AddFileReaderService(op => op.InitializeOnFirstCall = true);

            await builder.Build().RunAsync();
        }
    }
}
