using GameEngine;

namespace ExampleGame
{
    public class ExampleCharacters
    {
        public string Player { get; private set; } 

        public ExampleCharacters(ExampleGameSourceData gameData)
        {
            Player = gameData.AddCharacter(new Character("Player", 50, 40));
        }
    }
}
