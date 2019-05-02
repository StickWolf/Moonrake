using Newtonsoft.Json;
using System;

namespace GameEngine.Characters
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class Character : TrackableInstance
    {
        [JsonProperty]
        public string Name { get; set; }

        [JsonProperty]
        public int HitPoints { get; set; }

        [JsonProperty]
        public int MaxHitPoints { get; set; }

        [JsonProperty]
        public int MaxAttack { get; set; }

        [JsonProperty]
        public int CounterAttackChance { get; set; }

        [JsonProperty]
        public int MaxHeal { get; set; }

        private static Random rnd = new Random();

        public Character(string name, int hp)
        {
            Name = name;
            HitPoints = hp;
            MaxHitPoints = hp;
        }

        /// <summary>
        /// Allows the character to take their turn.
        /// </summary>
        public virtual void Turn()
        {
        }

        public virtual void Attack(Character attackingCharacter)
        {
            var attackDamage = GetAttackDamage(attackingCharacter.MaxAttack);
            var playerCharacter = GameState.CurrentGameState.GetPlayerCharacter();
            var playerLoc = GameState.CurrentGameState.GetCharacterLocation(playerCharacter.TrackingId);
            var charactersInLocation = GameState.CurrentGameState.GetCharactersInLocation(playerLoc.TrackingId, includePlayer: true);
            HitPoints = HitPoints - attackDamage;
            if (HitPoints <= 0)
            {
                // Makes sure that the player has 0 hp, so when a heal
                // comes in, it isn't like -100 to -50.
                HitPoints = 0;
                Console.WriteLine($"{Name} is dead.");
                return;
            }
            if (charactersInLocation.Contains(this))
            {
                Console.WriteLine($"{Name} has {HitPoints}/{MaxHitPoints} left");
            }

            var counterAttackPosiblity = rnd.Next(CounterAttackChance, 100);
            if(counterAttackPosiblity >= 75)
            {
                if (IsDead())
                {
                    return;
                }
                Console.WriteLine($"{Name} countered!");
                var maxAttackDamage = MaxAttack / 2;
                var counterAttackDamage = GetAttackDamage(maxAttackDamage);
                attackingCharacter.HitPoints = attackingCharacter.HitPoints - counterAttackDamage;
                if (attackingCharacter.HitPoints <= 0)
                {
                    // Makes sure that the player has 0 hp, so when a heal
                    // comes in, it isn't like -100 to -50.
                    attackingCharacter.HitPoints = 0;
                    Console.WriteLine($"{attackingCharacter.Name} is dead.");
                    return;
                }
                if (charactersInLocation.Contains(attackingCharacter))
                {
                    Console.WriteLine($"{attackingCharacter.Name} has {attackingCharacter.HitPoints}/{attackingCharacter.MaxHitPoints} left");
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
            if(HitPoints <= 0)
            {
                return true;
            }
            return false;
        }

        public void Heal(int healAmount)
        {
            if (IsDead())
            {
                return;
            }

            if ((HitPoints + healAmount) > MaxHitPoints)
            {
                healAmount = MaxHitPoints - HitPoints;
            }
            HitPoints = HitPoints + healAmount;
        }

        public void Heal(Character healingCharacter)
        {
            if(IsDead())
            {
                return;
            }
            var healAmount = GetHealAmount(MaxHeal);
            var playerCharacter = GameState.CurrentGameState.GetPlayerCharacter();
            var playerLoc = GameState.CurrentGameState.GetCharacterLocation(playerCharacter.TrackingId);
            var charactersInLocation = GameState.CurrentGameState.GetCharactersInLocation(playerLoc.TrackingId, includePlayer: true);
            Heal(healAmount);
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
