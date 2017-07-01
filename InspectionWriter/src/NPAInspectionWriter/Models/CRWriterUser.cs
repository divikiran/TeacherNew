using System;
using NPAInspectionWriter.Models;
using NPAInspectionWriter.iOS.Models;
using SQLite.Net.Attributes;

namespace NPAInspectionWriter.Models
{
    public class CRWriterUser : IDatabaseRecord
    {
        [PrimaryKey]
        public Guid AccountId { get; set; }

        public string UserName { get; set; }

        public int UserLevel { get; set; }

        public Guid LocationId { get; set; }

        public string Location { get; set; }

        public string AuthToken { get; set; }

        public string SessionToken { get; set; }

        public string Email { get; set; }

        public bool CanCreateCr { get; set; }

        public string LastErrorMessage { get; set; }

        public DateTime LastSync { get; set; } = DateTime.Now;

        public static implicit operator CRWriterUser( AmsUser user )
        {
            return new CRWriterUser()
            {
                AccountId = user.UserAccountId,
                AuthToken = (user.AuthToken == null ? Helpers.InspectionWriterClient.BasicAuthCredential : user.AuthToken),
                SessionToken = user.SessionToken,
                CanCreateCr = user.CanCreateInspections,
                Email = user.EmailAddress,
                LastErrorMessage = AppData.AppMessages.MatrixGlitchErrorMessage,
                LastSync = DateTime.Now,
                Location = user.Location,
                LocationId = user.LocationId,
                UserLevel = user.UserLevel,
                UserName = user.UserName
            };
        }

        public static implicit operator Guid( CRWriterUser user ) => user.AccountId;
    }
}
