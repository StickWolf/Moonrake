namespace GameEngine.Locations
{
    /// <summary>
    /// This portal rule will always be used if encountered.
    /// Any rules after this in a portal rules list would essentially be ignored.
    /// This rule represents a portal that is open without condition.
    /// </summary>
    public class PortalDestinationAlwaysOpenRule : PortalDestinationRule
    {
        public PortalDestinationAlwaysOpenRule(string destination, string description)
            : base(destination, description)
        {
        }
    }
}
