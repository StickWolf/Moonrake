using System;

namespace ServerEngine.Locations
{
    /// <summary>
    /// This portal rule will always be used if encountered.
    /// Any rules after this in a portal rules list would essentially be ignored.
    /// This rule represents a portal that is closed without condition.
    /// </summary>
    public class PortalAlwaysClosedRule : PortalRule
    {
        public PortalAlwaysClosedRule(Guid originTrackingId, Guid destinationTrackingId, string description)
            : base(originTrackingId, destinationTrackingId, description)
        {
        }
    }
}
