using System;
using ServerEngine;
using ServerEngine.Characters;
using Newtonsoft.Json;

namespace ExampleGameServer.Items
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class CrystalDiviner : Item
    {
        public CrystalDiviner() : base("Crystal Device")
        {
            IsUnique = true;
            IsBound = false;
        }

        public override string GetDescription(int count)
        {
            return "a device made of crystal that has an unknown purpose";
        }

        public override void Grab(int count, Character grabbingCharacter)
        {
            // TODO: Check a character ability or some gamevar to determine this
            bool canGrab = false;

            if (canGrab)
            {
                base.Grab(count, grabbingCharacter);
            }
            else
            {
                grabbingCharacter.SendDescriptiveTextDtoMessage("You attempt to grab the crystal device, but your hands pass through it as though it is just a projection.");
                grabbingCharacter.GetLocation().SendDescriptiveTextDtoMessage($"{grabbingCharacter.Name} tries to grab the crystal device, but their hands pass right through it!", grabbingCharacter);
            }
        }
    }
}
