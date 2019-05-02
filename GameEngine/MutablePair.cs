using Newtonsoft.Json;

namespace GameEngine
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class MutablePair<T1,T2>
    {
        [JsonProperty]
        public T1 Key { get; set; }

        [JsonProperty]
        public T2 Value { get; set; }

        public MutablePair(T1 key, T2 value)
        {
            Key = key;
            Value = value;
        }
    }
}
