using ExampleGameServer.Items;
using ServerEngine;
using ServerEngine.GrainSiloAndClient;
using System;

namespace ExampleGameServer
{
    public class ExampleItems
    {
        public Guid BanquetToSecretWarpedHallKeyhole { get; private set; }
        public Guid ColoredLightA { get; private set; }
        public Guid ColoredLightB { get; private set; }
        public Guid ColoredLightSwitchA { get; private set; }
        public Guid ColoredLightSwitchB { get; private set; }
        public Guid CrystalDiviner { get; private set; }
        public Guid DullBronzeKey { get; set; }
        public Guid StartRoomLever { get; private set; }
        public Guid HealingPotion { get; private set; }

        public void NewWorld(ExampleGameSourceData gameData)
        {
            CrystalDiviner = GrainClusterClient.Universe.AddItem(new CrystalDiviner()).Result;
            {
                GrainClusterClient.Universe.TryAddLocationItemCount(gameData.EgLocations.Start, CrystalDiviner, 1);
            }

            // Light and light switch A in Start
            ColoredLightA = GrainClusterClient.Universe.AddItem(new ColoredLight(gameData.GameVariables.ColoredLightAColor)).Result;
            {
                GrainClusterClient.Universe.TryAddLocationItemCount(gameData.EgLocations.Start, ColoredLightA, 1);
            }
            ColoredLightSwitchA = GrainClusterClient.Universe.AddItem(new ColoredLightSwitch(gameData.GameVariables.ColoredLightAColor)).Result;
            {
                GrainClusterClient.Universe.TryAddLocationItemCount(gameData.EgLocations.Start, ColoredLightSwitchA, 1);
            }

            // Light and light switch B in the Banquet Elevator
            ColoredLightB = GrainClusterClient.Universe.AddItem(new ColoredLight(gameData.GameVariables.ColoredLightBColor)).Result;
            {
                GrainClusterClient.Universe.TryAddLocationItemCount(gameData.EgLocations.BanquetElevator, ColoredLightB, 1);
            }
            ColoredLightSwitchB = GrainClusterClient.Universe.AddItem(new ColoredLightSwitch(gameData.GameVariables.ColoredLightBColor)).Result;
            {
                GrainClusterClient.Universe.TryAddLocationItemCount(gameData.EgLocations.BanquetElevator, ColoredLightSwitchB, 1);
            }

            // Bronze key and keyhole pair
            DullBronzeKey = GrainClusterClient.Universe.AddItem(new Item("Dull Bronze Key") { IsUnique = true, IsUseableFrom = ItemUseableFrom.Inventory }).Result;
            BanquetToSecretWarpedHallKeyhole = GrainClusterClient.Universe.AddItem(new Keyhole(gameData.GameVariables.BanquetToSecretWarpedHallDoorOpen, DullBronzeKey)).Result;
            {
                GrainClusterClient.Universe.TryAddLocationItemCount(gameData.EgLocations.BanquetHall, BanquetToSecretWarpedHallKeyhole, 1);
            }

            // Start room lever
            StartRoomLever = GrainClusterClient.Universe.AddItem(new Lever(gameData.GameVariables.StartRoomLever)).Result;
            {
                GrainClusterClient.Universe.TryAddLocationItemCount(gameData.EgLocations.Start, StartRoomLever, 1);
            }

            // Healing potion
            HealingPotion = GrainClusterClient.Universe.AddItem(new HealingPotion(5)).Result;
            {
                GrainClusterClient.Universe.TryAddLocationItemCount(gameData.EgLocations.Start, HealingPotion, 25);
            }
        }
    }
}
