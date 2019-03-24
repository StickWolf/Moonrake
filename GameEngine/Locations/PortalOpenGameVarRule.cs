namespace GameEngine.Locations
{
    /// <summary>
    /// This portal rule is used if the specified game variable equals the expected value
    /// </summary>
    public class PortalOpenGameVarRule : PortalRule
    {
        public string GameVarName { get; private set; }

        public string ExpectedValue { get; private set; }

        public PortalOpenGameVarRule(string origin, string destination, string description, string gameVarName, string expectedValue)
            : base(origin, destination, description)
        {
            GameVarName = gameVarName;
            ExpectedValue = expectedValue;
        }
    }
}
