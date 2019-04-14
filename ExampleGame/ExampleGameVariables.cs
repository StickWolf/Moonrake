namespace ExampleGame
{
    public class ExampleGameVariables
    {
        public string BanquetElevatorFloor { get; private set; }
        public string BanquetToSecretWarpedHallDoorOpen { get; private set; }

        public string ColoredLightAColor { get; private set; }
        public string ColoredLightBColor { get; private set; }

        public ExampleGameVariables(ExampleGameSourceData gameData)
        {
            BanquetElevatorFloor = gameData.AddDefaultGameVar("BanquetElevatorFloor", "1");
            BanquetToSecretWarpedHallDoorOpen = gameData.AddDefaultGameVar("BanquetToSecretWarpedHallDoorOpen", "false");

            ColoredLightAColor = gameData.AddDefaultGameVar("ColoredLightA.Color", "red");
            ColoredLightBColor = gameData.AddDefaultGameVar("ColoredLightB.Color", "teal");
        }
    }
}
