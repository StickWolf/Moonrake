using ServerEngine;
using ServerEngine.Characters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonKittyServer.DragonKittyStrangeItems
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class RoomLight : Item
    {
        [JsonProperty]
        private string LightColor { get; set; }

        [JsonProperty]
        private bool IsOn { get; set; }

        public RoomLight(string lightColor, bool isOn, int roomLightNumber) : base("room light")
        {
            LightColor = lightColor;
            IsOn = isOn;
            IsUnique = false;
            IsBound = true;
            IsUseableFrom = ItemUseableFrom.Location;
            IsInteractionPrimary = true;
        }

        public override string GetDescription(int count)
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

        public override void Interact(Item otherItem, Character interactingCharacter)
        {
            if(IsOn)
            {
                IsOn = false;
                interactingCharacter.SendDescriptiveTextDtoMessage("You turned the light off.");
            }
            else if (!IsOn)
            {
                IsOn = true;
                interactingCharacter.SendDescriptiveTextDtoMessage("You turned the light on.");
            }
        }
    }
}
