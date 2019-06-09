using Microsoft.Extensions.Logging;
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
                        parts.AddApplicationPart(typeof(ItemGrain).Assembly).WithReferences();
                    })
                    .ConfigureLogging(logging =>
                    {
                        logging.AddConsole();
                    })
                    .Configure<EndpointOptions>(options =>
                    {
                        options.AdvertisedIPAddress = IPAddress.Loopback;
                    })
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
