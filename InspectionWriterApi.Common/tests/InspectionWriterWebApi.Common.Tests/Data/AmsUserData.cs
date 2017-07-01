using InspectionWriterWebApi.Models;
using System;
using System.Collections;
using System.Collections.Generic;

namespace InspectionWriterWebApi.Common.Tests.Data
{
    public class AmsUserData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator() => _users.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        IEnumerable<object[]> _users
        {
            get
            {
                return new[]
                {
                    new object[]
                    {
                        "{\"userAccountId\":\"47e0ecd7-b056-4fd4-bc08-78feda408b62\",\"userName\":\"testuser\",\"userLevel\":99,\"location\":\"San Diego\",\"locationId\":\"c4b1d148-57e2-4fd2-9dae-0121d40854a0\",\"emailAddress\":\"testuser@npauctions.com\",\"currentAppVersion\":\"3.5.1B\",\"currentAppVersionRequiredDate\":\"2016-04-25T00:00:00\",\"canCreateCr\":true}",
                        new AmsUser()
                        {
                            UserAccountId = new Guid( "47e0ecd7-b056-4fd4-bc08-78feda408b62" ),
                            UserName = "testuser",
                            UserLevel = 99,
                            Location = "San Diego",
                            LocationId = new Guid( "c4b1d148-57e2-4fd2-9dae-0121d40854a0" ),
                            EmailAddress = "testuser@npauctions.com",
                            CurrentAppVersion = "3.5.1B",
                            CurrentAppVersionRequiredDate = new DateTime( 2016, 4, 25 ),
                            CanCreateInspections = true
                        }
                    },
                    new object[]
                    {
                        "{\"userAccountId\":\"47e0ecd7-b056-4fd4-bc08-78feda408b62\",\"userName\":\"dsiegel\",\"userLevel\":99,\"location\":\"San Diego\",\"locationId\":\"c4b1d148-57e2-4fd2-9dae-0121d40854a0\",\"emailAddress\":\"dsiegel@npauctions.com\",\"currentAppVersion\":\"3.5.1B\",\"currentAppVersionRequiredDate\":\"2016-04-25T00:00:00\",\"canCreateCr\":true}",
                        new AmsUser()
                        {
                            UserAccountId = new Guid( "47e0ecd7-b056-4fd4-bc08-78feda408b62" ),
                            UserName = "dsiegel",
                            UserLevel = 99,
                            Location = "San Diego",
                            LocationId = new Guid( "c4b1d148-57e2-4fd2-9dae-0121d40854a0" ),
                            EmailAddress = "dsiegel@npauctions.com",
                            CurrentAppVersion = "3.5.1B",
                            CurrentAppVersionRequiredDate = new DateTime( 2016, 4, 25 ),
                            CanCreateInspections = true
                        }
                    }
                };
            }
        }
    }
}
