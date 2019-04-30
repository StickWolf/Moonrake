using ExampleGame.Items;
using GameEngine;
using System;

namespace ExampleGame
{
    public class ExampleItems
    {
        public Guid BanquetToSecretWarpedHallKeyhole { get; private set; }
        public Guid ColoredLightA { get; private set; }
        public Guid ColoredLightB { get; private set; }
        public Guid ColoredLightSwitchA { get; private set; }
        public Guid ColoredLightSwitchB { get; private set; }
        public Guid CrystalDiviner { get; private set; }
        public Guid DullBronzeKey { get; private set; }
        public Guid StartRoomLever { get; private set; }

        public ExampleItems(ExampleGameSourceData gameData)
        {
            CrystalDiviner = GameState.CurrentGameState.AddItem(new CrystalDiviner());
            {
                GameState.CurrentGameState.TryAddLocationItemCount(gameData.EgLocations.Start, CrystalDiviner, 1);
            }

            // Light and light switch A in Start
            ColoredLightA = GameState.CurrentGameState.AddItem(new ColoredLight(gameData.GameVariables.ColoredLightAColor));
            {
                GameState.CurrentGameState.TryAddLocationItemCount(gameData.EgLocations.Start, ColoredLightA, 1);
            }
            ColoredLightSwitchA = GameState.CurrentGameState.AddItem(new ColoredLightSwitch(gameData.GameVariables.ColoredLightAColor));
            {
                GameState.CurrentGameState.TryAddLocationItemCount(gameData.EgLocations.Start, ColoredLightSwitchA, 1);
            }

            // Light and light switch B in the Banquet Elevator
            ColoredLightB = GameState.CurrentGameState.AddItem(new ColoredLight(gameData.GameVariables.ColoredLightBColor));
            {
                GameState.CurrentGameState.TryAddLocationItemCount(gameData.EgLocations.BanquetElevator, ColoredLightB, 1);
            }
            ColoredLightSwitchB = GameState.CurrentGameState.AddItem(new ColoredLightSwitch(gameData.GameVariables.ColoredLightBColor));
            {
                GameState.CurrentGameState.TryAddLocationItemCount(gameData.EgLocations.BanquetElevator, ColoredLightSwitchB, 1);
            }

            // Bronze key and keyhole pair
            DullBronzeKey = GameState.CurrentGameState.AddItem(new Item("Dull Bronze Key") { IsUnique = true, IsInteractable = true });
            BanquetToSecretWarpedHallKeyhole = GameState.CurrentGameState.AddItem(new Keyhole(gameData.GameVariables.BanquetToSecretWarpedHallDoorOpen, DullBronzeKey));
            {
                GameState.CurrentGameState.TryAddLocationItemCount(gameData.EgLocations.BanquetHall, BanquetToSecretWarpedHallKeyhole, 1);
            }

            // Start room lever
            StartRoomLever = GameState.CurrentGameState.AddItem(
                new Lever(gameData.GameVariables.StartRoomLever)
                {
                    CustomInteract = new Action<string>((fromPosition) =>
                    {
                        if (fromPosition.Equals("off"))
                        {
                            GameEngine.Console.WriteLine($"You move the lever. A small crack forms in the wall and a dull looking key falls out.");
                            GameState.CurrentGameState.TryAddLocationItemCount(gameData.EgLocations.Start, gameData.EgItems.DullBronzeKey, 1);
                            GameState.CurrentGameState.SetGameVarValue(gameData.GameVariables.StartRoomLever, "on");
                        }
                        else
                        {
                            GameEngine.Console.WriteLine($"The lever is jammed and won't budge.");
                        }
                    })
                });
            {
                GameState.CurrentGameState.TryAddLocationItemCount(gameData.EgLocations.Start, StartRoomLever, 1);
            }
        }
    }
}
