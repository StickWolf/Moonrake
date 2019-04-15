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

        public RoomLight(string lightColor, bool isOn, int roomLightNumber) : base($"RoomLight[{roomLightNumber}]", "room light")
        {
            LightColor = lightColor;
            IsOn = isOn;
            IsUnique = false;
            IsBound = true;
            IsInteractable = true;
        }

        public string GetDescription(int count, GameSourceData gameData, GameState gameState)
        {
            if(IsOn)
            {
                return $"a light that is on";
            }
            else if(!IsOn)
            {
                return $"a light that is off";
            }
            return "strange, flashing light";
        }

        public void Interact(GameSourceData gameData, GameState gameState)
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
