using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class Engine
    {
        private List<Character> AllCharacters { get; set; } = new List<Character>();

        private IGameData gameData;

        private Engine(IGameData gameData)
        {
            this.gameData = gameData;

            // Ask the gamedata class to create all the characters that will be needed for the game
            // and fill up our local list with the objects we get back.
            var characters = gameData.CreateAllGameCharacters();
            AllCharacters.AddRange(characters);
        }

        public static void Start(Func<IGameData> gameDataFactory, int loadFromSlot = 0)
        {
            bool keepPlaying = true;

            while (keepPlaying)
            {
                // Create a new game data instance
                var gameData = gameDataFactory();

                // Create a new engine
                var engineInstance = new Engine(gameData);

                // Load a saved game if loadfromSlot is between 0 and 5
                if (loadFromSlot > 0 && loadFromSlot < 6)
                {
                    // TODO: Do the work to load a saved game here.
                }
                else
                {
                    // Otherwise if we're not starting a new game, show the game intro text
                    // Show the game introduction
                    string finishedChange = gameData.FinalName();
                    string answer;
                    string intro = gameData.GetGameIntroduction();
                    string playerData = gameData.MainCharacterNamePick();
                    Console.Clear();
                    Console.WriteLine(intro);
                    Console.WriteLine(playerData);
                    answer = Console.ReadLine();
                    answer = answer.ToLower();
                    if(answer == "yes")
                    {
                        string change = gameData.IfPlayerWantsToChangeName();
                        Console.WriteLine(change);
                        gameData.PlayerNewName = Console.ReadLine();
                        gameData.PlayerName = gameData.PlayerNewName;
                        Console.WriteLine(finishedChange);                     
                    }
                    else if(answer == "no")
                    {
                        Console.WriteLine(finishedChange);
                    } 
                }

                // Let the player keep playing until they are either dead, they won the game or they want to quit.
                // The main game loop, 1 loop = 1 game turn
                while (true)
                {
                    // TODO: Decide everything here that should happen in the main game loop

                    Console.ReadLine();

                    // TODO: If the user types in exit, ask they if they want to save first, then set keepPlaying to false and break;

                    // TODO: If the user types in load, figure out what slot they want to load from e.g. "load 3" will load from slot 3.
                    // TODO: set loadFromSlot to the correct number and break. This will assure we clear the engine of the current game
                    // TODO: and are starting from the save correctly.

                    // TODO: Fix this to check the actual player instead of hardcoding true here.
                    bool playerIsDead = true; 
                    if (playerIsDead)
                    {
                        Console.WriteLine("You have died. Would you like to continue from the last save (yes/no)?");
                        if (Console.ReadLine().Equals("yes", StringComparison.OrdinalIgnoreCase))
                        {
                            // TODO: Figure out what the last save spot is and set that to loadFromSlot here
                        }
                        else
                        {
                            keepPlaying = false;
                        }
                        break;
                    }

                    bool playerHasWon = false;
                    if (playerHasWon)
                    {
                        // TODO: Print out the end of game story. This should be provided by the game data.
                        keepPlaying = false;
                        break;
                    }
                }
            }
        }
    }
}
