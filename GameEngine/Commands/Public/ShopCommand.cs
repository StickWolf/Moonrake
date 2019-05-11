using GameEngine.Characters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameEngine.Commands.Public
{
    public class ShopCommand : ICommand
    {
        public List<string> ActivatingWords => new List<string>() { "shop" };

        public void Execute(List<string> extraWords, Character shoppingCharacter)
        {
            var shoppingCharacterLocation = GameState.CurrentGameState.GetCharacterLocation(shoppingCharacter.TrackingId);

            //  2. Look in the game data for any tradeposts that are currently at this location
            var allTradePostsInPlayersLocation = GameState.CurrentGameState.GetTradePostsAtLocation(shoppingCharacterLocation.TrackingId);
            
            //  3. If there are no tradeposts here then mention that and return.
            if (allTradePostsInPlayersLocation.Count == 0)
            {
                shoppingCharacter.SendMessage("There are no availible shops in your area.");
                shoppingCharacterLocation.SendMessage($"{shoppingCharacter.Name} wants to go shopping!", shoppingCharacter);
                return;
            }

            //  4. If there is just 1 tradepost then automatically choose that one
            TradePost chosenTradePost;
            if (allTradePostsInPlayersLocation.Count == 1)
            {
                chosenTradePost = allTradePostsInPlayersLocation[0];
            }
            // Don't prompt NPCs who are running actions
            else if (!shoppingCharacter.IsPlayerCharacter())
            {
                return;
            }
            else
            {
                // if there are multiple then give the user a choice on which one they want to shop at.
                var choices = allTradePostsInPlayersLocation
                    .ToDictionary(t => t, t => t.Name);
                chosenTradePost = shoppingCharacter.Choose("Where would you like to shop?", choices, includeCancel: false);
            }
            shoppingCharacter.SendMessage($"Welcome, you will be shopping at {chosenTradePost.Name}");

            //  5. One they have chosen a tradepost, look through each tradeset the tradepost offers
            //     and list out each item and the cost of each item in a menu that the user can choose
            //     from, but also can exit from without buying anything.




            //
            //     Note: for each item listed the program should check to see if it is a unique item and
            //     if so, should check to make sure that item is at no other location before listing it.
            //     If there isn't a function to do this check, then make one.
            //
            //     The items in the list should be displayed as follows
            //     e.g.
            //            1. [Available] Apple                  [AppleSeed(4/1),Water(15/3)]
            //            2. [Available] Fruitcake              [Apple(3/1),Banana(2/1)]
            //            3. [Available] Pancake Buffet         [Fruitcake(1/1),Pancake(34/5),Strawberry(57/10),Syrup(5/2)]
            //            4. [Unavailable] Golden Cupcake       [Gold Bar(0/1),Metalic Refining Powder(0/1),Cupcake(15/1)]
            //            5. [Unavailable] Strawberry IceCream  [Money(0/15)]
            //            6. Exit
            //
            //     The Apple can be bought because it costs 1 AppleSeed and 3 Water, the player as 4 AppleSeeds and 15 Waters.
            //     The Golden Cupcake can not be bought because the player does not have the required items.
            //
            //     If the same item appears in multiple tradesets that are linked to a trade post then all the items should be listed
            //     And the user should be allowed to choose which version they want to buy. This routine should keep track of which
            //     version they are choosing so it can deduct the right ingredients.

            //  6. When the user selects an item to buy, the game should check to see if they have the right items and then
            //     modify their inventory if so. Otherwise it should tell them they can't buy it.
            //  7. After purchasing the current shop list should be re-caclulated and shown again.
            //  8. If the user exits the shop then the command ends and they are back at the location the were previously.

            // TODO: determine how bound items will be presented in shops
        }
    }
}
