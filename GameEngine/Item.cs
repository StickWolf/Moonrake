using GameEngine.Characters;
using Newtonsoft.Json;
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
        /// <param name="otherItem">
        /// If another item is being used on this item, this is that other item
        /// </param>
        /// <param name="interactingCharacter">The character that is initiating the interaction</param>
        public virtual void Interact(Item otherItem, Character interactingCharacter)
        {
            // TODO: go through all interact inmplementations and make sure they are using the correct .SendMessage calls
            interactingCharacter.SendMessage("You find nothing special.");
        }

        /// <summary>
        /// Allows an item to specify what happens when it is grabbed.
        /// </summary>
        /// <param name="count">The number of items being grabbed</param>
        /// <param name="grabbingCharacter">The character that is doing the grabbing</param>
        public virtual void Grab(int count, Character grabbingCharacter)
        {
            // TODO: go through all grab implementations and make sure they are using the correct .SendMessage calls
            var description = GetDescription(count);
            var characterLoc = GameState.CurrentGameState.GetCharacterLocation(grabbingCharacter.TrackingId);
            // Remove it from the floor
            var removeLocationResult = GameState.CurrentGameState.TryAddLocationItemCount(characterLoc.TrackingId, this.TrackingId, -count);
            // And place it in the player's inventory, but only if it was removed from the floor successfully
            if (removeLocationResult)
            {
                GameState.CurrentGameState.TryAddCharacterItemCount(grabbingCharacter.TrackingId, this.TrackingId, count);
                grabbingCharacter.SendMessage($"You grabbed {description}.");
            }
            else
            {
                grabbingCharacter.SendMessage($"Something prevented you from grabbing {description}.");
            }
        }

        /// <summary>
        /// Allows an item to specify what happens when it is dropped.
        /// </summary>
        /// <param name="count">The number of items being dropped</param>
        /// <param name="droppingCharacter">The character that is doing the dropping</param>
        public virtual void Drop(int count, Character droppingCharacter)
        {
            // TODO: go through all drop implementations and make sure they are using the correct .SendMessage calls
            var description = GetDescription(count);
            var characterLoc = GameState.CurrentGameState.GetCharacterLocation(droppingCharacter.TrackingId);

            // Remove it from the character's inventory
            var removeCharResult = GameState.CurrentGameState.TryAddCharacterItemCount(droppingCharacter.TrackingId, this.TrackingId, -count);

            // And place it on the floor, but only if it was removed from the inventory successfully
            if (removeCharResult)
            {
                GameState.CurrentGameState.TryAddLocationItemCount(characterLoc.TrackingId, this.TrackingId, count);
                droppingCharacter.SendMessage($"You dropped {description}.");
            }
            else
            {
                droppingCharacter.SendMessage($"Something prevented you from dropping {description}.");
            }
        }
    }
}
