using GameEngine;

namespace ExampleGame.Items
{
    public class CrystalDiviner : Item
    {
        public CrystalDiviner() : base("CrystalDiviner", "Crystal Diviner")
        {
            IsUnique = true;
            IsBound = true;
        }

        public override string GetDescription(int count, GameState gameState)
        {
            return "A device made of crystal that has an unknown purpose.";
        }
    }
}
