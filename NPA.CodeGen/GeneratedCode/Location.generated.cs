using System;
using System.Linq;
using System.Collections.Generic;

namespace NPA.CodeGen
{
    public class Location : IEquatable<Location>, IComparable<Location>
    {
        public Guid LocationId { get; private set; }
        public string DisplayName { get; private set; }
		public string ShortName { get; private set; }
        public bool IsPhysicalLocation { get; private set; }
		public string BaseImageUrl { get; private set; }

        private Location() { }

        private Location(Guid locationId, string displayName, string shortName, bool isPhysicalLocation, string baseImageUrl)
        {
            LocationId = locationId;
            DisplayName = displayName;
			ShortName = shortName;
            IsPhysicalLocation = isPhysicalLocation;
			BaseImageUrl = baseImageUrl;
        }

        public bool Equals(Location other)
        {
            return this.LocationId.Equals(other.LocationId);
        }

        public int CompareTo(Location other)
        {
            return this.ShortName.CompareTo(other.ShortName);
        }

        public override string ToString()
        {
            return DisplayName;
        }

        public static Location FromGuid(Guid locationId)
        {
            return GetValues().Single(l => l.LocationId == locationId);
        }

		public static Location FromShortName(string shortName)
		{
		   return GetValues().SingleOrDefault(v => string.Compare(v.ShortName, shortName, true) == 0);
		}

		public static Location FromDisplayName(string displayName)
		{
		   return GetValues().SingleOrDefault(v => string.Compare(v.DisplayName, displayName, true) == 0);
		}

        public static Location Atlanta = new Location(new Guid("bcaa6410-dc7c-4970-b815-703f1027d574"), "Atlanta", "Atlanta", true, "http://amsatl.npauctions.com/amsproto/library/image.aspx");

        public static Location Cincinnati = new Location(new Guid("1bfcb8d2-0bbf-dc11-ade2-0019b9b35da2"), "Cincinnati", "Cincinnati", true, "http://amscin.npauctions.com/amsproto/library/image.aspx");

        public static Location Dallas = new Location(new Guid("744abc97-d2a1-4eb5-9d8f-265eb86c18bb"), "Dallas", "Dallas", true, "http://amsdal.npauctions.com/amsproto/library/image.aspx");

        public static Location Philadelphia = new Location(new Guid("8f7750f9-b5f2-49c5-b972-8245602e06c0"), "Philadelphia", "Philadelphia", true, "http://amsphl.npauctions.com/amsproto/library/image.aspx");

        public static Location SanDiego = new Location(new Guid("c4b1d148-57e2-4fd2-9dae-0121d40854a0"), "San Diego", "SanDiego", true, "http://amssan.npauctions.com/amsproto/library/image.aspx");

        public static Location AzureUsWest = new Location(new Guid("907bbfe3-416c-42d2-a8fd-7e5bda44a668"), "Azure Us West", "AzureUSWest", false, "http://amssan.npauctions.com/amsproto/library/image.aspx");

        public static Location HDDX = new Location(new Guid("c21639bf-3421-4575-8a68-e5b51cb83294"), "HDDX", "HDDX", false, "http://amssan.npauctions.com/amsproto/library/image.aspx");

        public static Location HubLocation = new Location(new Guid("e7608c69-dd79-dd11-aca2-0019b9b35da2"), "Hub Location", "HUBLOCATION", false, "http://amssan.npauctions.com/amsproto/library/image.asp");

        public static Location Penske = new Location(new Guid("03f75604-8dd9-4a85-82b9-82065819be0a"), "Penske", "Penske", false, "http://amssan.npauctions.com/amsproto/library/image.aspx");

        public static Location Remote = new Location(new Guid("923d12b2-f63a-4313-b9b6-d52d9c5f565c"), "Remote", "Remote", false, "http://amssan.npauctions.com/amsproto/library/image.aspx");

        public static Location RemoteAtlanta = new Location(new Guid("84430d4e-c7bc-4725-91be-cf06b31302ff"), "Remote Atlanta", "RemoteAtlanta", false, "http://amsatl.npauctions.com/amsproto/library/image.aspx");

        public static Location RemoteCincinnati = new Location(new Guid("1a915e72-0b82-4184-b14e-cfda341a3372"), "Remote Cincinnati", "RemoteCincinnati", false, "http://amscin.npauctions.com/amsproto/library/image.aspx");

        public static Location RemoteDallas = new Location(new Guid("da24167c-c69f-45c3-b95c-0776cebd135e"), "Remote Dallas", "RemoteDallas", false, "http://amsdal.npauctions.com/amsproto/library/image.aspx");

        public static Location SimulcastOnly = new Location(new Guid("6263c70b-6630-4663-ba8b-70e8d92dc0b6"), "Simulcast Only", "SimulcastOnly", false, "http://amssan.npauctions.com/amsproto/library/image.aspx");

        public static IEnumerable<Location> GetValues()
        {
            yield return Atlanta;
            yield return Cincinnati;
            yield return Dallas;
            yield return Philadelphia;
            yield return SanDiego;
            yield return AzureUsWest;
            yield return HDDX;
            yield return HubLocation;
            yield return Penske;
            yield return Remote;
            yield return RemoteAtlanta;
            yield return RemoteCincinnati;
            yield return RemoteDallas;
            yield return SimulcastOnly;
    
        }
    }
}
