namespace GameEngine.Locations
{
    public class PortalDestinationDetails
    {
        /// <summary>
        /// The destination of where the portal leads to.
        /// Null if the portal is closed.
        /// </summary>
        public string Destination { get; set; }

        /// <summary>
        /// A description of what the portal looks like.
        /// </summary>
        public string Description { get; set; }
    }
}
