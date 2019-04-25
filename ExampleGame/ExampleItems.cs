using ExampleGame.Items;
using GameEngine;
using System;

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
        public string StartRoomLever { get; private set; }

        public ExampleItems(ExampleGameSourceData gameData)
        {
            CrystalDiviner = gameData.AddItem(new CrystalDiviner());
            {
                gameData.AddDefaultLocationItem(gameData.Locations.Start, CrystalDiviner, 1);
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
            BanquetToSecretWarpedHallKeyhole = gameData.AddItem(new Keyhole(gameData.GameVariables.BanquetToSecretWarpedHallDoorOpen, DullBronzeKey));
            {
                gameData.AddDefaultLocationItem(gameData.Locations.BanquetHall, BanquetToSecretWarpedHallKeyhole, 1);
            }

            // Start room lever
            StartRoomLever = gameData.AddItem(
                new Lever(gameData.GameVariables.StartRoomLever)
                {
                    CustomInteract = new Action<GameState, string>((gameState, fromPosition) =>
                    {
                        if (fromPosition.Equals("off"))
                        {
                            GameEngine.Console.WriteLine($"You move the lever. A small crack forms in the wall and a dull looking key falls out.");
                            gameState.TryAddLocationItemCount(gameData.Locations.Start, gameData.Items.DullBronzeKey, 1, gameData);
                            gameState.SetGameVarValue(gameData.GameVariables.StartRoomLever, "on");
                        }
                        else
                        {
                            GameEngine.Console.WriteLine($"The lever is jammed and won't budge.");
                        }
                    })
                });
            {
                gameData.AddDefaultLocationItem(gameData.Locations.Start, StartRoomLever, 1);
            }
        }
    }
}
