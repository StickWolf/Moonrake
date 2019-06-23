using BaseClientServerDtos;
using BaseClientServerDtos.ToClient;
using Newtonsoft.Json;
using ServerEngine.Commands;
using ServerEngine.GrainSiloAndClient;
using ServerEngine.Locations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ServerEngine.Characters
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

        [JsonProperty]
        public List<string> TurnBehaviors { get; set; }

        [JsonProperty]
        public DateTime LastKilledTime { get; set; }

        [JsonProperty]
        public TimeSpan RespawnTime { get; set; }

        [JsonProperty]
        public TimeSpan TurnCooldown { get; set; }

        [JsonProperty]
        public DateTime LastTurnTakenTime { get; set; }

        /// <summary>
        /// Indicates if a player character has ever entered the world before
        /// </summary>
        [JsonProperty]
        public bool IsNew { get; set; } = true;

        /// <summary>
        /// Indicates if this character needs to be focused on by a client in order to stay in the world
        /// </summary>
        [JsonProperty]
        public bool NeedsFocus { get; set; }

        private static Random rnd = new Random();

        public Character(string name, int hitPoints)
        {
            Name = name;
            HitPoints = hitPoints;
            MaxHitPoints = hitPoints;
            RespawnTime = TimeSpan.FromMinutes(3);
            LastKilledTime = DateTime.MinValue;

            // Default NPC cooldown is somewhere between 15 and 25 seconds
            TurnCooldown = TimeSpan.FromSeconds(rnd.Next(15, 26));
            LastTurnTakenTime = DateTime.MinValue;
        }

        /// <summary>
        /// Sends a message to the character that only the receiving character can see
        /// </summary>
        /// <param name="text">The text to send</param>
        public void SendDtoMessage(FiniteDto dto)
        {
            // Is any client focusing on this character? If not then no message is sent.
            var focusedClient = AttachedClients.GetCharacterFocusedClient(this.TrackingId);
            if (focusedClient == null)
            {
                return;
            }

            focusedClient.SendDtoMessage(dto);
        }

        public void SendDescriptiveTextDtoMessage(string text)
        {
            var dto = new DescriptiveTextDto(text);
            SendDtoMessage(dto);
        }

        /// <summary>
        /// Gets the current location of the character
        /// </summary>
        /// <returns></returns>
        public Location GetLocation()
        {
            return GrainClusterClient.Universe.GetCharacterLocation(this.TrackingId).Result;
        }

        public AttachedClient GetClient()
        {
            return AttachedClients.GetCharacterFocusedClient(this.TrackingId);
        }

        public bool CanTakeTurn()
        {
            return (DateTime.Now > LastTurnTakenTime + TurnCooldown);
        }

        public int GetSecondsTillNextTurn()
        {
            var timeTill = (LastTurnTakenTime + TurnCooldown) - DateTime.Now;
            return Convert.ToInt32(timeTill.TotalSeconds);
        }

        /// <summary>
        /// Allows the character to take their turn.
        /// </summary>
        public virtual void Turn()
        {
            if (TurnBehaviors != null)
            {
                foreach (var turnBehaviorName in TurnBehaviors)
                {
                    var behavior = GrainClusterClient.Universe.GetTurnBehavior(turnBehaviorName).Result;
                    behavior?.Turn(this);
                }
            }
        }

        public void TurnComplete()
        {
            LastTurnTakenTime = DateTime.Now;
        }

        /// <summary>
        /// Indicates if the server should consider this character as "present in the world"
        /// or if the character is "logged out" / "offline", etc.
        /// </summary>
        /// <returns></returns>
        public bool IsPresentInWorld()
        {
            if (!NeedsFocus)
            {
                return true;
            }

            return HasClientFocus();
        }

        /// <summary>
        /// Indicates if there is a client currently focusing on this character
        /// </summary>
        /// <returns>True if there is a focusing client</returns>
        public bool HasClientFocus()
        {
            var client = AttachedClients.GetCharacterFocusedClient(this.TrackingId);
            return client != null;
        }

        public virtual void Attack(Character attackingCharacter)
        {
            var attackDamage = GetAttackDamage(attackingCharacter.MaxAttack);
            Hit(attackDamage);
            this.GetLocation().SendDescriptiveTextDtoMessage($"{attackingCharacter.Name} hits {this.Name} for {attackDamage} damage!", null);
            if (IsDead())
            {
                this.GetLocation().SendDescriptiveTextDtoMessage($"{Name} has been killed.", null);
                return;
            }

            var counterAttackRoll = rnd.Next(1, 100);
            if(counterAttackRoll >= (100 - CounterAttackPercent))
            {
                var maxAttackDamage = MaxAttack / 2;
                var counterAttackDamage = GetAttackDamage(maxAttackDamage);
                attackingCharacter.Hit(counterAttackDamage);

                this.GetLocation().SendDescriptiveTextDtoMessage($"{this.Name} counter attacks and hits {attackingCharacter.Name} for {counterAttackDamage} damage!", null);

                if (attackingCharacter.IsDead())
                {
                    this.GetLocation().SendDescriptiveTextDtoMessage($"{attackingCharacter.Name} has been killed.", null);
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

            if (IsDead())
            {
                LastKilledTime = DateTime.Now;
            }
        }

        public void Respawn()
        {
            if (CanRespawn())
            {
                HitPoints = MaxHitPoints;
            }
        }

        public bool CanRespawn()
        {
            if(LastKilledTime + RespawnTime < DateTime.Now)
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
            HitPoints += HitPoints;
        }

        public void Heal(Character healingCharacter)
        {
            if(IsDead() || healingCharacter.IsDead())
            {
                return;
            }
            var healAmount = GetHealAmount(MaxHeal);
            Heal(healAmount);

            this.GetLocation().SendDescriptiveTextDtoMessage($"{Name} has been healed for {healAmount}.", null);
        }

        private int GetHealAmount(int maxHeal)
        {
            int heal = rnd.Next(0, maxHeal);
            return maxHeal;
        }

        /// <summary>
        /// Gets all items the character is holding in their inventory that can be used
        /// </summary>
        /// <returns></returns>
        public List<KeyValuePair<Item, int>> GetUseableInventoryItems()
        {
            var useableCharacterItems = GrainClusterClient.Universe.GetCharacterItems(this.TrackingId).Result
                .Where(i => i.Key.IsVisible) // Only allow interaction with visible items
                .Where(i => i.Key.IsUseableFrom == ItemUseableFrom.All || i.Key.IsUseableFrom == ItemUseableFrom.Inventory) // Only choose items that can be used
                .Select(i => new KeyValuePair<Item, int>(i.Key, i.Value))
                .ToList();
            return useableCharacterItems;
        }

        /// <summary>
        /// Makes the character use the interact command
        /// </summary>
        /// <param name="primaryItem">The primary item to have the character interact with</param>
        /// <param name="secondaryItem">The secondary item to have the character interact with</param>
        public void ExecuteInteractCommand(Item primaryItem, Item secondaryItem)
        {
            var extraWords = new List<string>();
            if (secondaryItem != null)
            {
                extraWords.Add(secondaryItem.TrackingId.ToString());
            }
            if (primaryItem != null)
            {
                extraWords.Add(primaryItem.TrackingId.ToString());
            }

            CommandRunner.TryRunCommandFromCharacter("use", extraWords, this);
        }

        /// <summary>
        /// Makes the character run the grab command on the specified item.
        /// Note that the item must be present in the amount specified in 
        /// order for the command to succeed.
        /// </summary>
        /// <param name="itemToGrab">The item to grab</param>
        /// <param name="countToGrab">The number of items the character should try to grab</param>
        public void ExecuteGrabCommand(Item itemToGrab, int countToGrab)
        {
            var extraWords = new List<string>()
                {
                    countToGrab.ToString(),
                    itemToGrab.TrackingId.ToString(),
                };
            CommandRunner.TryRunCommandFromCharacter("grab", extraWords, this);
        }

        /// <summary>
        /// Makes the character run the drop command on the specified item.
        /// Note that the item must be present in the amount specified in
        /// order for the command to succeed.
        /// </summary>
        /// <param name="itemToDrop">The item to drop</param>
        /// <param name="countToDrop">The cound of items the character should try to drop</param>
        public void ExecuteDropCommand(Item itemToDrop, int countToDrop)
        {
            var extraWords = new List<string>()
                {
                    countToDrop.ToString(),
                    itemToDrop.TrackingId.ToString(),
                };
            CommandRunner.TryRunCommandFromCharacter("drop", extraWords, this);
        }

        /// <summary>
        /// Makes the character run the attack command on the specified character.
        /// </summary>
        /// <param name="characterToAttack">The character that this character should attack</param>
        public void ExecuteAttackCommand(Character characterToAttack)
        {
            var extraWords = new List<string>() { characterToAttack.TrackingId.ToString() };
            CommandRunner.TryRunCommandFromCharacter("attack", extraWords, this);
        }

        /// <summary>
        /// Makes the character run the move command
        /// </summary>
        /// <param name="locationToMoveTo">The location to attempt to move to</param>
        public void ExecuteMoveCommand(Location locationToMoveTo)
        {
            var extraWords = new List<string>() { locationToMoveTo.TrackingId.ToString() };
            CommandRunner.TryRunCommandFromCharacter("move", extraWords, this);
        }

        /// <summary>
        /// Makes the character run the look command
        /// </summary>
        public void ExecuteLookCommand()
        {
            CommandRunner.TryRunCommandFromCharacter("look", new List<string>(), this);
        }

        /// <summary>
        /// Makes the character run the stats command
        /// </summary>
        public void ExecuteStatsCommand()
        {
            CommandRunner.TryRunCommandFromCharacter("stats", new List<string>(), this);
        }

        /// <summary>
        /// Makes the character run the inventory command
        /// </summary>
        public void ExecuteInventoryCommand()
        {
            CommandRunner.TryRunCommandFromCharacter("inv", new List<string>(), this);
        }
    }
}
