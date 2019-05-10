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
            var choice = rnd.Next(0, 5);

            switch (choice)
            {
                case 0:
                    this.GetLocation().SendMessage($"{Name} squeaks.", this);
                    break;
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
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
            var index = rnd.Next(0, roomCharactersExceptRat.Count-1);
            return roomCharactersExceptRat[index];
        }

    }
}
