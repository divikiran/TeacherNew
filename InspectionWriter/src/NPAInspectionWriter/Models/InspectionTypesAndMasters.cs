using System.Collections.Generic;
using System.Linq;
using NPAInspectionWriter.Models;
using Newtonsoft.Json;

namespace NPAInspectionWriter.Models
{
    [JsonObject]
    public class InspectionTypesAndMasters
    {
        [JsonProperty( "masters" )]
        public IEnumerable<InspectionMaster> Masters { get; set; }

        [JsonProperty( "types" )]
        public IEnumerable<InspectionType> Types { get; set; }

        public static implicit operator List<InspectionMasterRecord>( InspectionTypesAndMasters typesAndMasters )
        {
            var imrs = new List<InspectionMasterRecord>();

            if( typesAndMasters?.Masters == null || typesAndMasters.Masters.Count() < 1 ) return imrs;

            foreach( var master in typesAndMasters.Masters )
            {
                imrs.Add( new InspectionMasterRecord()
                {
                    MasterId = master.MasterId,
                    DisplayName = master.DisplayName,
                    MaxInspectionScore = master.MaxInspectionScore
                } );
            }

            return imrs;
        }

        public static implicit operator List<InspectionTypeRecord>( InspectionTypesAndMasters typesAndMasters )
        {
            var itrs = new List<InspectionTypeRecord>();

            if( typesAndMasters?.Types == null || typesAndMasters?.Types?.Count() < 1  ) return itrs;

            foreach( var type in typesAndMasters.Types )
            {
                itrs.Add( new InspectionTypeRecord()
                {
                    Id = type.InspectionTypeId,
                    DisplayName = type.DisplayName
                } );
            }

            return itrs;
        }
    }
}
