using ServerEngine.Characters;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerEngine.Commands.GameCommands
{
    public class InventoryCommand : IGameCommand
    {
        public List<string> ActivatingWords => new List<string>() { "inv", "inventory" };

        public string PermissionNeeded => null;

        public void Execute(List<string> extraWords, Character inventorySeekingCharacter)
        {
            var characterItems = GameState.CurrentGameState.GetCharacterItems(inventorySeekingCharacter.TrackingId);
            if (characterItems == null || !characterItems.Any())
            {
                inventorySeekingCharacter.SendDescriptiveTextDtoMessage("You have no items.");
                inventorySeekingCharacter.GetLocation().SendDescriptiveTextDtoMessage($"{inventorySeekingCharacter.Name} is looking at their inventory.", inventorySeekingCharacter);
                return;
            }

            StringBuilder inventoryBuilder = new StringBuilder();
            inventoryBuilder.AppendLine("You are currently holding:");
            foreach (var characterItem in characterItems)
            {
                var item = characterItem.Key;
                // Don't show invisible items
                if (!item.IsVisible)
                {
                    continue;
                }

                var description = item.GetDescription(characterItem.Value).UppercaseFirstChar();
                inventoryBuilder.AppendLine(description);
            }

            inventorySeekingCharacter.SendDescriptiveTextDtoMessage(inventoryBuilder.ToString());
            inventorySeekingCharacter.GetLocation().SendDescriptiveTextDtoMessage($"{inventorySeekingCharacter.Name} is looking at their inventory.", inventorySeekingCharacter);
        }
    }
}
