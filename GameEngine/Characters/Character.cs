using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Characters
{
    public class Character
    {
        public string Name { get; private set; }

        public int Hp, FullHp, MaxAttack, CounterAttackChance;

        private static Random rnd = new Random();

        public Character(string name, int hp)
        {
            Name = name;
            Hp = hp;
            FullHp = hp;
        }

        public virtual void Attack(Character attackingCharacter, GameSourceData gameData)
        {
            var attackDamage = GetAttackDamage(attackingCharacter.MaxAttack);
            var locationName = GameState.CurrentGameState.GetCharacterLocation(PlayerCharacter.TrackingName);
            var charactersInLocation = GameState.CurrentGameState.GetCharactersInLocation(locationName);
            charactersInLocation.Add(PlayerCharacter.TrackingName);
            List<Character> characterList = new List<Character>();
            foreach(var characterName in charactersInLocation)
            {
                gameData.TryGetCharacter(characterName, out Character character);
                characterList.Add(character);
            }
            Hp = Hp - attackDamage;
            if (Hp <= 0)
            {
                // Makes sure that the player has 0 hp, so when a heal
                // comes in, it isn't like -100 to -50.
                Hp = 0;
                Console.WriteLine($"{Name} is dead.");
                return;
            }
            if (charactersInLocation.Contains(Name))
            {
                Console.WriteLine($"{Name} has {Hp}/{FullHp} left");
            }

            var counterAttackPosiblity = rnd.Next(CounterAttackChance, 100);
            if(counterAttackPosiblity >= 75)
            {
                if (Hp == 0)
                {
                    return;
                }
                Console.WriteLine($"{Name} countered!");
                var maxAttackDamage = MaxAttack / 2;
                var counterAttackDamage = GetAttackDamage(maxAttackDamage);
                attackingCharacter.Hp = attackingCharacter.Hp - counterAttackDamage;
                if (attackingCharacter.Hp <= 0)
                {
                    // Makes sure that the player has 0 hp, so when a heal
                    // comes in, it isn't like -100 to -50.
                    attackingCharacter.Hp = 0;
                    Console.WriteLine($"{attackingCharacter.Name} is dead.");
                    return;
                }
                if (charactersInLocation.Contains(attackingCharacter.Name))
                {
                    Console.WriteLine($"{attackingCharacter.Name} has {attackingCharacter.Hp}/{attackingCharacter.FullHp} left");
                }
            }
        }

        private int GetAttackDamage(int maxAttack)
        {
            int damage = rnd.Next(0, maxAttack);
            return damage;
        }

        //ZABTODO: Make a heal fuction and add a healing power method
    }
}
