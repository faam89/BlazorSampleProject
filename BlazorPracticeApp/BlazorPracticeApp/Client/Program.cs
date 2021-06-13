using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using BlazorPracticeApp.Client.Helpers;

namespace BlazorPracticeApp.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");


            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddTransient<IRepositoy,Repository>();

            builder.Services.AddSingleton<SingletonService>();

            builder.Services.AddTransient<TransientService>();

            await builder.Build().RunAsync();
        }
    }
}
