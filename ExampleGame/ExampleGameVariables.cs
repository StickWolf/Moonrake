using GameEngine;

namespace ExampleGame
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
            BanquetElevatorFloor = GameState.CurrentGameState.SetGameVarValue("BanquetElevatorFloor", "1");
            BanquetToSecretWarpedHallDoorOpen = GameState.CurrentGameState.SetGameVarValue("BanquetToSecretWarpedHallDoorOpen", "false");

            ColoredLightAColor = GameState.CurrentGameState.SetGameVarValue("ColoredLightA.Color", "red");
            ColoredLightBColor = GameState.CurrentGameState.SetGameVarValue("ColoredLightB.Color", "teal");

            StartRoomLever = GameState.CurrentGameState.SetGameVarValue("StartRoomLever", "off");
        }
    }
}
