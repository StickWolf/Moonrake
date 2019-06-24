using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using ServerEngine.Grains;
using System.Net;
using System.Threading.Tasks;

namespace ServerEngine.GrainSiloAndClient
{
    public static class GrainSiloHost
    {
        private static ISiloHost SiloHost { get; set; }

        public static async Task StartAsync()
        {
            if (SiloHost == null)
            {
                SiloHost = new SiloHostBuilder()
                    .UseLocalhostClustering()
                    .Configure<ClusterOptions>(options =>
                    {
                        options.ClusterId = "GameState";
                        options.ServiceId = "ServerEngine";
                    })
                    .ConfigureApplicationParts(parts =>
                    {
                        parts.AddApplicationPart(typeof(GameUniverseGrain).Assembly).WithReferences();
                    })
                    .ConfigureLogging(logging =>
                    {
                        logging.AddConsole();
                    })
                    .Configure<EndpointOptions>(options =>
                    {
                        options.AdvertisedIPAddress = IPAddress.Loopback;
                    })
                    .AddAzureTableGrainStorageAsDefault(builder => builder.Configure<IOptions<ClusterOptions>>((options, silo) =>
                    {
                        options.ConnectionString = "UseDevelopmentStorage=true";
                        options.UseJson = true;
                        options.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.All;
                        options.UseFullAssemblyNames = true;
                    }))
                    .Build();
            }
            await SiloHost.StartAsync();
        }

        public static async Task StopAsync()
        {
            if (SiloHost != null)
            {
                await SiloHost.StopAsync();
            }
        }
    }
}
