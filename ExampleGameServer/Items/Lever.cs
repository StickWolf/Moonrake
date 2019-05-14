using ServerEngine;
using ServerEngine.Characters;
using Newtonsoft.Json;
using System;

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
            string leverPosition = GameState.CurrentGameState.GetGameVarValue(GameVariableToggle);
            if (leverPosition == null)
            {
                interactingCharacter.SendMessage($"The lever appears to be broken and cannot be moved.");
                interactingCharacter.GetLocation().SendMessage($"{interactingCharacter.Name} is examining the broken lever.", interactingCharacter);
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
                interactingCharacter.SendMessage($"You move the lever. A small crack forms in the wall and a dull looking key falls out.");
                interactingCharacter.GetLocation().SendMessage($"{interactingCharacter.Name} moves the lever and a key falls out of the wall!.", interactingCharacter);
                GameState.CurrentGameState.TryAddLocationItemCount(gameData.EgLocations.Start, gameData.EgItems.DullBronzeKey, 1);

                // TODO: try to move away from game variables and just use properties of items/locations/etc directly
                GameState.CurrentGameState.SetGameVarValue(GameVariableToggle, "on");
            }
            else
            {
                interactingCharacter.SendMessage($"The lever is jammed and won't budge.");
                interactingCharacter.GetLocation().SendMessage($"{interactingCharacter.Name} is trying to move the lever, but it won't budge.", interactingCharacter);
            }
        }
    }
}
