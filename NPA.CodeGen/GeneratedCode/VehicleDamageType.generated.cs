using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace NPA.CodeGen
{
    public partial class VehicleDamageType : IEquatable<VehicleDamageType>, IComparable<VehicleDamageType>
    {
        public Guid VehicleDamageTypeId { get; private set; }
        public string DisplayName { get; private set; }
        public string ShortName { get; private set; }

        private VehicleDamageType() { }

        private VehicleDamageType(Guid vehicleDamageTypeId, string vehicleDamageTypeName, string displayName)
        {
            this.VehicleDamageTypeId = vehicleDamageTypeId;
            this.ShortName = vehicleDamageTypeName;
            this.DisplayName = displayName;
        }

        public bool Equals(VehicleDamageType other)
        {
            return this.VehicleDamageTypeId.Equals(other.VehicleDamageTypeId);
        }

        public int CompareTo(VehicleDamageType other)
        {
            return this.ShortName.CompareTo(other.ShortName);
        }

        public override string ToString()
        {
            return DisplayName;
        }
        
        public static VehicleDamageType FromGuid(Guid guid)
        {
            return GetValues().SingleOrDefault(v => v.VehicleDamageTypeId == guid);
        }

        public static VehicleDamageType FromDisplayName(string name)
        {
            return GetValues().SingleOrDefault(v => string.Compare(v.DisplayName, name, true) == 0);
        }

        public static VehicleDamageType FromShortName(string name)
        {
            return GetValues().SingleOrDefault(v => string.Compare(v.ShortName, name, true) == 0);
        }

        public static VehicleDamageType AllOver = new VehicleDamageType(
            new Guid("e484ce93-a9e1-4ad7-8c04-728852e30e79"), "AllOver", "All Over");

        public static VehicleDamageType BiohazardousLoss = new VehicleDamageType(
            new Guid("81fa4f50-af15-4293-94a1-ab35504c5225"), "BiohazardousLoss", "Biohazardous Loss");

        public static VehicleDamageType DamagedUnit = new VehicleDamageType(
            new Guid("5d4f6667-bde9-403a-bd0c-9cffdc35bd5c"), "DamagedUnit", "Damaged Unit");

        public static VehicleDamageType FireLoss = new VehicleDamageType(
            new Guid("0be12b30-5d90-47b4-be63-a0827812cb7d"), "FireLoss", "Fire Loss");

        public static VehicleDamageType FRAME = new VehicleDamageType(
            new Guid("0219d6fa-acce-4642-96ea-3a14ae92cdac"), "FRAME", "FRAME");

        public static VehicleDamageType FrontEndCollision = new VehicleDamageType(
            new Guid("131f82e7-4716-4ff1-9fa2-9218733a2a8a"), "FrontEndCollision", "Front End Collision");

        public static VehicleDamageType HAIL = new VehicleDamageType(
            new Guid("a813a1bd-8ecd-4f08-8ddb-8dc70bad5cc0"), "HAIL", "HAIL");

        public static VehicleDamageType LoanPoolUnit = new VehicleDamageType(
            new Guid("5009acbd-f94a-4dcd-aab1-9b1668955cd0"), "LoanPoolUnit", "Loan Pool Unit");

        public static VehicleDamageType MECHANICAL = new VehicleDamageType(
            new Guid("02e95390-af15-4511-8cb9-eef8c5d963de"), "MECHANICAL", "MECHANICAL");

        public static VehicleDamageType MinorDentsScratches = new VehicleDamageType(
            new Guid("08777b33-84ef-4262-bf23-a79db3f825f0"), "MinorDentsScratches", "Minor Dents/Scratches");

        public static VehicleDamageType MissingAlteredVin = new VehicleDamageType(
            new Guid("366bf5b2-410f-42a6-9632-6d3b87c96e94"), "MissingAlteredVin", "Missing/Altered Vin");

        public static VehicleDamageType NormalWearAndTear = new VehicleDamageType(
            new Guid("3cb6758f-7791-4664-9156-c524fe2baa47"), "NormalWearAndTear", "Normal Wear And Tear");

        public static VehicleDamageType RearEndCollision = new VehicleDamageType(
            new Guid("b99798c3-e14c-4d91-9080-9997abd44625"), "RearEndCollision", "Rear End Collision");

        public static VehicleDamageType RepairIncomplete = new VehicleDamageType(
            new Guid("722e7d5b-9b05-44b4-81a4-de28574926bd"), "RepairIncomplete", "Repair Incomplete");

        public static VehicleDamageType RepairRejected = new VehicleDamageType(
            new Guid("77963996-dae2-4c97-8650-9cb9c553701a"), "RepairRejected", "Repair Rejected");

        public static VehicleDamageType RepairedPreviously = new VehicleDamageType(
            new Guid("7fd5167d-3944-428a-b62c-38ffa107ad2d"), "RepairedPreviously", "Repaired Previously");

        public static VehicleDamageType RepoUnit = new VehicleDamageType(
            new Guid("e6d98f7d-40bd-41b4-a0a2-8b7254fd4ef4"), "RepoUnit", "Repo Unit");

        public static VehicleDamageType SideCollision = new VehicleDamageType(
            new Guid("8e5aa79f-389f-4b93-ba32-caaf0a19db8e"), "SideCollision", "Side Collision");

        public static VehicleDamageType STRIPPED = new VehicleDamageType(
            new Guid("b8c689f2-b3bc-46e7-a224-bb7c6a9cf990"), "STRIPPED", "STRIPPED");

        public static VehicleDamageType UNKNOWN = new VehicleDamageType(
            new Guid("62b4ca7f-7b59-4110-9205-0ca8cb25eae1"), "UNKNOWN", "UNKNOWN");

        public static VehicleDamageType VandalismTheft = new VehicleDamageType(
            new Guid("2c97d7d0-5fc8-4643-b2b0-d95b240efd4c"), "VandalismTheft", "Vandalism/Theft");

        public static VehicleDamageType VinReplacement = new VehicleDamageType(
            new Guid("b36c0054-c3f9-48e5-89a5-5a770293c876"), "VinReplacement", "Vin Replacement");

        public static VehicleDamageType WaterFlood = new VehicleDamageType(
            new Guid("87016f0c-8282-48fe-82bd-ee5d5c51e172"), "WaterFlood", "Water/Flood");
        
        
        public static IEnumerable<VehicleDamageType> GetValues()
        {
            yield return AllOver;
            yield return BiohazardousLoss;
            yield return DamagedUnit;
            yield return FireLoss;
            yield return FRAME;
            yield return FrontEndCollision;
            yield return HAIL;
            yield return LoanPoolUnit;
            yield return MECHANICAL;
            yield return MinorDentsScratches;
            yield return MissingAlteredVin;
            yield return NormalWearAndTear;
            yield return RearEndCollision;
            yield return RepairIncomplete;
            yield return RepairRejected;
            yield return RepairedPreviously;
            yield return RepoUnit;
            yield return SideCollision;
            yield return STRIPPED;
            yield return UNKNOWN;
            yield return VandalismTheft;
            yield return VinReplacement;
            yield return WaterFlood;
        }
    }
}
