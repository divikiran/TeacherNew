using System;
using System.Linq;
using System.Collections.Generic;

namespace NPA.CodeGen
{
    public class UserLevel : IEquatable<UserLevel>, IComparable<UserLevel>
    {
        public Guid UserLevelId { get; private set; }
        public int UserLevelCode { get; private set; }
		public string DisplayName { get; private set; }
        
        private UserLevel() { }

        private UserLevel(Guid userLevelId, int userLevel, string displayName)
        {
            UserLevelId = userLevelId;
            UserLevelCode = userLevel;
			DisplayName = displayName;			
        }

        public bool Equals(UserLevel other)
        {
            return this.UserLevelId.Equals(other.UserLevelId);
        }

        public int CompareTo(UserLevel other)
        {
            return this.UserLevelCode.CompareTo(other.UserLevelCode);
        }

        public override string ToString()
        {
            return DisplayName;
        }

        public static UserLevel FromGuid(Guid userLevelId)
        {
            return GetValues().SingleOrDefault(ul => ul.UserLevelId == userLevelId);
        }

		public static UserLevel FromDisplayName(string displayName)
		{
		   return GetValues().SingleOrDefault(ul => string.Compare(ul.DisplayName, displayName, true) == 0);
		}

		public static UserLevel FromUserLevelCode(int userLevelCode)
		{
			return GetValues().SingleOrDefault(ul => ul.UserLevelCode == userLevelCode);
		}

        public static UserLevel Disabled = new UserLevel(new Guid("c474bf33-c8a6-e111-962a-0019b9b35da2"), 0, "Disabled");

        public static UserLevel PortalUser = new UserLevel(new Guid("8f4461b2-890f-4445-8580-2d948564fa4e"), 1, "PortalUser");

        public static UserLevel PortalMultiLenderUser = new UserLevel(new Guid("b1884b12-a5db-4e0b-a38d-0c03b220b401"), 2, "PortalMultiLenderUser");

        public static UserLevel ItTestTechnician = new UserLevel(new Guid("c45e68ef-903d-dd11-9dca-0019b9b35da2"), 3, "ItTestTechnician");

        public static UserLevel PortalSuperUser = new UserLevel(new Guid("45673b6d-4271-4f29-a1f6-15d8e3357b3c"), 3, "PortalSuperUser");

        public static UserLevel PortalAdmin = new UserLevel(new Guid("14bb49fe-603d-444a-9516-8978ecee8ed3"), 4, "PortalAdmin");

        public static UserLevel User = new UserLevel(new Guid("7d8dda58-b3c1-43dc-b844-49b2683a14cb"), 5, "User");

        public static UserLevel Editor = new UserLevel(new Guid("4a18dcac-012f-43c8-bcf9-87464a7d2bee"), 10, "Editor");

        public static UserLevel SiteAdmin = new UserLevel(new Guid("6d73eedd-cbd8-4c9a-af77-cde830aa9ab7"), 20, "SiteAdmin");

        public static UserLevel DataAssistant = new UserLevel(new Guid("62532f90-9f52-dd11-b13e-0019b9b35da2"), 25, "DataAssistant");

        public static UserLevel SystemAdmin = new UserLevel(new Guid("7601df0f-c23e-4296-8ce0-9fcc41fffa0a"), 30, "SystemAdmin");

        public static UserLevel DataAdmin = new UserLevel(new Guid("22b60c44-734f-dd11-b13e-0019b9b35da2"), 35, "DataAdmin");

        public static UserLevel AccountingAdmin = new UserLevel(new Guid("dc25895a-4087-dc11-89b0-000c55360864"), 40, "AccountingAdmin");

        public static UserLevel GeneralManager = new UserLevel(new Guid("233773a0-8bbb-df11-9d2e-0019b9b35da2"), 45, "GeneralManager");

        public static UserLevel Management = new UserLevel(new Guid("c177e99e-fce6-dc11-ade2-0019b9b35da2"), 50, "Management");

        public static UserLevel Developer = new UserLevel(new Guid("7c9254e5-fc18-4b74-a581-a0d47f3647e7"), 99, "Developer");

        public static IEnumerable<UserLevel> GetValues()
        {
            yield return Disabled;
            yield return PortalUser;
            yield return PortalMultiLenderUser;
            yield return ItTestTechnician;
            yield return PortalSuperUser;
            yield return PortalAdmin;
            yield return User;
            yield return Editor;
            yield return SiteAdmin;
            yield return DataAssistant;
            yield return SystemAdmin;
            yield return DataAdmin;
            yield return AccountingAdmin;
            yield return GeneralManager;
            yield return Management;
            yield return Developer;
    
        }
    }
}
