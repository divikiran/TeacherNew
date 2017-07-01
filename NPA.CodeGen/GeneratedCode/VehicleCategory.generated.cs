using System;
using System.Linq;
using System.Collections.Generic;

namespace NPA.CodeGen
{
    public class VehicleCategory : IEquatable<VehicleCategory>, IComparable<VehicleCategory>
    {
        public Guid VehicleCategoryId { get; private set; }
        public string ShortName { get; private set; }
		public string DisplayName { get; private set; }
        public VINType VinType { get; private set; }

        private VehicleCategory() { }

        private VehicleCategory(Guid vehicleCategoryId, string displayName, string shortName, VINType vinType)
        {
            VehicleCategoryId = vehicleCategoryId;
            DisplayName = displayName;
			ShortName = shortName;
            VinType = vinType;
        }

        public bool Equals(VehicleCategory other)
        {
            return this.VehicleCategoryId.Equals(other.VehicleCategoryId);
        }

        public int CompareTo(VehicleCategory other)
        {
            return this.ShortName.CompareTo(other.ShortName);
        }

        public override string ToString()
        {
            return DisplayName;
        }

        public static VehicleCategory FromGuid(Guid categoryId)
        {
            return GetValues().SingleOrDefault(vc => vc.VehicleCategoryId == categoryId);
        }

		public static VehicleCategory FromShortName(string shortName)
		{
		   return GetValues().SingleOrDefault(v => string.Compare(v.ShortName, shortName, true) == 0);
		}

		public static VehicleCategory FromDisplayName(string displayName)
		{
		   return GetValues().SingleOrDefault(v => string.Compare(v.DisplayName, displayName, true) == 0);
		}

        public static VehicleCategory ApparelAndMore = new VehicleCategory(new Guid("bbbaa5d3-6b66-408a-b9c9-f265c953ca42"), "APPAREL AND MORE", "ApparelAndMore", VINType.FromShortName("Standard"));

        public static VehicleCategory AtvSideBySide = new VehicleCategory(new Guid("c3b590c4-bd7d-dc11-a551-0019b9b35da2"), "ATV--SIDE BY SIDE", "AtvSideBySide", VINType.FromShortName("Standard"));

        public static VehicleCategory AtvSport = new VehicleCategory(new Guid("6b95054d-9ebb-45c5-90a4-1f57c162a388"), "ATV--SPORT", "AtvSport", VINType.FromShortName("Standard"));

        public static VehicleCategory AtvUtility = new VehicleCategory(new Guid("8b04fe6a-9ffb-418a-a5b3-5f4000ed54dc"), "ATV--UTILITY", "AtvUtility", VINType.FromShortName("Standard"));

        public static VehicleCategory AUTO = new VehicleCategory(new Guid("84205a55-343e-4152-9988-90e49277a1b8"), "AUTO", "AUTO", VINType.FromShortName("Standard"));

        public static VehicleCategory BOAT = new VehicleCategory(new Guid("f5180e95-cdc1-4530-998c-20c25f50dad9"), "BOAT", "BOAT", VINType.FromShortName("Marine"));

        public static VehicleCategory BoatTrailer = new VehicleCategory(new Guid("51dedf50-3a11-44a9-88fe-efa67de0fc10"), "BOAT TRAILER", "BoatTrailer", VINType.FromShortName("Standard"));

        public static VehicleCategory Camper5ThWheel = new VehicleCategory(new Guid("59fe3cf1-0ede-4a2f-bab7-56f7b1c5da8b"), "CAMPER / 5TH WHEEL", "Camper5ThWheel", VINType.FromShortName("Standard"));

        public static VehicleCategory CRUISER = new VehicleCategory(new Guid("da744e98-392a-4ef9-aedc-3e1e231ceeaf"), "CRUISER", "CRUISER", VINType.FromShortName("Standard"));

        public static VehicleCategory DualSport = new VehicleCategory(new Guid("d05f33a8-71a2-4551-86d5-3ca0462d7641"), "DUAL SPORT", "DualSport", VINType.FromShortName("Standard"));

        public static VehicleCategory DumpTrailer = new VehicleCategory(new Guid("1f2fa44e-e436-4641-8298-59373ba27458"), "DUMP TRAILER", "DumpTrailer", VINType.FromShortName("Standard"));

        public static VehicleCategory DuneBuggy = new VehicleCategory(new Guid("d29dfc95-f00f-dd11-ad1a-0019b9b35da2"), "DUNE BUGGY", "DuneBuggy", VINType.FromShortName("Standard"));

        public static VehicleCategory EnclosedTrailer = new VehicleCategory(new Guid("531e258a-6e19-4c37-a036-1dabaaad345d"), "ENCLOSED TRAILER", "EnclosedTrailer", VINType.FromShortName("Standard"));

        public static VehicleCategory ENGINE = new VehicleCategory(new Guid("a2daaa75-2f57-4fe7-b0f0-a74924a66ca8"), "ENGINE", "ENGINE", VINType.FromShortName("Standard"));

        public static VehicleCategory ESCOOTER = new VehicleCategory(new Guid("95adc29a-3f05-4c50-a788-d31dc72a0e75"), "E-SCOOTER", "ESCOOTER", VINType.FromShortName("Standard"));

        public static VehicleCategory GENERATOR = new VehicleCategory(new Guid("dc8a2f0b-507b-dc11-a551-0019b9b35da2"), "GENERATOR", "GENERATOR", VINType.FromShortName("Standard"));

        public static VehicleCategory GoCart = new VehicleCategory(new Guid("e19f4c49-230a-4a57-9e85-3d729e4c34b3"), "GO CART", "GoCart", VINType.FromShortName("Standard"));

        public static VehicleCategory GolfCart = new VehicleCategory(new Guid("9a13ec7b-e785-4ee7-bba9-8d2bb83330a6"), "GOLF CART", "GolfCart", VINType.FromShortName("Standard"));

        public static VehicleCategory HorseTrailer = new VehicleCategory(new Guid("ae96e24d-4989-4b40-9ce9-0d3dde893b39"), "HORSE TRAILER", "HorseTrailer", VINType.FromShortName("Standard"));

        public static VehicleCategory LawnMower = new VehicleCategory(new Guid("a537fa16-d787-43aa-9581-db01113f5479"), "LAWN MOWER", "LawnMower", VINType.FromShortName("Standard"));

        public static VehicleCategory Misc = new VehicleCategory(new Guid("985b1039-ca2e-4802-90d5-b8e79d66c5cd"), "MISC.", "Misc", VINType.FromShortName("Standard"));

        public static VehicleCategory MOPED = new VehicleCategory(new Guid("e0c63c8b-e915-42e4-ac3a-72ec9a89e86e"), "MOPED", "MOPED", VINType.FromShortName("Standard"));

        public static VehicleCategory MX = new VehicleCategory(new Guid("3d3c74a2-3b88-43af-9a4f-ff8159ce4e89"), "MX", "MX", VINType.FromShortName("Standard"));

        public static VehicleCategory NadaUnknownImport = new VehicleCategory(new Guid("32b460e9-2365-446f-9b08-7a5f49237930"), "NADA UNKNOWN IMPORT", "NadaUnknownImport", VINType.FromShortName("Standard"));

        public static VehicleCategory PWC = new VehicleCategory(new Guid("f02806eb-88f4-45bd-a6e0-5bfc1a2a3b32"), "PWC", "PWC", VINType.FromShortName("Marine"));

        public static VehicleCategory PwcDoubleTrailer = new VehicleCategory(new Guid("a94e359a-2d91-4375-9063-ee3263154103"), "PWC DOUBLE TRAILER", "PwcDoubleTrailer", VINType.FromShortName("Standard"));

        public static VehicleCategory PwcSingleTrailer = new VehicleCategory(new Guid("ecdf8111-ea20-4214-aee3-134e5d6c8b3e"), "PWC SINGLE TRAILER", "PwcSingleTrailer", VINType.FromShortName("Standard"));

        public static VehicleCategory RV = new VehicleCategory(new Guid("036b9965-5a28-418d-9786-673bf8e30086"), "RV", "RV", VINType.FromShortName("Standard"));

        public static VehicleCategory SCOOTER = new VehicleCategory(new Guid("55dc9b54-ae73-4f50-9eec-cde91ecf157a"), "SCOOTER", "SCOOTER", VINType.FromShortName("Standard"));

        public static VehicleCategory SideCar = new VehicleCategory(new Guid("6dc6dc16-7487-47c7-bafb-c3045bad46d5"), "SIDE CAR", "SideCar", VINType.FromShortName("Standard"));

        public static VehicleCategory SNOWMOBILE = new VehicleCategory(new Guid("06bc4126-f6df-4b47-a495-ea15db608569"), "SNOWMOBILE", "SNOWMOBILE", VINType.FromShortName("Standard"));

        public static VehicleCategory SPECIALTY = new VehicleCategory(new Guid("25a693d9-fd6c-4eea-9163-7ee476cefe4e"), "SPECIALTY", "SPECIALTY", VINType.FromShortName("Standard"));

        public static VehicleCategory SPORT = new VehicleCategory(new Guid("eaf0e6b3-ccae-4bbf-ac8a-d46f4df4321f"), "SPORT", "SPORT", VINType.FromShortName("Standard"));

        public static VehicleCategory TRACTOR = new VehicleCategory(new Guid("6c2c6fe9-1c2b-48e5-8909-9373265f2efe"), "TRACTOR", "TRACTOR", VINType.FromShortName("Standard"));

        public static VehicleCategory TRIKE = new VehicleCategory(new Guid("5fe74cda-4f96-dd11-aca2-0019b9b35da2"), "TRIKE", "TRIKE", VINType.FromShortName("Standard"));

        public static VehicleCategory UNKNOWN = new VehicleCategory(new Guid("9d4b51be-c52c-456d-a552-cdb6c6e477a1"), "UNKNOWN", "UNKNOWN", VINType.FromShortName("Standard"));

        public static VehicleCategory UtilityTrailer = new VehicleCategory(new Guid("81b139f3-e2cc-469c-922c-d03413a7fcbd"), "UTILITY TRAILER", "UtilityTrailer", VINType.FromShortName("Standard"));

        public static VehicleCategory VINTAGE = new VehicleCategory(new Guid("e43631c8-9f2b-44a6-a80e-2c00fc9c45af"), "VINTAGE", "VINTAGE", VINType.FromShortName("Marine"));

        public static IEnumerable<VehicleCategory> GetValues()
        {
            yield return ApparelAndMore;
            yield return AtvSideBySide;
            yield return AtvSport;
            yield return AtvUtility;
            yield return AUTO;
            yield return BOAT;
            yield return BoatTrailer;
            yield return Camper5ThWheel;
            yield return CRUISER;
            yield return DualSport;
            yield return DumpTrailer;
            yield return DuneBuggy;
            yield return EnclosedTrailer;
            yield return ENGINE;
            yield return ESCOOTER;
            yield return GENERATOR;
            yield return GoCart;
            yield return GolfCart;
            yield return HorseTrailer;
            yield return LawnMower;
            yield return Misc;
            yield return MOPED;
            yield return MX;
            yield return NadaUnknownImport;
            yield return PWC;
            yield return PwcDoubleTrailer;
            yield return PwcSingleTrailer;
            yield return RV;
            yield return SCOOTER;
            yield return SideCar;
            yield return SNOWMOBILE;
            yield return SPECIALTY;
            yield return SPORT;
            yield return TRACTOR;
            yield return TRIKE;
            yield return UNKNOWN;
            yield return UtilityTrailer;
            yield return VINTAGE;
    
        }
    }
}
