using GameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamsAndWhatTheyMean.DragonKittyStrangeItems
{
    public class LightMine : Item
    {
        private string LightColor { get; set; }
        private bool IsOn { get; set; }

        public LightMine(string lightColor, bool isOn) : base("LightMine", "A Light Mine")
        {
            LightColor = lightColor;
            IsOn = isOn;
            IsUnique = false;
            IsBound = false;
            IsInteractable = true;
        }

        public override string GetDescription(int count, GameSourceData gameData, GameState gameState)
        {
            if (IsOn)
            {
                return $"A light mine that is on";
            }
            else if (!IsOn)
            {
                return $"A light mine that is off";
            }
            return "A strange, flashing light";
        }

        public override void Interact(GameSourceData gameData, GameState gameState)
        {
            if (IsOn)
            {
                IsOn = false;
                GameEngine.Console.WriteLine("You turned the light mine off.");
            }
            else if (!IsOn)
            {
                IsOn = true;
                GameEngine.Console.WriteLine("You turned the light mine on.");
            }
        }
    }
}
