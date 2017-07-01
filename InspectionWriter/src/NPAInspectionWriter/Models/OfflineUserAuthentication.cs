using System;
using SQLite.Net.Attributes;

namespace NPAInspectionWriter.Models
{
    public class OfflineUserAuthentication
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public Guid UserAccountId { get; set; }

        public int AccountLevel { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public Guid LocationId { get; set; }

        public string Location { get; set; }

        public string Email { get; set; }

        public bool CanCreateCr { get; set; }
    }
}
