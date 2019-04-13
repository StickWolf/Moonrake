using GameEngine;
using GameEngine.Locations;

namespace Moonrake
{
    public class MoonrakeGameData : GameSourceData
    {

        public MoonRakeCharacters MoonRakeCharacters { get; private set; }
        public MoonRakeGameVariables MoonRakeGameVariables { get; private set; }
        public MoonRakeItems MoonRakeItems { get; private set; }
        public MoonRakeLocations MoonRakeLocations { get; private set; }

        public MoonrakeGameData()
        {
            MoonRakeCharacters = new MoonRakeCharacters(this);
            MoonRakeGameVariables = new MoonRakeGameVariables(this);
            MoonRakeItems = new MoonRakeItems(this);
            MoonRakeLocations = new MoonRakeLocations(this);

            DefaultPlayerName = "Eric";

            GameIntroductionText = "Once, there were three ancient instruments:" +
                " The Harp, Piano, and the Drum." +
                " Inside of each instrument there was a magical gem:" +
                " A ruby, sapphire, and a diamond." +
                " When the gems are merged, it will the create an ancient weapon:" +
                " The Moonrake." +
                " Hello, Welcome to Moonrake, a text adventure game.";

            AddDefaultCharacterLocation(MoonRakeCharacters.Player, MoonRakeLocations.TreeHouse);

            AddDefaultCharacterItem(MoonRakeCharacters.Player, MoonRakeItems.Money, 35);

            #region TradeSets
            var tsIceCream = AddTradeSet("Ice Cream",
                new ItemRecipe(MoonRakeItems.ChocolateIceCream,
                    new ItemRecipeIngredient(MoonRakeItems.Money, 10)
                ),
                new ItemRecipe(MoonRakeItems.VanillaIceCream,
                    new ItemRecipeIngredient(MoonRakeItems.Money, 15)
                ),
                new ItemRecipe(MoonRakeItems.StrawberryIceCream,
                    new ItemRecipeIngredient(MoonRakeItems.Money, 15)
                )
            );
            #endregion

            #region TradePosts

            var tpIceCreamShop = AddTradePost("Ice Cream Shop", tsIceCream);
            DefaultTradePostLocations[tpIceCreamShop] = MoonRakeLocations.IceCreamShop;

            #endregion
        }
    }
}
