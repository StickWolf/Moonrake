namespace GameEngine.Locations
{
    /// <summary>
    /// A portal destination rule is tested to see if a portal links to the specified destination.
    /// A location can have multiple destination rules to determine what the actual destination is.
    /// </summary>
    public abstract class PortalDestinationRule
    {
        /// <summary>
        /// The destination of where the portal leads to if the rule matches.
        /// This may be null if the portal is closed.
        /// </summary>
        public string Destination { get; private set; }

        /// <summary>
        /// A description of what the portal looks like when the rule matches.
        /// This description should only define what the portal portion looks like
        /// and not the room beyond.
        /// </summary>
        public string Description { get; private set; }

        public PortalDestinationRule(string destination, string description)
        {
            Destination = destination;
            Description = description;
        }
    }
}
