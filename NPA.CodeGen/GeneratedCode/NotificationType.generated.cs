using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace NPA.CodeGen
{
    public partial class NotificationType : IEquatable<NotificationType>, IComparable<NotificationType>
    {
        public Guid NotificationTypeId { get; private set; }
        public string DisplayName { get; private set; }
        public string ShortName { get; private set; }

        private NotificationType() { }

        private NotificationType(Guid notificationTypeId, string notificationTypeName, string displayName)
        {
            this.NotificationTypeId = notificationTypeId;
            this.ShortName = notificationTypeName;
            this.DisplayName = displayName;
        }

        public bool Equals(NotificationType other)
        {
            return this.NotificationTypeId.Equals(other.NotificationTypeId);
        }

        public int CompareTo(NotificationType other)
        {
            return this.ShortName.CompareTo(other.ShortName);
        }

        public override string ToString()
        {
            return DisplayName;
        }
        
        public static NotificationType FromGuid(Guid guid)
        {
            return GetValues().SingleOrDefault(v => v.NotificationTypeId == guid);
        }

        public static NotificationType FromDisplayName(string name)
        {
            return GetValues().SingleOrDefault(v => string.Compare(v.DisplayName, name, true) == 0);
        }

        public static NotificationType FromShortName(string name)
        {
            return GetValues().SingleOrDefault(v => string.Compare(v.ShortName, name, true) == 0);
        }

        public static NotificationType Bulkmail = new NotificationType(
            new Guid("105d5bc1-154e-4a2e-822c-96955060c271"), "Bulkmail", "Bulkmail");

        public static NotificationType Email = new NotificationType(
            new Guid("c99b35b4-4388-4b68-8f16-4cfb34336b4d"), "Email", "Email");

        public static NotificationType SMS = new NotificationType(
            new Guid("7a5558f6-e82e-4d96-9631-754e9405fb63"), "SMS", "SMS");
        
        
        public static IEnumerable<NotificationType> GetValues()
        {
            yield return Bulkmail;
            yield return Email;
            yield return SMS;
        }
    }
}
