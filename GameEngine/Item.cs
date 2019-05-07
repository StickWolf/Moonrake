﻿using Newtonsoft.Json;
using System;

namespace GameEngine
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class Item : TrackableInstance
    {
        /// <summary>
        /// Name that is displayed in the game
        /// </summary>
        [JsonProperty]
        public string DisplayName { get; private set; }

        /// <summary>
        /// Tells if the item is unique (there can be only 1), or not, in which case there could be many.
        /// The game will take care of assuring that only 1 is held by any character at a time.
        /// </summary>
        [JsonProperty]
        public bool IsUnique { get; set;}

        /// <summary>
        /// Indicates if an item can be picked up, moved locations and dropped or if it is stuck to where it is.
        /// </summary>
        [JsonProperty]
        public bool IsBound { get; set; }

        /// <summary>
        /// Indicates if this item can be the primary target of interaction.
        /// <see cref="IsUseableFrom"/> must also be set correctly for the item to actually be interacted with.
        /// </summary>
        [JsonProperty]
        public bool IsInteractionPrimary { get; set; }

        /// <summary>
        /// Indicates where an item can be used from (location, inventory, etc)
        /// </summary>
        [JsonProperty]
        public ItemUseableFrom IsUseableFrom { get; set; } = ItemUseableFrom.Nowhere;

        /// <summary>
        /// Indicates if the item is visible or not.
        /// Only visible items can be picked up or seen.
        /// </summary>
        [JsonProperty]
        public bool IsVisible { get; set; } = true;

        public Item(string displayName)
        {
            DisplayName = displayName;
        }

        /// <summary>
        /// Gets a description of what the item looks like.
        /// </summary>
        /// <param name="count">The number of items that are present</param>
        /// <param name="gameData">The gamedata for the game</param>
        /// <param name="currentGameState">The current game state</param>
        /// <returns>A description</returns>
        public virtual string GetDescription(int count)
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
        /// <param name="otherItem">
        /// If another item is being used on this item, this is that other item
        /// </param>
        public virtual void Interact(Item otherItem, Guid interactingCharacterTrackingId)
        {
            // TODO: go through all interact inmplementations and make sure they are using the correct .SendMessage calls
            Console.WriteLine("You find nothing special.");
        }

        /// <summary>
        /// Allows an item to specify what happens when it is grabbed.
        /// </summary>
        /// <param name="count">The number of items being grabbed</param>
        /// <param name="grabbingCharacterTrackingId">The name of the character who is grabbing</param>
        /// <param name="gameState">The current game state</param>
        /// <param name="gameData">The current game data</param>
        public virtual void Grab(int count, Guid grabbingCharacterTrackingId)
        {
            // TODO: go through all grab implementations and make sure they are using the correct .SendMessage calls
            var description = GetDescription(count);
            var characterLoc = GameState.CurrentGameState.GetCharacterLocation(grabbingCharacterTrackingId);
            // Remove it from the floor
            var removeLocationResult = GameState.CurrentGameState.TryAddLocationItemCount(characterLoc.TrackingId, this.TrackingId, -count);
            // And place it in the player's inventory, but only if it was removed from the floor successfully
            if (removeLocationResult)
            {
                GameState.CurrentGameState.TryAddCharacterItemCount(grabbingCharacterTrackingId, this.TrackingId, count);
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
        public virtual void Drop(int count, Guid droppingCharacterTrackingId)
        {
            // TODO: go through all drop implementations and make sure they are using the correct .SendMessage calls
            var description = GetDescription(count);
            var characterLoc = GameState.CurrentGameState.GetCharacterLocation(droppingCharacterTrackingId);

            // Remove it from the character's inventory
            var removeCharResult = GameState.CurrentGameState.TryAddCharacterItemCount(droppingCharacterTrackingId, this.TrackingId, -count);

            // And place it on the floor, but only if it was removed from the inventory successfully
            if (removeCharResult)
            {
                GameState.CurrentGameState.TryAddLocationItemCount(characterLoc.TrackingId, this.TrackingId, count);
                Console.WriteLine($"You dropped {description}.");
            }
            else
            {
                Console.WriteLine($"Something prevented you from dropping {description}.");
            }
        }
    }
}
