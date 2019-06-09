using Orleans;
using ServerEngine.GrainInterfaces;
using System.Threading.Tasks;

namespace ServerEngine.Grains
{
    public class ItemGrain : Grain, IItemGrain
    {
        string displayName;

        public Task<string> GetDescription(int count)
        {
            string result = (count == 1) ? $"a {displayName}" : $"{count} {displayName}s";
            return Task.FromResult(result);
        }

        public Task SetDisplayName(string name)
        {
            displayName = name;
            return Task.CompletedTask;
        }
    }
}
