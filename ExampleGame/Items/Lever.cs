using GameEngine;
using Newtonsoft.Json;
using System;

namespace ExampleGame.Items
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class Lever : Item
    {
        [JsonProperty]
        private string GameVariableToggle { get; set; }

        public Action<string> CustomInteract { get; set; }

        public Lever(string gameVariableToggle) : base("Lever")
        {
            GameVariableToggle = gameVariableToggle;
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
                if (CustomInteract != null)
                {
                    CustomInteract(leverPosition);
                }
                else
                {
                    GameEngine.Console.WriteLine($"You move the lever.");
                    GameState.CurrentGameState.SetGameVarValue(GameVariableToggle, leverPosition.Equals("on") ? "off" : "on");
                }
            }
        }
    }
}
