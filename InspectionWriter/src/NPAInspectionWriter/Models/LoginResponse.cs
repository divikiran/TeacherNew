using System;
using Newtonsoft.Json;
using NPAInspectionWriter.Helpers;

namespace NPAInspectionWriter.iOS.Models
{
    public class LoginResponse
    {
        public string Token { get; set; }

        public Guid LocationId { get; set; }

        public bool Valid { get; set; }

        [JsonIgnore]
        public NPALocation Location
        {
            get { return NPAConstants.GetLocationById( LocationId ); }
        }

        //public Guid EntityLinkId { get; set; }

        //public int EntityLinkTypeId { get; set; }
    }
}
