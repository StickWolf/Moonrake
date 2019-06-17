using ServerEngine;
using ServerEngine.GrainSiloAndClient;

namespace ExampleGameServer
{
    public class ExampleGameVariables
    {
        public string BanquetElevatorFloor { get; private set; }
        public string BanquetToSecretWarpedHallDoorOpen { get; private set; }

        public string ColoredLightAColor { get; private set; }
        public string ColoredLightBColor { get; private set; }

        public string StartRoomLever { get; private set; }

        public void NewWorld(ExampleGameSourceData gameData)
        {
            BanquetElevatorFloor = GrainClusterClient.Universe.SetGameVarValue("BanquetElevatorFloor", "1").Result;
            BanquetToSecretWarpedHallDoorOpen = GrainClusterClient.Universe.SetGameVarValue("BanquetToSecretWarpedHallDoorOpen", "false").Result;

            ColoredLightAColor = GrainClusterClient.Universe.SetGameVarValue("ColoredLightA.Color", "red").Result;
            ColoredLightBColor = GrainClusterClient.Universe.SetGameVarValue("ColoredLightB.Color", "teal").Result;

            StartRoomLever = GrainClusterClient.Universe.SetGameVarValue("StartRoomLever", "off").Result;
        }
    }
}
