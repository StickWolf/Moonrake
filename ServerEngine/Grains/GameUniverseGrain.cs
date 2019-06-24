using Newtonsoft.Json;
using Orleans;
using Orleans.Runtime;
using ServerEngine.Characters;
using ServerEngine.Characters.Behaviors;
using ServerEngine.Commands.GameCommands;
using ServerEngine.GrainInterfaces;
using ServerEngine.GrainStates;
using ServerEngine.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerEngine.Grains
{
    public class GameUniverseGrain : Grain<GameUniverseState>, IGameUniverseGrain
    {
        public Task<string> GetStats()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Id: {this.GetPrimaryKeyString()}");
            return Task.FromResult(sb.ToString());
        }

        public Task<string> GetGameIntroductionText()
        {
            return Task.FromResult(State.GameIntroductionText);
        }

        public Task SetGameIntroductionText(string value)
        {
            State.GameIntroductionText = value;
            return this.WriteStateAsync();
        }

        public Task<string> GetGameVarValue(string gameVariableName)
        {
            if (State.GameVars.ContainsKey(gameVariableName))
            {
                return Task.FromResult(State.GameVars[gameVariableName]);
            }
            return Task.FromResult<string>(null);
        }

        public async Task<string> SetGameVarValue(string gameVariableName, string value)
        {
            State.GameVars[gameVariableName] = value;
            await this.WriteStateAsync();

            return gameVariableName;
        }

        public async Task<Guid> AddTradeSet(TradeSet tradeSet)
        {
            State.TradeSets[tradeSet.TrackingId] = tradeSet;
            await WriteStateAsync();
            return tradeSet.TrackingId;
        }

        public Task<Guid> AddTradeSet(string tradesetName, params ItemRecipe[] itemRecipes)
        {
            var tradeSet = new TradeSet(tradesetName, itemRecipes);
            return AddTradeSet(tradeSet);
        }

        public Task<TradeSet> GetTradeSet(Guid tradeSetTrackingId)
        {
            if (State.TradeSets.ContainsKey(tradeSetTrackingId))
            {
                return Task.FromResult(State.TradeSets[tradeSetTrackingId]);
            }
            return Task.FromResult<TradeSet>(null);
        }

        public Task AddGameCommand(IGameCommand command)
        {
            State.GameCommands.Add(command);
            return this.WriteStateAsync();
        }

        public Task<IGameCommand> GetGameCommand(string commandName, List<string> accountPermissions)
        {
            if (string.IsNullOrWhiteSpace(commandName))
            {
                return Task.FromResult<IGameCommand>(null);
            }

            var command = State.GameCommands
                .Where(c => c.ActivatingWords.Any(w => w.Equals(commandName, StringComparison.OrdinalIgnoreCase)))
                .FirstOrDefault(c => c.PermissionNeeded == null || accountPermissions.Contains(c.PermissionNeeded, StringComparer.OrdinalIgnoreCase));

            return Task.FromResult(command);
        }

        public Task AddTurnBehavior(string behaviorName, ITurnBehavior behavior)
        {
            State.TurnBehaviors[behaviorName] = behavior;
            return this.WriteStateAsync();
        }

        public Task<ITurnBehavior> GetTurnBehavior(string behaviorName)
        {
            if (behaviorName == null)
            {
                return Task.FromResult<ITurnBehavior>(null);
            }

            if (State.TurnBehaviors.ContainsKey(behaviorName))
            {
                return Task.FromResult(State.TurnBehaviors[behaviorName]);
            }
            return null;
        }

        public Task<Guid> AddTradePost(Guid locationTrackingId, string tradePostName, params Guid[] tradeSetTrackingIds)
        {
            var tradePost = new TradePost(tradePostName, tradeSetTrackingIds);
            return AddTradePost(tradePost, locationTrackingId);
        }

        public async Task<Guid> AddTradePost(TradePost tradePost, Guid locationTrackingId)
        {
            State.TradePosts[tradePost.TrackingId] = tradePost;
            State.CurrentTradePostLocations[tradePost.TrackingId] = locationTrackingId;
            await this.WriteStateAsync();
            return tradePost.TrackingId;
        }

        public Task<TradePost> GetTradePost(Guid tradePostTrackingId)
        {
            if (State.TradePosts.ContainsKey(tradePostTrackingId))
            {
                return Task.FromResult(State.TradePosts[tradePostTrackingId]);
            }
            return null;
        }

        public Task SetTradePostLocation(Guid tradePostTrackingId, Guid locationTrackingId)
        {
            State.CurrentTradePostLocations[tradePostTrackingId] = locationTrackingId;
            return this.WriteStateAsync();
        }

        public Task<List<TradePost>> GetTradePostsAtLocation(Guid locationTrackingId)
        {
            var tradePostNames = State.CurrentTradePostLocations
                .Where(kvp => kvp.Value.Equals(locationTrackingId))
                .Select(kvp => GetTradePost(kvp.Key).Result)
                .ToList();

            return Task.FromResult(tradePostNames);
        }

        public Task<List<Character>> GetCharactersInLocation(Guid locationTrackingId)
        {
            var characters = State.CharacterLocations
                .Where(kvp => kvp.Value == locationTrackingId) // Where location is the one passed in
                .Select(kvp => GetCharacter(kvp.Key).Result)
                .Where(c => c.IsPresentInWorld());

            return Task.FromResult(characters.ToList());
        }

        public Task<Location> GetCharacterLocation(Guid characterTrackingId)
        {
            if (State.CharacterLocations.ContainsKey(characterTrackingId))
            {
                var locationTrackingId = State.CharacterLocations[characterTrackingId];
                return Task.FromResult(State.Locations[locationTrackingId]);
            }
            return Task.FromResult<Location>(null);
        }

        public Task SetCharacterLocation(Guid characterTrackingId, Guid locationTrackingId)
        {
            State.CharacterLocations[characterTrackingId] = locationTrackingId;
            return this.WriteStateAsync();
        }

        public async Task<Guid> AddCharacter(Character character, Guid locationTrackingId)
        {
            State.Characters.Add(character.TrackingId, character);
            State.CharacterLocations[character.TrackingId] = locationTrackingId;
            await this.WriteStateAsync();
            return character.TrackingId;
        }

        public Task<Character> GetCharacter(Guid characterTrackingId)
        {
            if (State.Characters.ContainsKey(characterTrackingId))
            {
                return Task.FromResult(State.Characters[characterTrackingId]);
            }
            return Task.FromResult<Character>(null);
        }

        public Task<Dictionary<Item, int>> GetCharacterItems(Guid characterTrackingId)
        {
            if (State.CharactersItems.ContainsKey(characterTrackingId))
            {
                var characterItems = State.CharactersItems[characterTrackingId]
                    .ToDictionary(kvp => GetItem(kvp.Key).Result, kvp => kvp.Value);
                return Task.FromResult(characterItems);
            }
            else
            {
                return Task.FromResult(new Dictionary<Item, int>());
            }
        }

        public Task<Dictionary<Item, int>> GetLocationItems(Guid locationTrackingId)
        {
            if (State.LocationItems.ContainsKey(locationTrackingId))
            {
                var locationItems = State.LocationItems[locationTrackingId]
                    .ToDictionary(kvp => GetItem(kvp.Key).Result, kvp => kvp.Value);
                return Task.FromResult(locationItems);
            }
            else
            {
                return Task.FromResult(new Dictionary<Item, int>());
            }
        }

        public Task<Character> GetNextTurnNPC()
        {
            var nextCharacter = State.Characters.Values
                .FirstOrDefault(c => !c.NeedsFocus && c.CanTakeTurn() && c.IsPresentInWorld());
            return  Task.FromResult(nextCharacter);
        }

        public async Task<Guid> AddItem(Item item)
        {
            State.Items[item.TrackingId] = item;
            await this.WriteStateAsync();
            return item.TrackingId;
        }

        public Task<Item> GetItem(Guid itemTrackingId)
        {
            var res = State.Items.ContainsKey(itemTrackingId) ? State.Items[itemTrackingId] : null;
            return Task.FromResult<Item>(res);
        }

        public Task<bool> IsPlayerCharacterNameInUse(string characterName)
        {
            var res = State.Characters.Values.Any(c => c.NeedsFocus && c.Name.Equals(characterName, StringComparison.OrdinalIgnoreCase));
            return Task.FromResult(res);
        }

        public Task<Guid> AddPortal(params PortalRule[] destinationRules)
        {
            var portal = new Portal(destinationRules.ToList());
            return AddPortal(portal);
        }

        public async Task<Guid> AddPortal(Portal portal)
        {
            State.Portals[portal.TrackingId] = portal;
            await this.WriteStateAsync();
            return portal.TrackingId;
        }

        public Task<Portal> GetPortal(Guid portalTrackingId)
        {
            var res = State.Portals.ContainsKey(portalTrackingId) ? State.Portals[portalTrackingId] : null;
            return Task.FromResult<Portal>(res);
        }

        public Task<List<Portal>> GetPortalsInLocation(Guid originLocationTrackingId)
        {
            var portals = State.Portals.Values
                .Where(p => p.HasOriginLocation(originLocationTrackingId))
                .ToList();
            return Task.FromResult(portals);
        }

        public Task RemoveItemEveryWhere(Guid itemTrackingId)
        {
            // Remove the item from all characters
            foreach (var characterTrackingId in State.CharactersItems.Keys)
            {
                if (State.CharactersItems[characterTrackingId].ContainsKey(itemTrackingId))
                {
                    State.CharactersItems[characterTrackingId].Remove(itemTrackingId);
                }
            }
            var zeroCharKeys = State.CharactersItems.Where(kvp => !kvp.Value.Any()).Select(kvp => kvp.Key).ToList();
            foreach (var key in zeroCharKeys)
            {
                State.CharactersItems.Remove(key);
            }

            // Remove the item from all locations
            foreach (var locationName in State.LocationItems.Keys)
            {
                if (State.LocationItems[locationName].ContainsKey(itemTrackingId))
                {
                    State.LocationItems[locationName].Remove(itemTrackingId);
                }
            }
            var zeroLocationKeys = State.LocationItems.Where(kvp => !kvp.Value.Any()).Select(kvp => kvp.Key).ToList();
            foreach (var key in zeroLocationKeys)
            {
                State.LocationItems.Remove(key);
            }
            return this.WriteStateAsync();
        }

        public async Task<bool> TryAddCharacterItemCount(Guid characterTrackingId, Guid itemTrackingId, int count)
        {
            var item = GetItem(itemTrackingId).Result;
            if (item == null)
            {
                return false;
            }

            if (count == 0)
            {
                return true;
            }

            void setCharacterItemCount(Guid sCharacterTrackingId, Guid sItemTrackingId, int sCount)
            {
                if (!State.CharactersItems.ContainsKey(sCharacterTrackingId))
                {
                    State.CharactersItems.Add(sCharacterTrackingId, new Dictionary<Guid, int>());
                }
                if (!State.CharactersItems[sCharacterTrackingId].ContainsKey(sItemTrackingId))
                {
                    State.CharactersItems[sCharacterTrackingId].Add(sItemTrackingId, sCount);
                }
                else
                {
                    State.CharactersItems[sCharacterTrackingId][sItemTrackingId] = sCount;
                }
            }

            void removeCharacterItem(Guid sCharacterTrackingId, Guid sItemTrackingId)
            {
                if (!State.CharactersItems.ContainsKey(sCharacterTrackingId))
                {
                    return;
                }
                if (State.CharactersItems[sCharacterTrackingId].ContainsKey(sItemTrackingId))
                {
                    State.CharactersItems[sCharacterTrackingId].Remove(sItemTrackingId);
                    if (!State.CharactersItems[sCharacterTrackingId].Any())
                    {
                        State.CharactersItems.Remove(sCharacterTrackingId);
                    }
                }
            }

            // The goal is to only keep records in the dictionary for counts greater than 0
            // Even if we do temporarily add keys here, they should be cleaned up before returning if needed
            var characterItemCount = 0;
            if (State.CharactersItems.ContainsKey(characterTrackingId) && State.CharactersItems[characterTrackingId].ContainsKey(itemTrackingId))
            {
                characterItemCount = State.CharactersItems[characterTrackingId][itemTrackingId];
            }

            // Non-unique items can be added and removed from characters without worrying about the count
            if (!item.IsUnique)
            {
                characterItemCount += count;
                if (characterItemCount > 0)
                {
                    setCharacterItemCount(characterTrackingId, itemTrackingId, characterItemCount);
                    await this.WriteStateAsync();
                    return true;
                }
                else if (characterItemCount == 0)
                {
                    removeCharacterItem(characterTrackingId, itemTrackingId);
                    await this.WriteStateAsync();
                    return true;
                }
                await this.WriteStateAsync();
                return false;
            }

            // The item is unique, and we're removing it.
            // Since this is a removal we don't need to worry about checking for duplicates being made
            if (count < 0)
            {
                if (characterItemCount > 0)
                {
                    removeCharacterItem(characterTrackingId, itemTrackingId);
                    await this.WriteStateAsync();
                    return true;
                }
                await this.WriteStateAsync();
                return false;
            }

            // The only other option is that this is a unique item being added to the character
            // First remove it from everywhere else, then add it here.
            await RemoveItemEveryWhere(itemTrackingId);
            setCharacterItemCount(characterTrackingId, itemTrackingId, 1);
            await this.WriteStateAsync();
            return true;
        }

        public async Task<bool> TryAddLocationItemCount(Guid locationTrackingId, Guid itemTrackingId, int count)
        {
            var item = GetItem(itemTrackingId).Result;
            if (item == null)
            {
                return false;
            }

            if (count == 0)
            {
                return true;
            }

            void setLocationItemCount(Guid sLocationTrackingId, Guid sItemTrackingId, int sCount)
            {
                if (!State.LocationItems.ContainsKey(sLocationTrackingId))
                {
                    State.LocationItems.Add(sLocationTrackingId, new Dictionary<Guid, int>());
                }
                if (!State.LocationItems[sLocationTrackingId].ContainsKey(sItemTrackingId))
                {
                    State.LocationItems[sLocationTrackingId].Add(sItemTrackingId, sCount);
                }
                else
                {
                    State.LocationItems[sLocationTrackingId][sItemTrackingId] = sCount;
                }
            }

            void removeLocationItem(Guid sLocationTrackingId, Guid sItemTrackingId)
            {
                if (!State.LocationItems.ContainsKey(sLocationTrackingId))
                {
                    return;
                }
                if (State.LocationItems[sLocationTrackingId].ContainsKey(sItemTrackingId))
                {
                    State.LocationItems[sLocationTrackingId].Remove(sItemTrackingId);
                    if (!State.LocationItems[sLocationTrackingId].Any())
                    {
                        State.LocationItems.Remove(sLocationTrackingId);
                    }
                }
            }

            // The goal is to only keep records in the dictionary for counts greater than 0
            // Even if we do temporarily add keys here, they should be cleaned up before returning if needed
            var locationItemCount = 0;
            if (State.LocationItems.ContainsKey(locationTrackingId) && State.LocationItems[locationTrackingId].ContainsKey(itemTrackingId))
            {
                locationItemCount = State.LocationItems[locationTrackingId][itemTrackingId];
            }

            // Non-unique items can be added and removed from locations without worrying about the count
            if (!item.IsUnique)
            {
                locationItemCount += count;
                if (locationItemCount > 0)
                {
                    setLocationItemCount(locationTrackingId, itemTrackingId, locationItemCount);
                    await this.WriteStateAsync();
                    return true;
                }
                else if (locationItemCount == 0)
                {
                    removeLocationItem(locationTrackingId, itemTrackingId);
                    await this.WriteStateAsync();
                    return true;
                }
                await this.WriteStateAsync();
                return false;
            }

            // The item is unique, and we're removing it.
            // Since this is a removal we don't need to worry about checking for duplicates being made
            if (count < 0)
            {
                // Double check to make sure there really is an item here
                if (locationItemCount > 0)
                {
                    removeLocationItem(locationTrackingId, itemTrackingId);
                    await this.WriteStateAsync();
                    return true;
                }
                await this.WriteStateAsync();
                return false;
            }

            // The only other option is that this is a unique item being added to a location
            // First remove it from everywhere else, then add it here.
            await RemoveItemEveryWhere(itemTrackingId);
            setLocationItemCount(locationTrackingId, itemTrackingId, 1);
            await this.WriteStateAsync();
            return true;
        }

        /// <summary>
        /// Clones the specified item so that there are 2 instances of it in the Items.
        /// The clone won't have any references to it and is free to mutate directly after cloning.
        /// The clone will have a new tracking Id assigned.
        /// </summary>
        /// <param name="sourceItemTrackingId">The source item tracking id to clone</param>
        /// <returns>The cloned item</returns>
        public Task ConvertItemToClone(Guid sourceItemTrackingId)
        {
            var sourceItem = GetItem(sourceItemTrackingId).Result;
            State.Items.Remove(sourceItem.TrackingId);

            // Cloning just means serializing and deserializing to a new instance
            var serializerSettings = GetJsonSerializerSettings();
            string serializedItem = JsonConvert.SerializeObject(sourceItem, Formatting.Indented, serializerSettings);
            var clonedItem = JsonConvert.DeserializeObject<Item>(serializedItem, serializerSettings);

            // The source gets the new id and becomes unlinked (not refrenced by location or character item counts)
            // This allows the source item to call this method, then to safely mutate itself.
            sourceItem.TrackingId = Guid.NewGuid();

            AddItem(sourceItem).Wait();
            AddItem(clonedItem).Wait();

            return Task.CompletedTask;
        }

        public async Task DedupeItems()
        {
            bool dupeFound = true;
            while (dupeFound)
            {
                dupeFound = false;
                foreach (var itemA in State.Items.Values)
                {
                    foreach (var itemB in State.Items.Values)
                    {
                        // If these are actually the same item instance then ignore
                        if (itemA.TrackingId == itemB.TrackingId)
                        {
                            continue;
                        }

                        if (EqualChecker.AreEqual(itemA, itemB))
                        {
                            dupeFound = true;

                            // TODO: Not sure how we would update refs to itemB that are found in other object properties
                            // TODO: Maybe we can raise an event here that can be subscribed to
                            State.Items.Remove(itemB.TrackingId);
                            await StackItems(itemB.TrackingId, itemA.TrackingId);
                            break;
                        }
                    }
                    if (dupeFound)
                    {
                        break;
                    }
                }
            }
            await this.WriteStateAsync();
        }

        public async Task<Guid> AddLocation(Location location)
        {
            State.Locations.Add(location.TrackingId, location);
            await this.WriteStateAsync();
            return location.TrackingId;
        }

        public Task<Location> GetLocation(Guid locationTrackingId)
        {
            var res = State.Locations.ContainsKey(locationTrackingId) ? State.Locations[locationTrackingId] : null;
            return Task.FromResult(res);
        }

        /// <summary>
        /// Gets a list of locations that are currently available to move into from the specified location
        /// </summary>
        /// <param name="originLocationTrackingId">The origin location tracking id</param>
        /// <returns>A list of open portals</returns>
        public Task<List<Location>> GetConnectedLocations(Guid originLocationTrackingId)
        {
            var validLocations = State.Portals.Values
            .Where(p => p.HasOriginLocation(originLocationTrackingId)) // Portals in the specified location
            .Select(p => p.GetDestination(originLocationTrackingId)) // Get destination info for each of the portals
            .Where(d => d.DestinationTrackingId != Guid.Empty) // Exclude destinations that lead nowhere (locked doors, etc)
            .Select(d => GetLocation(d.DestinationTrackingId).Result) // Get the actual destination location
            .ToList();
            return Task.FromResult(validLocations);
        }

        public Task SetCustom(object custom)
        {
            State.Custom = custom;
            return this.WriteStateAsync();
        }

        public Task<object> GetCustom()
        {
            return Task.FromResult(State.Custom);
        }

        private async Task StackItems(Guid removeItemTrackingId, Guid receiveItemTrackingId)
        {
            foreach (var characterTrackingId in State.CharactersItems.Keys)
            {
                if (State.CharactersItems[characterTrackingId] == null)
                {
                    continue;
                }

                if (!State.CharactersItems[characterTrackingId].ContainsKey(removeItemTrackingId))
                {
                    continue;
                }

                var removedCount = State.CharactersItems[characterTrackingId][removeItemTrackingId];
                State.CharactersItems[characterTrackingId].Remove(removeItemTrackingId);
                if (State.CharactersItems[characterTrackingId].ContainsKey(receiveItemTrackingId))
                {
                    State.CharactersItems[characterTrackingId][receiveItemTrackingId] += removedCount;
                }
                else
                {
                    State.CharactersItems[characterTrackingId][receiveItemTrackingId] = removedCount;
                }
            }

            foreach (var locationTrackingId in State.LocationItems.Keys)
            {
                if (State.LocationItems[locationTrackingId] == null)
                {
                    continue;
                }

                if (!State.LocationItems[locationTrackingId].ContainsKey(removeItemTrackingId))
                {
                    continue;
                }

                var removedCount = State.LocationItems[locationTrackingId][removeItemTrackingId];
                State.LocationItems[locationTrackingId].Remove(removeItemTrackingId);
                if (State.LocationItems[locationTrackingId].ContainsKey(receiveItemTrackingId))
                {
                    State.LocationItems[locationTrackingId][receiveItemTrackingId] += removedCount;
                }
                else
                {
                    State.LocationItems[locationTrackingId][receiveItemTrackingId] = removedCount;
                }
            }

            await this.WriteStateAsync();
        }

        private JsonSerializerSettings GetJsonSerializerSettings()
        {
            var serializerSettings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All,
                ContractResolver = new JsonPrivateSetResolver()
            };
            return serializerSettings;
        }
    }
}
