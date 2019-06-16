using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using ServerEngine.GrainInterfaces;
using ServerEngine.Grains;
using System.Threading.Tasks;

namespace ServerEngine.GrainSiloAndClient
{
    public static class GrainClusterClient
    {
        public static IClusterClient ClusterClient { get; set; }

        private static string GameUniverseName { get; set; }

        public static IGameUniverseGrain Universe
        {
            get
            {
                var gameUniverseGrain = ClusterClient.GetGrain<IGameUniverseGrain>(GameUniverseName);
                return gameUniverseGrain;
            }
        }

        public static void SetGameUniverseName(string name)
        {
            GameUniverseName = name;
        }

        public static async Task StartAsync()
        {
            if (ClusterClient == null)
            {
                ClusterClient = new ClientBuilder()
                    .UseLocalhostClustering()
                    .Configure<ClusterOptions>(options =>
                    {
                        options.ClusterId = "GameState";
                        options.ServiceId = "ServerEngine";
                    })
                    .ConfigureApplicationParts(parts =>
                    {
                        parts.AddApplicationPart(typeof(IItemGrain).Assembly).WithReferences();
                    })
                    .ConfigureLogging(logging =>
                    {
                        logging.AddConsole();
                    })
                    .Build();
            }
            await ClusterClient.Connect();
        }

        public static async Task StopAsync()
        {
            if (ClusterClient != null)
            {
                await ClusterClient.Close();
            }
        }
    }
}
