namespace GameEngine.Locations
{
    /// <summary>
    /// This location is only visible when the specified game var is set properly
    /// </summary>
    public class LocationConnectionGameVar : LocationConnection
    {
        public string GameVarName { get; private set; }

        public string ExpectedSetting { get; private set; }

        public LocationConnectionGameVar(string locationName, string gameVarName, string expectedSetting)
            : base(locationName)
        {
            GameVarName = GameVarName;
            ExpectedSetting = expectedSetting;
        }
    }
}
