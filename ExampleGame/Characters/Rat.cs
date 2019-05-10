using GameEngine;
using GameEngine.Characters;
using GameEngine.Commands;
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
            var choice = rnd.Next(0, 8);

            if (choice > 2)
            {
                this.GetLocation().SendMessage($"{Name} squeaks.", this);
            }

            switch (choice)
            {
                case 0:
                    // TODO: Look
                case 1:
                    // TODO: Interact
                case 2:
                    var randomItem = PickRandomLocationItem();
                    if (randomItem.HasValue)
                    {
                        var extraWords = new List<string>()
                        {
                            randomItem.Value.Value.ToString(),
                            randomItem.Value.Key.TrackingId.ToString(),
                        };
                        PublicCommandHelper.TryRunPublicCommand("grab", extraWords, this);
                    }
                    break;
                case 3:
                    // TODO: Move
                case 4:
                    // TODO: Drop
                case 5:
                    // TODO: Stats
                case 6:
                    // TODO: Inventory
                case 7:
                    var randomCharacter = PickRandomCharacterInRoom();
                    if (randomCharacter != null)
                    {
                        var extraWords = new List<string>() { randomCharacter.TrackingId.ToString() };
                        PublicCommandHelper.TryRunPublicCommand("attack", extraWords, this);
                    }
                    break;
            }
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

        private KeyValuePair<Item, int>? PickRandomLocationItem()
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


    }
}
