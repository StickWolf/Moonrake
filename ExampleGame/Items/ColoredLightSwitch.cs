using GameEngine;
using GameEngine.Characters;
using Newtonsoft.Json;
using System;

namespace ExampleGame.Items
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class ColoredLightSwitch : Item
    {
        [JsonProperty]
        private string GameVariableColor { get; set; }

        public ColoredLightSwitch(string gameVariableColor) : base("Colored light switch")
        {
            GameVariableColor = gameVariableColor;
            IsUnique = false;
            IsBound = true;
            IsUseableFrom = ItemUseableFrom.Location;
            IsInteractionPrimary = true;
        }

        public override string GetDescription(int count)
        {
            return $"a light switch";
        }

        public override void Interact(Item otherItem, Character interactingCharacter)
        {
            string lightColor = GameState.CurrentGameState.GetGameVarValue(GameVariableColor);
            if (lightColor == null)
            {
                interactingCharacter.SendMessage($"You flip the light switch, but nothing appears to happen.");
                interactingCharacter.GetLocation().SendMessage($"{interactingCharacter.Name} flips the light switch.", interactingCharacter);
            }
            else
            {
                var newColor = "blue";
                switch (lightColor)
                {
                    case "red":
                        newColor = "green";
                        break;
                    case "green":
                        newColor = "red";
                        break;
                    case "dark purple":
                        newColor = "teal";
                        break;
                    case "teal":
                        newColor = "dark purple";
                        break;
                }
                GameState.CurrentGameState.SetGameVarValue(GameVariableColor, newColor);
                interactingCharacter.SendMessage($"You flip the light switch and the {lightColor} light now begins to glow {newColor}.");
                interactingCharacter.GetLocation().SendMessage($"{interactingCharacter.Name} flips the light switch.", interactingCharacter);
            }
        }
    }
}
