using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace NPAInspectionWriter.Helpers
{
    public class NPAConstants
    {
        static NPAConstants()
        {
            if( s_locations?.Count == 0 )
                SetLocations();
        }

        public const string StockNumberValidationPattern = @"^\d{8}$";


        public const string SimpleVinValidationPattern = "^[a-zA-Z0-9]{5,17}$";

        public static ReadOnlyCollection<NPALocation> NPALocations
        {
            get { return new ReadOnlyCollection<NPALocation>( s_locations ); }
        }

        public static string LookupLocationCodeById( Guid id )
        {
            var first = s_locations.FirstOrDefault( loc => loc.Id == id );
            return s_locations.FirstOrDefault( loc => loc.Id == id ).Code ?? string.Empty;
        }

        public static NPALocation GetLocationById( Guid id ) =>
            s_locations.FirstOrDefault( loc => loc.Id == id );

        private static void SetLocations()
        {
            s_locations = new List<NPALocation>()
            {
                new NPALocation()
                {
                    Id = new Guid( "e7608c69-dd79-dd11-aca2-0019b9b35da2" ),
                    Name = "HUB LOCATION",
                    Code = "H",
                    IsPhysicalLocation = false
                },
                new NPALocation()
                {
                    Id = new Guid( "1bfcb8d2-0bbf-dc11-ade2-0019b9b35da2" ),
                    Name = "Cincinnati",
                    Code = "CIN",
                    IsPhysicalLocation = true
                },
                new NPALocation()
                {
                    Id = new Guid( "c4b1d148-57e2-4fd2-9dae-0121d40854a0" ),
                    Name = "San Diego",
                    Code = "SAN",
                    IsPhysicalLocation = true
                },
                new NPALocation()
                {
                    Id = new Guid( "da24167c-c69f-45c3-b95c-0776cebd135e" ),
                    Name = "Remote Dallas",
                    Code = "RDAL",
                    IsPhysicalLocation = false
                },
                new NPALocation()
                {
                    Id = new Guid( "744abc97-d2a1-4eb5-9d8f-265eb86c18bb" ),
                    Name = "Dallas",
                    Code = "DAL",
                    IsPhysicalLocation = true
                },
                new NPALocation()
                {
                    Id = new Guid( "3ab6e74e-b9d5-470c-b37f-3e4fb55de6cd" ),
                    Name = "Unassigned",
                    IsPhysicalLocation = false
                },
                new NPALocation()
                {
                    Id = new Guid( "bcaa6410-dc7c-4970-b815-703f1027d574" ),
                    Name = "Atlanta",
                    Code = "ATL",
                    IsPhysicalLocation = true
                },
                new NPALocation()
                {
                    Id = new Guid( "11c8face-679b-4a1c-bffa-70868aec5760" ),
                    Name = "Columbus",
                    Code = "OH",
                    IsPhysicalLocation = false
                },
                new NPALocation()
                {
                    Id = new Guid( "6263c70b-6630-4663-ba8b-70e8d92dc0b6" ),
                    Name = "Simulcast Only",
                    Code = "RSAN",
                    IsPhysicalLocation = false
                },
                new NPALocation()
                {
                    Id = new Guid( "03f75604-8dd9-4a85-82b9-82065819be0a" ),
                    Name = "Penske",
                    Code = "PAG",
                    IsPhysicalLocation = false
                },
                new NPALocation()
                {
                    Id = new Guid( "84430d4e-c7bc-4725-91be-cf06b31302ff" ),
                    Name = "Remote Atlanta",
                    Code = "RATL",
                    IsPhysicalLocation = false
                },
                new NPALocation()
                {
                    Id = new Guid( "e660c79e-eb52-402a-aaed-cfd7fffceae4" ),
                    Name = "NPA Virtual Auction",
                    Code = "NPAV",
                    IsPhysicalLocation = false
                },
                new NPALocation()
                {
                    Id = new Guid( "1a915e72-0b82-4184-b14e-cfda341a3372" ),
                    Name = "Remote Cincinnati",
                    Code = "RCIN",
                    IsPhysicalLocation = false
                },
                new NPALocation()
                {
                    Id = new Guid( "923d12b2-f63a-4313-b9b6-d52d9c5f565c" ),
                    Name = "Remote",
                    Code = "REM",
                    IsPhysicalLocation = false
                },
                new NPALocation()
                {
                    Id = new Guid( "c21639bf-3421-4575-8a68-e5b51cb83294" ),
                    Name = "HDDX",
                    Code = "HD",
                    IsPhysicalLocation = false
                },
                new NPALocation()
                {
                    Id = new Guid( "8F7750F9-B5F2-49C5-B972-8245602E06C0" ),
                    Name = "Philadelphia",
                    Code = "PHL",
                    IsPhysicalLocation = true
                },
            };
        }

        private static IList<NPALocation> s_locations = new List<NPALocation>();
    }
}
