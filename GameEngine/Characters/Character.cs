using GameEngine.Locations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

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
        /// Sends a message to the character that only the receiving character can see
        /// </summary>
        /// <param name="text">The text to send</param>
        public void SendMessage(string text, bool newLine = true)
        {
            var playerCharacter = GameState.CurrentGameState.GetPlayerCharacter();

            // Only show the message if the message is for the player
            if (this.TrackingId == playerCharacter.TrackingId)
            {
                if (newLine)
                {
                    Console.WriteLine(text); // SendMessage
                }
                else
                {
                    Console.Write(text); // SendMessage
                }
            }
            else
            {
                // TODO: add an admin command that lets you see other characters messages
                //Console.WriteLine($"{{MessageToCharacter}} \"{this.Name}\" : {text}"); // SendMessage
            }
        }

        /// <summary>
        /// Sends a blank line to the character
        /// </summary>
        public void SendMessage()
        {
            SendMessage(string.Empty);
        }

        public string Choose(string prompt, List<string> choices, bool includeCancel)
        {
            return Console.Choose(prompt, choices, this, includeCancel);
        }

        public T Choose<T>(string prompt, Dictionary<T, string> choices, bool includeCancel)
        {
            return Console.Choose(prompt, choices, this, includeCancel);
        }

        /// <summary>
        /// Gets the current location of the character
        /// </summary>
        /// <returns></returns>
        public Location GetLocation()
        {
            return GameState.CurrentGameState.GetCharacterLocation(this.TrackingId);
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

            HitPoints -= attackDamage;
            if (HitPoints <= 0)
            {
                // Makes sure that the player has 0 hp, so when a heal
                // comes in, it isn't like -100 to -50.
                HitPoints = 0;
                this.GetLocation().SendMessage($"{Name} is dead.", this);
                return;
            }

            this.GetLocation().SendMessage($"{Name} has {HitPoints}/{MaxHitPoints} left", this);

            var counterAttackPosiblity = rnd.Next(CounterAttackChance, 100);
            if(counterAttackPosiblity >= 75)
            {
                if (IsDead())
                {
                    return;
                }
                this.GetLocation().SendMessage($"{Name} countered!", this);

                var maxAttackDamage = MaxAttack / 2;
                var counterAttackDamage = GetAttackDamage(maxAttackDamage);
                attackingCharacter.HitPoints = attackingCharacter.HitPoints - counterAttackDamage;
                if (attackingCharacter.HitPoints <= 0)
                {
                    // Makes sure that the player has 0 hp, so when a heal
                    // comes in, it isn't like -100 to -50.
                    attackingCharacter.HitPoints = 0;
                    this.GetLocation().SendMessage($"{attackingCharacter.Name} is dead.", this);
                    return;
                }

                this.GetLocation().SendMessage($"{attackingCharacter.Name} has {attackingCharacter.HitPoints}/{attackingCharacter.MaxHitPoints} left", this);
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
            Heal(healAmount);
            this.GetLocation().SendMessage($"{Name} has been healed for {healAmount}.", this);
        }

        private int GetHealAmount(int maxHeal)
        {
            int heal = rnd.Next(0, maxHeal);
            return maxHeal;
        }
    }
}
