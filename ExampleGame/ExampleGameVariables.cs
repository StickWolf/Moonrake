namespace ExampleGame
{
    public class ExampleGameVariables
    {
        public string BanquetElevatorFloor { get; private set; }
        public string BanquetSecretHallOpen { get; private set; }

        public ExampleGameVariables(ExampleGameSourceData gameData)
        {
            BanquetElevatorFloor = gameData.AddDefaultGameVar("BanquetElevatorFloor", "1");
            BanquetSecretHallOpen = gameData.AddDefaultGameVar("BanquetSecretHallOpen", "false");
        }
    }
}
