using GameEngine;
using GameEngine.Locations;

namespace Moonrake
{
    public class MoonrakeGameData
    {
        public MoonRakePlayersAndCharacters MoonRakePlayers { get; private set; } = new MoonRakePlayersAndCharacters();
        public MoonRakeGameVariables MoonRakeGameVariables { get; private set; } = new MoonRakeGameVariables();
        public MoonRakeItems MoonRakeItems { get; private set; } = new MoonRakeItems();
        public MoonRakeLocations MoonRakeLocations { get; private set; } = new MoonRakeLocations();

        public void NewWorld()
        {
            MoonRakeGameVariables.NewWorld(this);
            MoonRakeLocations.NewWorld(this);
            MoonRakePlayers.NewWorld(this);
            MoonRakeItems.NewWorld(this);

            GameState.CurrentGameState.GameIntroductionText = "Once, there were three ancient instruments:" +
                " The Harp, Piano, and the Drum." +
                " Inside of each instrument there was a magical gem:" +
                " A ruby, sapphire, and a diamond." +
                " When the gems are merged, it will the create an ancient weapon:" +
                " The Moonrake." +
                "And also...... wanna go inside? Play somethin' or somethin'?";

            //GameState.CurrentGameState.TryAddCharacterItemCount(MoonRakePlayers.Player, MoonRakeItems.Money, 35);

            #region TradeSets

            #endregion

            #region TradePosts

            var IceCream = GameState.CurrentGameState.AddTradeSet("Ice Cream",
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
            var IceCreamShop = GameState.CurrentGameState.AddTradePost(MoonRakeLocations.IceCreamShop, "Ice Cream Shop", IceCream);

            var BigPepper = GameState.CurrentGameState.AddTradeSet("Big Pepper",
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
            var BigPepperShop = GameState.CurrentGameState.AddTradePost(MoonRakeLocations.BigPepperShop, "Big Pepper Shop", BigPepper);

            var TheBurgerDimplomat = GameState.CurrentGameState.AddTradeSet("The Burger Dimplomat",
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
            var TheBurgerDimplomatShop = GameState.CurrentGameState.AddTradePost(MoonRakeLocations.TheBurgerDimplomatShop, "The Burger Dimplomat Shop", BigPepper);

            #endregion

            GameState.CurrentGameState.Custom = this;
        }

        public static MoonrakeGameData Current()
        {
            return GameState.CurrentGameState.Custom as MoonrakeGameData;
        }
    }
}
