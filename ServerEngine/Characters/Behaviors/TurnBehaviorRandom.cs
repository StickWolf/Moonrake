using ServerEngine.GrainSiloAndClient;
using ServerEngine.Locations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ServerEngine.Characters.Behaviors
{
    public class TurnBehaviorRandom : ITurnBehavior
    {
        private static Random rnd = new Random();

        public void Turn(Character turnTakingCharacter)
        {
            var choice = rnd.Next(0, 30);
            switch (choice)
            {
                case 0:
                    turnTakingCharacter.ExecuteLookCommand();
                    break;
                case 1:
                    AiRandomInteract(turnTakingCharacter);
                    break;
                case 2:
                    AiRandomGrab(turnTakingCharacter);
                    break;
                case 3:
                    AiRandomMove(turnTakingCharacter);
                    break;
                case 4:
                    AiRandomDrop(turnTakingCharacter);
                    break;
                case 5:
                    turnTakingCharacter.ExecuteStatsCommand();
                    break;
                case 6:
                    turnTakingCharacter.ExecuteInventoryCommand();
                    break;
                case 7:
                    AiRandomAttack(turnTakingCharacter);
                    break;
            }
        }

        private void AiRandomAttack(Character turnTakingCharacter)
        {
            var randomCharacter = PickRandomCharacterInRoom(turnTakingCharacter);
            if (randomCharacter != null)
            {
                turnTakingCharacter.ExecuteAttackCommand(randomCharacter);
            }
        }

        private void AiRandomGrab(Character turnTakingCharacter)
        {
            var randomItem = PickRandomGrabbableLocationItem(turnTakingCharacter);
            if (randomItem.HasValue)
            {
                var rndCount = rnd.Next(0, randomItem.Value.Value + 1);
                turnTakingCharacter.ExecuteGrabCommand(randomItem.Value.Key, rndCount);
            }
        }

        private void AiRandomDrop(Character turnTakingCharacter)
        {
            var randomItem = PickRandomDroppableInventoryItem(turnTakingCharacter);
            if (randomItem.HasValue)
            {
                var rndCount = rnd.Next(0, randomItem.Value.Value + 1);
                turnTakingCharacter.ExecuteDropCommand(randomItem.Value.Key, rndCount);
            }
        }

        private void AiRandomMove(Character turnTakingCharacter)
        {
            var randomLocation = PickRandomMoveableLocation(turnTakingCharacter);
            if (randomLocation != null)
            {
                turnTakingCharacter.ExecuteMoveCommand(randomLocation);
            }
        }

        private void AiRandomInteract(Character turnTakingCharacter)
        {
            var secondaryItems = turnTakingCharacter.GetUseableInventoryItems()
                .Union(turnTakingCharacter.GetLocation().GetUseableItems()).ToList();

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

            turnTakingCharacter.ExecuteInteractCommand(primaryItem, secondaryItem);
        }

        private Location PickRandomMoveableLocation(Character turnTakingCharacter)
        {
            var destLocations = GrainClusterClient.Universe.GetConnectedLocations(turnTakingCharacter.GetLocation().TrackingId).Result;
            if (destLocations.Count == 0)
            {
                return null;
            }
            var index = rnd.Next(0, destLocations.Count);
            return destLocations[index];
        }

        private Character PickRandomCharacterInRoom(Character turnTakingCharacter)
        {
            var roomCharactersExceptRat = GrainClusterClient.Universe.GetCharactersInLocation(turnTakingCharacter.GetLocation().TrackingId).Result
                .Where(c => c.TrackingId != turnTakingCharacter.TrackingId).ToList();
            if (roomCharactersExceptRat.Count == 0)
            {
                return null;
            }
            var index = rnd.Next(0, roomCharactersExceptRat.Count);
            return roomCharactersExceptRat[index];
        }

        private KeyValuePair<Item, int>? PickRandomGrabbableLocationItem(Character turnTakingCharacter)
        {
            var locationItems = GrainClusterClient.Universe.GetLocationItems(turnTakingCharacter.GetLocation().TrackingId).Result
                .Where(i => !i.Key.IsBound && i.Key.IsVisible).ToList();
            if (locationItems.Count == 0)
            {
                return null;
            }
            var index = rnd.Next(0, locationItems.Count);
            return locationItems.ElementAt(index);
        }

        private KeyValuePair<Item, int>? PickRandomDroppableInventoryItem(Character turnTakingCharacter)
        {
            var inventoryItems = GrainClusterClient.Universe.GetCharacterItems(turnTakingCharacter.TrackingId).Result
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
