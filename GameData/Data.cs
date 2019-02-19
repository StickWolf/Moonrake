using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine;

namespace GameData
{
    public class Data : IGameData
    {
        public List<Character> CreateAllGameCharacters()
        {
            var allCharacters = new List<Character>();

            // TODO: create a list of characters that will be in this game here.

            return allCharacters;
        }

        public string GetGameIntroduction()
        {
            return "Once, there were three ancient instruments," + 
                " the Harp, Piano, and the Drum." + 
                " Inside of each instrument there was a magical gem," +
                " a ruby, sapphire, and a diamond." +
                " When the gems are merged, it will the create an ancient weapon:" +
                " The Moonrake."; 
        }
    }
}
