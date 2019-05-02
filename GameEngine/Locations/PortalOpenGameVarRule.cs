using Newtonsoft.Json;
using System;

namespace GameEngine.Locations
{
    /// <summary>
    /// This portal rule is used if the specified game variable equals the expected value
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class PortalOpenGameVarRule : PortalRule
    {
        [JsonProperty]
        public string GameVarName { get; private set; }

        [JsonProperty]
        public string ExpectedValue { get; private set; }

        public PortalOpenGameVarRule(Guid originTrackingId, Guid destinationTrackingId, string description, string gameVarName, string expectedValue)
            : base(originTrackingId, destinationTrackingId, description)
        {
            GameVarName = gameVarName;
            ExpectedValue = expectedValue;
        }
    }
}
