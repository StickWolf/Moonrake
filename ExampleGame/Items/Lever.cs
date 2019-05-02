using GameEngine;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ExampleGame.Items
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class Lever : Item
    {
        [JsonProperty]
        private string GameVariableToggle { get; set; }

        [JsonProperty]
        private Guid StartLocation { get; set; } // TODO: I don't want to pass startlocation and the key explicitly, but it needs to be something that survives serialization

        [JsonProperty]
        private Guid DullBronzeKey { get; set; }

        public Lever(string gameVariableToggle, Guid startLocation, Guid dullBronzeKey) : base("Lever")
        {
            GameVariableToggle = gameVariableToggle;
            StartLocation = startLocation;
            DullBronzeKey = dullBronzeKey;
            IsUnique = false;
            IsBound = true;
            IsInteractable = true;
        }

        public override string GetDescription(int count)
        {
            return $"a lever";
        }

        public override void Interact(Item otherItem)
        {
            string leverPosition = GameState.CurrentGameState.GetGameVarValue(GameVariableToggle);
            if (leverPosition == null)
            {
                GameEngine.Console.WriteLine($"The lever appears to be broken and cannot be moved.");
            }
            else
            {
                StartRoomInteract(leverPosition);
            }
        }

        private void StartRoomInteract(string fromPosition)
        {
            if (fromPosition.Equals("off"))
            {
                GameEngine.Console.WriteLine($"You move the lever. A small crack forms in the wall and a dull looking key falls out.");
                GameState.CurrentGameState.TryAddLocationItemCount(StartLocation, DullBronzeKey, 1);

                // TODO: try to move away from game variables and just use properties of items/locations/etc directly
                GameState.CurrentGameState.SetGameVarValue(GameVariableToggle, "on");
            }
            else
            {
                GameEngine.Console.WriteLine($"The lever is jammed and won't budge.");
            }
        }


    }
}
