using System;

namespace GameEngine
{
    public class Item
    {
        /// <summary>
        /// Name used to track the item in gamestate, this value is not displayed
        /// </summary>
        public string TrackingName { get; private set; }

        /// <summary>
        /// Name that is displayed in the game
        /// </summary>
        public string DisplayName { get; private set; }

        /// <summary>
        /// Tells if the item is unique (there can be only 1), or not, in which case there could be many.
        /// The game will take care of assuring that only 1 is held by any character at a time.
        /// </summary>
        public bool IsUnique { get; set;}

        /// <summary>
        /// Indicates if an item can be picked up, moved locations and dropped or if it is stuck to where it is.
        /// </summary>
        public bool IsBound { get; set; }

        /// <summary>
        /// Indicates if an item can be interacted with.
        /// </summary>
        public bool IsInteractable { get; set; }

        /// <summary>
        /// Indicates if the item is visible or not.
        /// Only visible items can be picked up or seen.
        /// </summary>
        public bool IsVisible { get; set; } = true;

        public Item(string trackingName, string displayName)
        {
            TrackingName = trackingName;
            DisplayName = displayName;
        }

        /// <summary>
        /// Gets a description of what the item looks like.
        /// </summary>
        /// <param name="count">The number of items that are present</param>
        /// <param name="gameData">The gamedata for the game</param>
        /// <param name="currentGameState">The current game state</param>
        /// <returns>A description</returns>
        public virtual string GetDescription(int count, GameState gameState)
        {
            if (count == 1)
            {
                return $"a {DisplayName}";
            }

            return $"{count} {DisplayName}s";
        }

        /// <summary>
        /// Interacts with an item
        /// </summary>
        /// <param name="gameState">The current game state</param>
        /// <param name="otherItemTrackingName">
        /// If another item is being used on this item, this is the tracking name of the other item
        /// </param>
        public virtual void Interact(GameState gameState, string otherItemTrackingName)
        {
            Console.WriteLine("You find nothing special.");
        }

        /// <summary>
        /// Allows an item to specify what happens when it is grabbed.
        /// </summary>
        /// <param name="count">The number of items being grabbed</param>
        /// <param name="grabbingCharacterTrackingId">The name of the character who is grabbing</param>
        /// <param name="gameState">The current game state</param>
        /// <param name="gameData">The current game data</param>
        public virtual void Grab(int count, Guid grabbingCharacterTrackingId, GameState gameState)
        {
            var description = GetDescription(count, gameState);
            var characterLoc = GameState.CurrentGameState.GetCharacterLocation(grabbingCharacterTrackingId);
            // Remove it from the floor
            var removeLocationResult = GameState.CurrentGameState.TryAddLocationItemCount(characterLoc.TrackingId, TrackingName, -count, this);
            // And place it in the player's inventory, but only if it was removed from the floor successfully
            if (removeLocationResult)
            {
                GameState.CurrentGameState.TryAddCharacterItemCount(grabbingCharacterTrackingId, TrackingName, count, this);
                Console.WriteLine($"You grabbed {description}.");
            }
            else
            {
                Console.WriteLine($"Something prevented you from grabbing {description}.");
            }
        }

        /// <summary>
        /// Allows an item to specify what happens when it is dropped.
        /// </summary>
        /// <param name="count">The number of items being dropped</param>
        /// <param name="droppingCharacterTrackingId">The name of the character who is dropping</param>
        /// <param name="gameState">The current game state</param>
        /// <param name="gameData">The current game data</param>
        public virtual void Drop(int count, Guid droppingCharacterTrackingId, GameState gameState)
        {
            var description = GetDescription(count, gameState);
            var characterLoc = GameState.CurrentGameState.GetCharacterLocation(droppingCharacterTrackingId);

            // Remove it from the character's inventory
            var removeCharResult = GameState.CurrentGameState.TryAddCharacterItemCount(droppingCharacterTrackingId, TrackingName, -count, this);

            // And place it on the floor, but only if it was removed from the inventory successfully
            if (removeCharResult)
            {
                GameState.CurrentGameState.TryAddLocationItemCount(characterLoc.TrackingId, TrackingName, count, this);
                Console.WriteLine($"You dropped {description}.");
            }
            else
            {
                Console.WriteLine($"Something prevented you from dropping {description}.");
            }
        }
    }
}
