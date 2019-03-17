namespace GameEngine.Locations
{
    public abstract class LocationConnection
    {
        public string LocationaName { get; private set; }

        public LocationConnection(string locationName)
        {
            LocationaName = locationName;
        }
    }
}
