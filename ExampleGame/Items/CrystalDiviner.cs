using System;
using GameEngine;

namespace ExampleGame.Items
{
    public class CrystalDiviner : Item
    {
        public CrystalDiviner() : base("Crystal Device")
        {
            IsUnique = true;
            IsBound = false;
        }

        public override string GetDescription(int count)
        {
            return "a device made of crystal that has an unknown purpose"; // TODO: write asserts that check the formatting of these, check for init caps and . ending sentences that shouldn't be present
        }

        public override void Grab(int count, Guid grabbingCharacterTrackingId)
        {
            // TODO: Check a character ability or some gamevar to determine this
            bool canGrab = false;

            if (canGrab)
            {
                base.Grab(count, grabbingCharacterTrackingId);
            }
            else
            {
                GameEngine.Console.WriteLine("You attempt to grab the crystal device, but your hands pass through it as though it is just a projection.");
            }
        }
    }
}
