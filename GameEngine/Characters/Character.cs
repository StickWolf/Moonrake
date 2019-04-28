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

        public int Hp, FullHp, MaxAttack, CounterAttackChance, MaxHeal;

        private static Random rnd = new Random();

        public Character(string name, int hp)
        {
            Name = name;
            Hp = hp;
            FullHp = hp;
        }

        /// <summary>
        /// Allows the character to take their turn.
        /// </summary>
        public virtual void Turn(GameSourceData gameData)
        {
        }

        private List<Character> GetCharacters(GameSourceData gameData)
        {
            //TODO: This should be in the gameState
            var locationName = GameState.CurrentGameState.GetCharacterLocation(PlayerCharacter.TrackingName);
            var charactersInLocation = GameState.CurrentGameState.GetCharactersInLocation(locationName);
            charactersInLocation.Add(PlayerCharacter.TrackingName);
            List<Character> characterList = new List<Character>();
            foreach (var characterName in charactersInLocation)
            {
                gameData.TryGetCharacter(characterName, out Character character);
                characterList.Add(character);
            }
            return characterList;
        }
        public virtual void Attack(Character attackingCharacter, GameSourceData gameData)
        {
            var attackDamage = GetAttackDamage(attackingCharacter.MaxAttack);
            var charactersInLocation = GetCharacters(gameData);
            Hp = Hp - attackDamage;
            if (Hp <= 0)
            {
                // Makes sure that the player has 0 hp, so when a heal
                // comes in, it isn't like -100 to -50.
                Hp = 0;
                Console.WriteLine($"{Name} is dead.");
                return;
            }
            if (charactersInLocation.Contains(this))
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
                if (charactersInLocation.Contains(attackingCharacter))
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

        public bool IsDead()
        {
            if(Hp <= 0)
            {
                return true;
            }
            return false;
        }

        public void Heal(Character healingCharacter, GameSourceData gameData)
        {
            if(Hp == 0)
            {
                return;
            }
            var healAmount = GetHealAmount(MaxHeal);
            var charactersInLocation = GetCharacters(gameData);
            if ((Hp + healAmount) > FullHp)
            {
                healAmount = FullHp - Hp;
            }
            Hp = Hp + healAmount;
            if (charactersInLocation.Contains(this))
            {
                Console.WriteLine($"{Name} has been healed for {healAmount}.");
            }
        }

        private int GetHealAmount(int maxHeal)
        {
            int heal = rnd.Next(0, maxHeal);
            return maxHeal;
        }
    }
}
