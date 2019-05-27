using ServerEngine.Characters;
using System.Collections.Generic;
using System.Linq;

namespace ServerEngine.Commands.GameCommands
{
    public class InventoryCommand : IGameCommand
    {
        public List<string> ActivatingWords => new List<string>() { "inv", "inventory" };

        public string PermissionNeeded => null;

        public void Execute(List<string> extraWords, Character inventorySeekingCharacter)
        {
            // TODO: rewrite as server/client

            //var characterItems = GameState.CurrentGameState.GetCharacterItems(inventorySeekingCharacter.TrackingId);
            //if(characterItems == null || !characterItems.Any())
            //{
            //    inventorySeekingCharacter.SendMessage("You have no items.");
            //    inventorySeekingCharacter.GetLocation().SendMessage($"{inventorySeekingCharacter.Name} is looking at their inventory.", inventorySeekingCharacter);
            //    return;
            //}

            //inventorySeekingCharacter.SendMessage("You are currently holding:");
            //foreach (var characterItem in characterItems)
            //{
            //    var item = characterItem.Key;
            //    // Don't show invisible items
            //    if (!item.IsVisible)
            //    {
            //        continue;
            //    }

            //    var description = item.GetDescription(characterItem.Value).UppercaseFirstChar();
            //    inventorySeekingCharacter.SendMessage(description);
            //}
        }
    }
}
