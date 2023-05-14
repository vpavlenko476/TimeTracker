using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using TimeTracker.Client;

namespace TimeTacker.Frontend
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped<TimeTrackerClient>(x => new TimeTrackerClient(new Uri(builder.Configuration["REST_TIME_TRACKER_URL"])));
            await builder.Build().RunAsync();
        }
    }
}