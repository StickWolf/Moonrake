namespace GameEngine.Locations
{
    /// <summary>
    /// This portal rule is used if the specified game variable equals the expected value
    /// </summary>
    public class PortalDestinationOpenGameVarRule : PortalDestinationRule
    {
        public string GameVarName { get; private set; }

        public string ExpectedValue { get; private set; }

        public PortalDestinationOpenGameVarRule(string destination, string description, string gameVarName, string expectedValue)
            : base(destination, description)
        {
            GameVarName = gameVarName;
            ExpectedValue = expectedValue;
        }
    }
}
