using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class Item
    {
        /// <summary>
        /// Name of the item
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Tells if the item is unique (there can be only 1), or not, in which case there could be many.
        /// The game will take care of assuring that only 1 is held by any character at a time.
        /// </summary>
        public bool IsUnique { get; set;}

        public Item(string name)
        {
            Name = name;
        }

        // TODO: Implement TradeSets
        //
        //      TradeSet - A trade set is a list of what item can be traded for other items.
        //      an example tradeset may define
        //          for 1 purple potion you need: 2 bananas and 1 granite pebble
        //          for 1 orange potion you need: 1 purple potion, 1 onyx crystal and 1 orange
        //      This tradeset (which defines how to make 2 different potions) can be attached to a TradePost (see below)
        //      These are different parts of the game like a potion shop or perhaps a potion crafting device.
        //
        //      The following support classes are for TradeSets
        //          class ItemRecipe, 
        //              contains a dictionary<string, int> which is the itemName and itemCount
        //              contains property ItemResult that tells the item name that is produced
        //          class TradeSet, 
        //              contains properties Name (the tradeset name)
        //              contains a list of ItemRecipes

        //      TradeSets are defined in the GameData (not GameState) and once defined remain static/readonly.

        //          Dictionary<string, TradeSet> TradeSets
        //          TradeSet[{TradeSetName}] = TradeSet
        //
        //      Gold / Currency would be considered items as well. This will allow creating tradesets for a standard
        //      shop, and will allow for things like selling ice cream cones in the fire realm for 100 gold each, but
        //      for 5 gold each in the ice realm.
        //
        // TODO: Implement TradePosts
        //      How do we identify a shop at a location and associate a TradeSet to it? We'll call this a TradePost.
        //
        //      A tradepost exists in a location and has an associated TradeSet that it offers
        //      Class TradePost, with property TradeSetName.
        //      A TradePost essentially has infinite items, but can only trade the items specified by the TradeSet.
        //      TradePost locations can be tracked in GameState (think about a vendor with a moving cart)
        //      Dictionary<string, string> TradePostLocations
        //      TradePostLocations[{TradePostName}] = {LocationName}
        //
        // TODO: How do we show a key laying on the floor, but only after the user has opened a trap door in the wall.
        //      Note: the act of opening the trap door would place the item on the floor. Not that the key knew how
        //      to become visible after the trap door was opened. We can keep track of these items in another GameState.
        //
        //      class LocationItemInfo which has LocationName and ItemName properties
        //      Dictionary<LocationItemInfo, int>
        //
        //      This dictionary works the same way as CharacterItems, except it defines items lying on the floor in various locations.
        //
        // TODO: How do we define item use actions
        //      e.g. if the key is used with the door, then x
    }
}
