using Orleans;
using System.Threading.Tasks;

namespace ServerEngine.GrainInterfaces
{
    public interface IItemGrain : IGrainWithGuidKey
    {
        Task<string> GetDescription(int count);
        Task SetDisplayName(string name);
    }
}
