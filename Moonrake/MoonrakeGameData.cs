using GameEngine;
using GameEngine.Locations;

namespace Moonrake
{
    public class MoonrakeGameData : GameSourceData
    {

        public PortalOpenGameVarRule PortalOpenGameVarRule { get; private set; }
        public Character Character { get; private set; }
        public Location Location { get; private set; }
        public Item Item { get; private set; }

        public MoonrakeGameData(Character character,Location location, Item item, PortalOpenGameVarRule portalOpenGameVarRule)
        {
            DefaultPlayerName = "Eric";

            GameIntroductionText = "Once, there were three ancient instruments:" +
                " The Harp, Piano, and the Drum." +
                " Inside of each instrument there was a magical gem:" +
                " A ruby, sapphire, and a diamond." +
                " When the gems are merged, it will the create an ancient weapon:" +
                " The Moonrake." +
                " Hello, Welcome to Moonrake, a text adventure game.";

            #region Characters

            var charPlayer = AddCharacter(new Character("Player", 50, 100));

            #endregion

            #region GameVars

            var gvIceCreamShopDoor = AddDefaultGameVar("IceCreamShopDoor", "closed");

            #endregion

            #region Locations

            var locTreeHouse = AddLocation(new Location("Tree House", "what looks like a small living space.",
                "You are in a tree house. You see a bed covered with a blanket, curtains on the windows and a fridge full of soda. " +
                "White wooden planks smashed together make up the walls and the floor. " +
                "You have a very certain feeling the tree house is going to break. "
                ));
            AddDefaultCharacterLocation(charPlayer, locTreeHouse);

            var locField = AddLocation(new Location("Field", "a beautifuly trimmed field of lime-green grass and flowers.",
                "You are in an open field, flowers and lime-green grass surround you. A number of benches provide a resting place for visitors. " +
                "There is a small tree house here with a rope ladder and windows. " +
                "In the center of the field you see an ice cream shop with a big sign on the door. There are tables with umbrellas around the shop."
                ));

            var locIceCreamShop = AddLocation(new Location("Ice Cream Shop", "an ice cream shop full of people at tables.",
                "You see many containers of candy, an ice cream machine and many people sitting in chairs around tables. " +
                "A cashier resides in the central area of the shop. " +
                "The store has checkered walls and a floor decorated with many pictures of candies."
                ));

            #endregion

            #region Portals

            // Tree house <--> Field
            AddPortal(
                new PortalAlwaysOpenRule(locTreeHouse, locField, "Through the door shaped space you see"),

                new PortalAlwaysOpenRule(locField, locTreeHouse, "Through the tree house door shaped space you see")
                );

            // Field <--> Ice cream shop
            AddPortal(
                new PortalOpenGameVarRule(locField, locIceCreamShop,
                "Through the Ice Cream Shop's medium wooden door you see", gvIceCreamShopDoor, "open"),
                new PortalAlwaysClosedRule(locField, null,
                "You see a closed medium sized wooden door under the big sign on the Ice Cream Shop."),

                new PortalOpenGameVarRule(locIceCreamShop, locField,
                "Through the medium wooden door you see", gvIceCreamShopDoor, "open"),
                new PortalAlwaysClosedRule(locIceCreamShop, null,
                "You see a closed medium sized wooden door inside the Ice Cream Shop.")
                );

            #endregion

            #region Items

            var itemWaterEye = AddItem(new Item("Water Eye") { IsUnique = true });
            var itemMoney = AddItem(new Item("Money") { IsUnique = false });
            var itemChocolateIceCream = AddItem(new Item("Chocolate Ice Cream") { IsUnique = false });
            var itemStrawberryIceCream = AddItem(new Item("Strawberry Ice Cream") { IsUnique = false });
            var itemVanillaIceCream = AddItem(new Item("Vanilla Ice Cream") { IsUnique = false });

            #endregion

            #region Default Character Items

            AddDefaultCharacterItem(charPlayer, itemMoney, 35);

            #endregion

            #region TradeSets
            var tsIceCream = AddTradeSet("Ice Cream",
                new ItemRecipe(itemChocolateIceCream,
                    new ItemRecipeIngredient(itemMoney, 10)
                ),
                new ItemRecipe(itemVanillaIceCream,
                    new ItemRecipeIngredient(itemMoney, 15)
                ),
                new ItemRecipe(itemStrawberryIceCream,
                    new ItemRecipeIngredient(itemMoney, 15)
                )
            );
            #endregion

            #region TradePosts

            var tpIceCreamShop = AddTradePost("Ice Cream Shop", tsIceCream);
            DefaultTradePostLocations[tpIceCreamShop] = locIceCreamShop;

            #endregion

            Character = character;
            Location = location;
            Item = item;
            PortalOpenGameVarRule = portalOpenGameVarRule;
        }
    }
}
