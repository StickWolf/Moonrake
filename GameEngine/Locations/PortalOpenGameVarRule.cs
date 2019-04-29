using System;

namespace GameEngine.Locations
{
    /// <summary>
    /// This portal rule is used if the specified game variable equals the expected value
    /// </summary>
    public class PortalOpenGameVarRule : PortalRule
    {
        public string GameVarName { get; private set; }

        public string ExpectedValue { get; private set; }

        public PortalOpenGameVarRule(Guid originTrackingId, Guid destinationTrackingId, string description, string gameVarName, string expectedValue)
            : base(originTrackingId, destinationTrackingId, description)
        {
            GameVarName = gameVarName;
            ExpectedValue = expectedValue;
        }
    }
}
