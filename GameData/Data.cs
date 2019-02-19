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

        public string PlayerName { get; set; } = "Eric";
        public string PlayerNewName { get; set; }

        public List<Character> CreateAllGameCharacters()
        {
            var allCharacters = new List<Character>();

            // TODO: create a list of characters that will be in this game here.

            return allCharacters;
        }

        public string GetGameIntroduction()
        {
            return "Once, there were three ancient instruments." + 
                "The Harp, Piano, and the Drum." + 
                "Inside of each instrument there was a magical gem." +
                "A ruby, sapphire, and a diamond." +
                "When the gems are merged, it will the create an ancient weapon." +
                "The Moonrake."; 
        }

        public string MainCharacterNamePick()
        {          
            return $"You, are {PlayerName}, would you like to change your name? yes/no";           
        }

        public string IfPlayerWantsToChangeName()
        {
            return $"You are changing your name, what is your new name?: ";
        }

        public string FinalName()
        {
            return $"Very well, you are: {PlayerName}.";
        }
    }
}
