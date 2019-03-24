using System.Collections.Generic;

namespace GameEngine.Locations
{
    /// <summary>
    /// A portal is something that can lead to another location.
    /// The location that a portal leads to is based on a set of rules.
    /// Example portals:
    ///     A standard door - This portal always leads to the same target location
    ///     A stairway - This portal always leads to the same target location
    ///     An elevator door - This portal leads to different locations depending on what floor the elevator is on.
    /// </summary>
    public class Portal
    {
        private List<PortalDestinationRule> DestinationRules { get; set; }

        public Portal(List<PortalDestinationRule> destinationRules)
        {
            DestinationRules = destinationRules;
        }

        public PortalDestinationDetails GetDestination()
        {
            var destDetails = new PortalDestinationDetails();

            foreach (var destRule in DestinationRules)
            {
                if (destRule is PortalDestinationAlwaysClosedRule || destRule is PortalDestinationAlwaysOpenRule)
                {
                    destDetails.Description = destRule.Description;
                    destDetails.Destination = destRule.Destination;
                    return destDetails;
                }
                else if (destRule is PortalDestinationOpenGameVarRule)
                {
                    var gaveVarRule = destRule as PortalDestinationOpenGameVarRule;

                    // TODO: when gavevars are added, actually do this check
                }
            }

            return destDetails;
        }
    }
}
