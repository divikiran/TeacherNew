using System;
using Xunit;
using InspectionWriterWebApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit.Abstractions;
using System.Collections.Generic;
using Xunit.Extensions;
using InspectionWriterWebApi.Common.Tests.Data;

namespace InspectionWriterWebApi.Common.Tests
{
    public class AmsUserTests
    {
        private readonly ITestOutputHelper output;

        public AmsUserTests( ITestOutputHelper outputHelper )
        {
            output = outputHelper;
        }

        [Theory]
        [ClassData( typeof( AmsUserData ) )]
        public void AmsUserSerialization( string jsonResponse, AmsUser user )
        {
            var expected = JObject.Parse( jsonResponse );
            var actual = JObject.FromObject( user );
            output.WriteLine( actual.ToString() );

            Assert.Equal( expected, actual );
        }

        [Theory]
        [ClassData( typeof( AmsUserData ) )]
        public void AmsUserDeserialization( string jsonResponse, AmsUser user )
        {
            var response = JsonConvert.DeserializeObject<AmsUser>( jsonResponse );
            Assert.Equal( user.UserAccountId, response.UserAccountId );
            Assert.Equal( user.UserName, response.UserName );
            Assert.Equal( user.UserLevel, response.UserLevel );
            Assert.Equal( user.Location, response.Location );
            Assert.Equal( user.LocationId, response.LocationId );
            Assert.Equal( user.EmailAddress, response.EmailAddress );
            Assert.Equal( user.CurrentAppVersion, response.CurrentAppVersion );
            Assert.Equal( user.CurrentAppVersionRequiredDate, response.CurrentAppVersionRequiredDate );
            Assert.Equal( user.CanCreateInspections, response.CanCreateInspections );
            //Assert.Equal( user, JsonConvert.DeserializeObject<AmsUser>( jsonResponse ) );
        }
    }
}
