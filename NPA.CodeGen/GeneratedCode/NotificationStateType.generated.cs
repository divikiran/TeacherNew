using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace NPA.CodeGen
{
    public partial class NotificationStateType : IEquatable<NotificationStateType>, IComparable<NotificationStateType>
    {
        public Guid NotificationStateTypeId { get; private set; }
        public string DisplayName { get; private set; }
        public string ShortName { get; private set; }

        private NotificationStateType() { }

        private NotificationStateType(Guid notificationStateTypeId, string notificationStateTypeName, string displayName)
        {
            this.NotificationStateTypeId = notificationStateTypeId;
            this.ShortName = notificationStateTypeName;
            this.DisplayName = displayName;
        }

        public bool Equals(NotificationStateType other)
        {
            return this.NotificationStateTypeId.Equals(other.NotificationStateTypeId);
        }

        public int CompareTo(NotificationStateType other)
        {
            return this.ShortName.CompareTo(other.ShortName);
        }

        public override string ToString()
        {
            return DisplayName;
        }
        
        public static NotificationStateType FromGuid(Guid guid)
        {
            return GetValues().SingleOrDefault(v => v.NotificationStateTypeId == guid);
        }

        public static NotificationStateType FromDisplayName(string name)
        {
            return GetValues().SingleOrDefault(v => string.Compare(v.DisplayName, name, true) == 0);
        }

        public static NotificationStateType FromShortName(string name)
        {
            return GetValues().SingleOrDefault(v => string.Compare(v.ShortName, name, true) == 0);
        }

        public static NotificationStateType AlertRaised = new NotificationStateType(
            new Guid("0ac85fb6-f8d5-41ee-8172-d085313b5a2b"), "AlertRaised", "Alert Raised");

        public static NotificationStateType Completed = new NotificationStateType(
            new Guid("b8fd1972-40cd-494f-a896-43f8f31e5163"), "Completed", "Completed");

        public static NotificationStateType Error = new NotificationStateType(
            new Guid("18e8d843-f2e8-4ff9-bc59-3f0fd46114f8"), "Error", "Error");

        public static NotificationStateType InProcess = new NotificationStateType(
            new Guid("7655f3a6-b96c-4fcb-b22d-57dec120fa23"), "InProcess", "InProcess");

        public static NotificationStateType Resolved = new NotificationStateType(
            new Guid("7caeea1b-6705-49ec-8732-28e31a8b23a7"), "Resolved", "Resolved");

        public static NotificationStateType Waiting = new NotificationStateType(
            new Guid("b5571492-8ec2-4139-8c22-bc04e25cd2bc"), "Waiting", "Waiting");
        
        
        public static IEnumerable<NotificationStateType> GetValues()
        {
            yield return AlertRaised;
            yield return Completed;
            yield return Error;
            yield return InProcess;
            yield return Resolved;
            yield return Waiting;
        }
    }
}
