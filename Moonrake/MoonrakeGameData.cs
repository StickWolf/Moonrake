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

            #endregion

            #region TradePosts

            var IceCream = AddTradeSet("Ice Cream",
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
            var IceCreamShop = AddTradePost("Ice Cream Shop", IceCream);
            DefaultTradePostLocations[IceCreamShop] = MoonRakeLocations.IceCreamShop;

            var BigPepper = AddTradeSet("Big Pepper",
                new ItemRecipe(MoonRakeItems.Burrito,
                    new ItemRecipeIngredient(MoonRakeItems.Money, 20)
                ),
                new ItemRecipe(MoonRakeItems.Taco,
                    new ItemRecipeIngredient(MoonRakeItems.Money, 15)
                ),
                new ItemRecipe(MoonRakeItems.Rice,
                    new ItemRecipeIngredient(MoonRakeItems.Money, 5)
                )
            );
            var BigPepperShop = AddTradePost("Big Pepper Shop", BigPepper);
            DefaultTradePostLocations[BigPepperShop] = MoonRakeLocations.BigPepperShop;

            var TheBurgerDimplomat = AddTradeSet("The Burger Dimplomat",
                new ItemRecipe(MoonRakeItems.Burger,
                    new ItemRecipeIngredient(MoonRakeItems.Money, 15)
                ),
                new ItemRecipe(MoonRakeItems.CheeseBurger,
                    new ItemRecipeIngredient(MoonRakeItems.Money, 25)
                ),
                new ItemRecipe(MoonRakeItems.Soda,
                    new ItemRecipeIngredient(MoonRakeItems.Money, 5)
                ),
                new ItemRecipe(MoonRakeItems.Fries,
                    new ItemRecipeIngredient(MoonRakeItems.Money, 5)
                )
            );
            var TheBurgerDimplomatShop = AddTradePost("The Burger Dimplomat Shop", BigPepper);
            DefaultTradePostLocations[TheBurgerDimplomatShop] = MoonRakeLocations.TheBurgerDimplomatShop;

            #endregion
        }
    }
}
