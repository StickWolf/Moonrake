using ExampleGame.Items;
using GameEngine;

namespace ExampleGame
{
    public class ExampleItems
    {
        public string BanquetToSecretWarpedHallKeyhole { get; private set; }
        public string ColoredLightA { get; private set; }
        public string ColoredLightB { get; private set; }
        public string ColoredLightSwitchA { get; private set; }
        public string ColoredLightSwitchB { get; private set; }
        public string CrystalDiviner { get; private set; }
        public string DullBronzeKey { get; private set; }

        public ExampleItems(ExampleGameSourceData gameData)
        {
            CrystalDiviner = gameData.AddItem(new CrystalDiviner());
            {
                gameData.AddDefaultCharacterItem(gameData.Characters.Player, CrystalDiviner, 1);
            }

            // Light and light switch A in Start
            ColoredLightA = gameData.AddItem(new ColoredLight(gameData.GameVariables.ColoredLightAColor));
            {
                gameData.AddDefaultLocationItem(gameData.Locations.Start, ColoredLightA, 1);
            }
            ColoredLightSwitchA = gameData.AddItem(new ColoredLightSwitch(gameData.GameVariables.ColoredLightAColor));
            {
                gameData.AddDefaultLocationItem(gameData.Locations.Start, ColoredLightSwitchA, 1);
            }

            // Light and light switch B in the Banquet Elevator
            ColoredLightB = gameData.AddItem(new ColoredLight(gameData.GameVariables.ColoredLightBColor));
            {
                gameData.AddDefaultLocationItem(gameData.Locations.BanquetElevator, ColoredLightB, 1);
            }
            ColoredLightSwitchB = gameData.AddItem(new ColoredLightSwitch(gameData.GameVariables.ColoredLightBColor));
            {
                gameData.AddDefaultLocationItem(gameData.Locations.BanquetElevator, ColoredLightSwitchB, 1);
            }

            // Bronze key and keyhole pair
            DullBronzeKey = gameData.AddItem(new Item("DullBronzeKey", "Dull Bronze Key") { IsUnique = true, IsInteractable = true });
            {
                gameData.AddDefaultLocationItem(gameData.Locations.Start, DullBronzeKey, 1);
            }
            BanquetToSecretWarpedHallKeyhole = gameData.AddItem(new Keyhole(gameData.GameVariables.BanquetToSecretWarpedHallDoorOpen, DullBronzeKey));
            {
                gameData.AddDefaultLocationItem(gameData.Locations.BanquetHall, BanquetToSecretWarpedHallKeyhole, 1);
            }
        }
    }
}
