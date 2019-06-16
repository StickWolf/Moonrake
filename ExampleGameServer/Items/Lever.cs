using ServerEngine;
using ServerEngine.Characters;
using Newtonsoft.Json;
using System;
using ServerEngine.GrainSiloAndClient;

namespace ExampleGameServer.Items
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class Lever : Item
    {
        [JsonProperty]
        private string GameVariableToggle { get; set; }

        public Lever(string gameVariableToggle) : base("Lever")
        {
            GameVariableToggle = gameVariableToggle;
            IsUnique = false;
            IsBound = true;
            IsUseableFrom = ItemUseableFrom.Location;
            IsInteractionPrimary = true;
        }

        public override string GetDescription(int count)
        {
            return $"a lever";
        }

        public override void Interact(Item otherItem, Character interactingCharacter)
        {
            string leverPosition = GrainClusterClient.Universe.GetGameVarValue(GameVariableToggle).Result;
            if (leverPosition == null)
            {
                interactingCharacter.SendDescriptiveTextDtoMessage($"The lever appears to be broken and cannot be moved.");
                interactingCharacter.GetLocation().SendDescriptiveTextDtoMessage($"{interactingCharacter.Name} is examining the broken lever.", interactingCharacter);
            }
            else
            {
                StartRoomInteract(leverPosition, interactingCharacter);
            }
        }

        private void StartRoomInteract(string fromPosition, Character interactingCharacter)
        {
            if (fromPosition.Equals("off"))
            {
                var gameData = ExampleGameSourceData.Current();
                interactingCharacter.SendDescriptiveTextDtoMessage($"You move the lever. A small crack forms in the wall and a dull looking key falls out.");
                interactingCharacter.GetLocation().SendDescriptiveTextDtoMessage($"{interactingCharacter.Name} moves the lever and a key falls out of the wall!.", interactingCharacter);
                GameState.CurrentGameState.TryAddLocationItemCount(gameData.EgLocations.Start, gameData.EgItems.DullBronzeKey, 1);

                // TODO: try to move away from game variables and just use properties of items/locations/etc directly
                GrainClusterClient.Universe.SetGameVarValue(GameVariableToggle, "on").Wait();
            }
            else
            {
                interactingCharacter.SendDescriptiveTextDtoMessage($"The lever is jammed and won't budge.");
                interactingCharacter.GetLocation().SendDescriptiveTextDtoMessage($"{interactingCharacter.Name} is trying to move the lever, but it won't budge.", interactingCharacter);
            }
        }
    }
}
