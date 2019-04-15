using GameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamsAndWhatTheyMean.DragonKittyStrangeItems
{
    public class RoomLight : Item
    {
        private string LightColor { get; set; }
        private bool IsOn { get; set; }

        public RoomLight(string lightColor, bool isOn, int roomLightNumber) : base($"RoomLight[{roomLightNumber}]", "A Room Light")
        {
            LightColor = lightColor;
            IsOn = isOn;
            IsUnique = false;
            IsBound = true;
            IsInteractable = true;
        }

        public override string GetDescription(int count, GameSourceData gameData, GameState gameState)
        {
            if(IsOn)
            {
                return $"A light that is on";
            }
            else if(!IsOn)
            {
                return $"A light that is off";
            }
            return "A strange, flashing light";
        }

        public override void Interact(GameSourceData gameData, GameState gameState)
        {
            if(IsOn)
            {
                IsOn = false;
                GameEngine.Console.WriteLine("You turned the light off.");
            }
            else if (!IsOn)
            {
                IsOn = true;
                GameEngine.Console.WriteLine("You turned the light on.");
            }
        }
    }
}
