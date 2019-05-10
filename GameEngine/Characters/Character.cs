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
        public int CounterAttackPercent { get; set; }

        [JsonProperty]
        public int MaxHeal { get; set; }

        private static Random rnd = new Random();

        public Character(string name, int hp)
        {
            Name = name;
            HitPoints = hp;
            MaxHitPoints = hp;
        }

        public bool IsPlayerCharacter()
        {
            return this is PlayerCharacter;
        }

        /// <summary>
        /// Sends a message to the character that only the receiving character can see
        /// </summary>
        /// <param name="text">The text to send</param>
        public void SendMessage(string text, bool newLine = true)
        {
            var playerCharacter = GameState.CurrentGameState?.GetPlayerCharacter();

            // Only show the message if the message is for the player
            if (playerCharacter == null || this.TrackingId == playerCharacter.TrackingId)
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
            Hit(attackDamage);
            var attackMessage = $"{attackingCharacter.Name} hits {this.Name} for {attackDamage} damage!";
            this.GetLocation().SendMessage(attackMessage, null);
            if (IsDead())
            {
                this.GetLocation().SendMessage($"{Name} has been killed.", null);
                return;
            }

            var counterAttackRoll = rnd.Next(1, 100);
            if(counterAttackRoll >= (100 - CounterAttackPercent))
            {
                var maxAttackDamage = MaxAttack / 2;
                var counterAttackDamage = GetAttackDamage(maxAttackDamage);
                attackingCharacter.Hit(counterAttackDamage);
                var counterAttackMessage = $"{this.Name} counter attacks and hits {attackingCharacter.Name} for {counterAttackDamage} damage!";
                this.GetLocation().SendMessage(counterAttackMessage, null);

                if (attackingCharacter.IsDead())
                {
                    this.GetLocation().SendMessage($"{attackingCharacter.Name} has been killed.", null);
                    return;
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

        public void Hit(int hitAmount)
        {
            if (IsDead())
            {
                return;
            }

            if ((HitPoints - hitAmount) < 0)
            {
                hitAmount = HitPoints;
            }
            HitPoints -= hitAmount;
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
            HitPoints += HitPoints;
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
