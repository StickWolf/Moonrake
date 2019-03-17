namespace GameEngine.Locations
{
    /// <summary>
    /// A basic location connection is always connected
    /// </summary>
    public class LocationConnectionBasic : LocationConnection
    {
        public LocationConnectionBasic(string locationName) : base(locationName) { }
    }
}
