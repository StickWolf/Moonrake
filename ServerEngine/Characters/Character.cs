using ServerEngine.Characters.Behaviors;
using ServerEngine.Commands;
using ServerEngine.Commands.Public;
using ServerEngine.Locations;
using Newtonsoft.Json;
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
            // Is any client focusing on this character? If not then no message is sent.
            var focusedClient = AttachedClients.GetCharacterFocusedClient(this.TrackingId);
            if (focusedClient == null)
            {
                return;
            }

            focusedClient.SendMessage(text, newLine);
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
            // Is any client focusing on this character? If not then no message is sent.
            var focusedClient = AttachedClients.GetCharacterFocusedClient(this.TrackingId);
            if (focusedClient == null)
            {
                return null;
            }
            return focusedClient.Choose(prompt, choices, includeCancel);
        }

        public T Choose<T>(string prompt, Dictionary<T, string> choices, bool includeCancel)
        {
            // Is any client focusing on this character? If not then no message is sent.
            var focusedClient = AttachedClients.GetCharacterFocusedClient(this.TrackingId);
            if (focusedClient == null)
            {
                return default(T);
            }

            return focusedClient.Choose(prompt, choices, includeCancel);
        }

        /// <summary>
        /// Gets the current location of the character
        /// </summary>
        /// <returns></returns>
        public Location GetLocation()
        {
            return GameState.CurrentGameState.GetCharacterLocation(this.TrackingId);
        }

        public Client GetClient()
        {
            return AttachedClients.GetCharacterFocusedClient(this.TrackingId);
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
                    var behavior = GameState.CurrentGameState.GetTurnBehavior(turnBehaviorName);
                    behavior?.Turn(this);
                }
            }
        }

        public bool HasPromptingBehaviors()
        {
            if (TurnBehaviors != null)
            {
                foreach (var turnBehaviorName in TurnBehaviors)
                {
                    var behavior = GameState.CurrentGameState.GetTurnBehavior(turnBehaviorName);
                    if (behavior.HasPromps)
                    {
                        return true;
                    }
                }
            }

            return false;
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

        /// <summary>
        /// Gets all items the character is holding in their inventory that can be used
        /// </summary>
        /// <returns></returns>
        public List<KeyValuePair<Item, int>> GetUseableInventoryItems()
        {
            var useableCharacterItems = GameState.CurrentGameState.GetCharacterItems(this.TrackingId)
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

            PublicCommandHelper.TryRunPublicCommand("use", extraWords, this);
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
            PublicCommandHelper.TryRunPublicCommand("grab", extraWords, this);
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
            PublicCommandHelper.TryRunPublicCommand("drop", extraWords, this);
        }

        /// <summary>
        /// Makes the character run the attack command on the specified character.
        /// </summary>
        /// <param name="characterToAttack">The character that this character should attack</param>
        public void ExecuteAttackCommand(Character characterToAttack)
        {
            var extraWords = new List<string>() { characterToAttack.TrackingId.ToString() };
            PublicCommandHelper.TryRunPublicCommand("attack", extraWords, this);
        }

        /// <summary>
        /// Makes the character run the move command
        /// </summary>
        /// <param name="locationToMoveTo">The location to attempt to move to</param>
        public void ExecuteMoveCommand(Location locationToMoveTo)
        {
            var extraWords = new List<string>() { locationToMoveTo.TrackingId.ToString() };
            PublicCommandHelper.TryRunPublicCommand("move", extraWords, this);
        }

        /// <summary>
        /// Makes the character run the look command
        /// </summary>
        public void ExecuteLookCommand()
        {
            PublicCommandHelper.TryRunPublicCommand("look", new List<string>(), this);
        }

        /// <summary>
        /// Makes the character run the stats command
        /// </summary>
        public void ExecuteStatsCommand()
        {
            PublicCommandHelper.TryRunPublicCommand("stats", new List<string>(), this);
        }

        /// <summary>
        /// Makes the character run the inventory command
        /// </summary>
        public void ExecuteInventoryCommand()
        {
            PublicCommandHelper.TryRunPublicCommand("inv", new List<string>(), this);
        }
    }
}
