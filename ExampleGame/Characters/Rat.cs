using GameEngine;
using GameEngine.Characters;
using GameEngine.Commands;
using GameEngine.Locations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExampleGame.Characters
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class Rat : Character
    {
        [JsonProperty]
        private int WhiskerCount { get; set; }

        private Random rnd = new Random();

        public Rat(string name, int hp, int whiskerCount) : base(name, hp)
        {
            WhiskerCount = whiskerCount;
        }

        public override void Attack(Character attackingCharacter)
        {
            WhiskerCount--;
            attackingCharacter.SendMessage($"{Name}'s whisker count is now {WhiskerCount}");
        }

        public override void Turn()
        {
            var choice = rnd.Next(0, 30);

            switch (choice)
            {
                case 0:
                    ExecuteLookCommand();
                    break;
                case 1:
                    RatAiRandomInteract();
                    break;
                case 2:
                    RatAiRandomGrab();
                    break;
                case 3:
                    RatAiRandomMove();
                    break;
                case 4:
                    RatAiRandomDrop();
                    break;
                case 5:
                    ExecuteStatsCommand();
                    break;
                case 6:
                    ExecuteInventoryCommand();
                    break;
                case 7:
                    RatAiRandomAttack();
                    break;
                case 8:
                    this.GetLocation().SendMessage($"{Name} squeaks.", this);
                    break;
            }
        }

        private void RatAiRandomAttack()
        {
            var randomCharacter = PickRandomCharacterInRoom();
            if (randomCharacter != null)
            {
                ExecuteAttackCommand(randomCharacter);
            }
        }

        private void RatAiRandomGrab()
        {
            var randomItem = PickRandomGrabbableLocationItem();
            if (randomItem.HasValue)
            {
                var rndCount = rnd.Next(0, randomItem.Value.Value + 1);

                ExecuteGrabCommand(randomItem.Value.Key, rndCount);
            }
        }

        private void RatAiRandomDrop()
        {
            var randomItem = PickRandomDroppableInventoryItem();
            if (randomItem.HasValue)
            {
                var rndCount = rnd.Next(0, randomItem.Value.Value + 1);
                ExecuteDropCommand(randomItem.Value.Key, rndCount);
            }
        }

        private void RatAiRandomMove()
        {
            var randomLocation = PickRandomMoveableLocation();
            if (randomLocation != null)
            {
                ExecuteMoveCommand(randomLocation);
            }
        }

        private void RatAiRandomInteract()
        {
            var secondaryItems = GetUseableInventoryItems()
                .Union(GetLocation().GetUseableItems()).ToList();

            var primaryItems = secondaryItems
                .Where(kvp => kvp.Key.IsInteractionPrimary).ToList();

            if (primaryItems.Count == 0)
            {
                return;
            }
            var rndPrimary = rnd.Next(0, primaryItems.Count);
            var primaryItem = primaryItems[rndPrimary].Key;

            if (secondaryItems.Count == 0)
            {
                return;
            }
            var rndSecondary = rnd.Next(0, secondaryItems.Count);
            var secondaryItem = secondaryItems[rndSecondary].Key;

            ExecuteInteractCommand(primaryItem, secondaryItem);
        }

        private Location PickRandomMoveableLocation()
        {
            var destLocations = GameState.CurrentGameState.GetConnectedLocations(this.GetLocation().TrackingId);
            if (destLocations.Count == 0)
            {
                return null;
            }
            var index = rnd.Next(0, destLocations.Count);
            return destLocations[index];
        }

        private Character PickRandomCharacterInRoom()
        {
            var roomCharactersExceptRat = GameState.CurrentGameState.GetCharactersInLocation(GetLocation().TrackingId, includePlayer: true)
                .Where(c => c.TrackingId != this.TrackingId).ToList();
            if (roomCharactersExceptRat.Count == 0)
            {
                return null;
            }
            var index = rnd.Next(0, roomCharactersExceptRat.Count);
            return roomCharactersExceptRat[index];
        }

        private KeyValuePair<Item, int>? PickRandomGrabbableLocationItem()
        {
            var locationItems = GameState.CurrentGameState.GetLocationItems(GetLocation().TrackingId)
                .Where(i => !i.Key.IsBound && i.Key.IsVisible).ToList();
            if (locationItems.Count == 0)
            {
                return null;
            }
            var index = rnd.Next(0, locationItems.Count);
            return locationItems.ElementAt(index);
        }

        private KeyValuePair<Item, int>? PickRandomDroppableInventoryItem()
        {
            var inventoryItems = GameState.CurrentGameState.GetCharacterItems(this.TrackingId)
                .Where(i => !i.Key.IsBound && i.Key.IsVisible).ToList();
            if (inventoryItems.Count == 0)
            {
                return null;
            }
            var index = rnd.Next(0, inventoryItems.Count);
            return inventoryItems.ElementAt(index);
        }


    }
}
