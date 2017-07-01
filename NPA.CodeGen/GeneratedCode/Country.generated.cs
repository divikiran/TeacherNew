using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPA.CodeGen
{
    public partial class Country : IEquatable<Country>, IComparable<Country>
    {
        public Guid CountryID { get; private set; }
        public string Abbreviation { get; private set; }
        public string CountryName { get; private set; }

        public Country(Guid countryID, string abbreviation, string countryName)
        {
            this.CountryID = countryID;
            this.Abbreviation = abbreviation;
            this.CountryName = countryName;
        }

       public bool Equals(Country other)
        {
            return this.CountryID.Equals(other.CountryID);
        }

        public int CompareTo(Country other)
        {
            return this.Abbreviation.CompareTo(other.Abbreviation);
        }

        public override string ToString()
        {
            return CountryName;
        }

        public static Country FromGuid(Guid guid)
        {
            return GetValues().SingleOrDefault(s => s.CountryID == guid);
        }

        public static Country FromAbbreviation(string abbrev)
        {
            return GetValues().SingleOrDefault(s => string.Compare(s.Abbreviation, abbrev, true) == 0);
        }

        public static Country FromCountryName(string CountryName)
        {
            return GetValues().SingleOrDefault(s => string.Compare(s.CountryName, CountryName, true) == 0);
        }


        public static Country Afghanistan = 
                new Country(new Guid("d4d4e3ba-07ee-4e74-bffc-6c4ebc6fa377"), "AF", "Afghanistan");

        public static Country AlandIslands = 
                new Country(new Guid("99d37d4e-3a6b-4c14-b034-0ec7ea71cb2d"), "AX", "Aland Islands");

        public static Country Albania = 
                new Country(new Guid("c2b2cd06-ca6a-419e-91e0-87cc31abc7c2"), "AL", "Albania");

        public static Country Algeria = 
                new Country(new Guid("6af9c0cf-62b1-48a5-8898-6830ec942048"), "DZ", "Algeria");

        public static Country AmericanSamoa = 
                new Country(new Guid("7533dae6-2364-49ba-8bd4-bc058363541b"), "AS", "American Samoa");

        public static Country Andorra = 
                new Country(new Guid("3e85a9a0-a936-4e2b-96c3-154e0cab9633"), "AD", "Andorra");

        public static Country Angola = 
                new Country(new Guid("5918fcd5-c101-4954-a5b0-43dabaec5807"), "AO", "Angola");

        public static Country Anguilla = 
                new Country(new Guid("9ef51755-4df5-4bf4-aa18-3f92c1120975"), "AI", "Anguilla");

        public static Country Antarctica = 
                new Country(new Guid("c6dfba32-e850-463f-9362-799ec0c41a1a"), "AQ", "Antarctica");

        public static Country AntiguaAndBarbuda = 
                new Country(new Guid("2b9e382b-93ac-41c4-86f8-acf424d405c5"), "AG", "Antigua And Barbuda");

        public static Country Argentina = 
                new Country(new Guid("153915b5-1325-49c9-8877-c9c5cecbbf17"), "AR", "Argentina");

        public static Country Armenia = 
                new Country(new Guid("80d128d4-849c-4250-9df5-f53c10f4ff55"), "AM", "Armenia");

        public static Country Aruba = 
                new Country(new Guid("c7cde6c0-a050-4f6b-a14c-dd99edd097f6"), "AW", "Aruba");

        public static Country Australia = 
                new Country(new Guid("add6f289-6f12-41d7-a630-641943000ce3"), "AU", "Australia");

        public static Country Austria = 
                new Country(new Guid("32c54944-5864-48f3-b1d7-196756d905c8"), "AT", "Austria");

        public static Country Azerbaijan = 
                new Country(new Guid("f97da438-21a7-4d9a-8d73-9cd75ea59a8c"), "AZ", "Azerbaijan");

        public static Country Bahamas = 
                new Country(new Guid("82f7db93-ef90-4ac6-bb1f-4fd1b10d03b3"), "BS", "Bahamas");

        public static Country Bahrain = 
                new Country(new Guid("0f5d705f-6a76-4648-b852-282651fcb14d"), "BH", "Bahrain");

        public static Country Bangladesh = 
                new Country(new Guid("55736bd0-5fe4-4121-8fc0-b225532c3875"), "BD", "Bangladesh");

        public static Country Barbados = 
                new Country(new Guid("219ba609-b088-491b-8941-0aec6a80faea"), "BB", "Barbados");

        public static Country Belarus = 
                new Country(new Guid("f5b8e049-2da0-4104-abf4-ebae86c058c5"), "BY", "Belarus");

        public static Country Belgium = 
                new Country(new Guid("ab3789e3-b845-41a8-8369-b10f55de3bae"), "BE", "Belgium");

        public static Country Belize = 
                new Country(new Guid("250b10d4-5087-4a5f-8eb2-91865b1b898f"), "BZ", "Belize");

        public static Country Benin = 
                new Country(new Guid("7a6ca18f-12d3-42b4-9e77-5451e81cf535"), "BJ", "Benin");

        public static Country Bermuda = 
                new Country(new Guid("2930d57b-167f-4999-b1a3-cf47891fde57"), "BM", "Bermuda");

        public static Country Bhutan = 
                new Country(new Guid("d370664f-9f3d-44a2-b860-6d51b2be16b4"), "BT", "Bhutan");

        public static Country Bolivia = 
                new Country(new Guid("50c76bb0-0638-440d-a5cb-9b8b0a079684"), "BO", "Bolivia");

        public static Country BosniaAndHerzegovina = 
                new Country(new Guid("1d7a2ad9-8334-4037-9c69-081e6f123dce"), "BA", "Bosnia And Herzegovina");

        public static Country Botswana = 
                new Country(new Guid("d9fa67bf-6af8-49b1-829c-4c94a2e0496e"), "BW", "Botswana");

        public static Country BouvetIsland = 
                new Country(new Guid("13604dbc-8b04-4dbd-9f25-43b50cd453e2"), "BV", "Bouvet Island");

        public static Country Brazil = 
                new Country(new Guid("ee5c635f-ea30-4162-b663-ab0b46c8a82e"), "BR", "Brazil");

        public static Country BritishIndianOceanTerritory = 
                new Country(new Guid("109fd7b4-87f6-4708-9941-bc32dfc09654"), "IO", "British Indian Ocean Territory");

        public static Country BruneiDarussalam = 
                new Country(new Guid("56cbe288-bc16-4d18-80cb-dd7f141c3808"), "BN", "Brunei Darussalam");

        public static Country Bulgaria = 
                new Country(new Guid("f27ded19-b1c9-4dab-958b-98b53b987799"), "BG", "Bulgaria");

        public static Country BurkinaFaso = 
                new Country(new Guid("22e18052-cf96-4719-a6bb-c49209c214f2"), "BF", "Burkina Faso");

        public static Country Burundi = 
                new Country(new Guid("70306639-da00-4155-b555-3d0068a12477"), "BI", "Burundi");

        public static Country Cambodia = 
                new Country(new Guid("b01ac96d-148a-4b66-b6e2-be3aa564c21b"), "KH", "Cambodia");

        public static Country Cameroon = 
                new Country(new Guid("a1bf1113-3a5f-4e00-9ca3-c0d6d3f6229b"), "CM", "Cameroon");

        public static Country Canada = 
                new Country(new Guid("79d486b2-ab77-4901-bf9f-6cb9354d6988"), "CA", "Canada");

        public static Country CapeVerde = 
                new Country(new Guid("fdd58167-fdba-441c-8f9d-c7bfa28c38b0"), "CV", "Cape Verde");

        public static Country CaymanIslands = 
                new Country(new Guid("d7497c97-6d2a-4cff-98a6-222d4a6cdddf"), "KY", "Cayman Islands");

        public static Country CentralAfricanRepublic = 
                new Country(new Guid("13959f51-6cf5-4b51-b2b8-851f368c92bc"), "CF", "Central African Republic");

        public static Country Chad = 
                new Country(new Guid("c53307f4-bec1-4644-b061-8fc44ccd906d"), "TD", "Chad");

        public static Country Chile = 
                new Country(new Guid("c200371d-abb2-4ec0-ae3c-fabf948dfd34"), "CL", "Chile");

        public static Country China = 
                new Country(new Guid("e7f4cb2a-3870-412f-a005-4113ddc39fc7"), "CN", "China");

        public static Country ChristmasIsland = 
                new Country(new Guid("15e78b81-b791-4c83-8c9a-c8009c1d6eaf"), "CX", "Christmas Island");

        public static Country CocosKeelingIslands = 
                new Country(new Guid("af87d049-f6db-422a-9262-e38f064f8dd5"), "CC", "Cocos (Keeling) Islands");

        public static Country Colombia = 
                new Country(new Guid("480bc5f5-2e29-4657-bab6-cb9bc9a984c0"), "CO", "Colombia");

        public static Country Comoros = 
                new Country(new Guid("82953d41-0ea5-41be-b3d8-3b1ccdf0c4d7"), "KM", "Comoros");

        public static Country Congo = 
                new Country(new Guid("cdcb4460-9ae8-4a5b-b772-a18c1bf19765"), "CG", "Congo");

        public static Country CookIslands = 
                new Country(new Guid("58d10e2a-2fe6-447b-a3b0-51249d7881eb"), "CK", "Cook Islands");

        public static Country CostaRica = 
                new Country(new Guid("97e3fdcc-fd7c-44a2-ab50-383c1f571789"), "CR", "Costa Rica");

        public static Country Croatia = 
                new Country(new Guid("a64a0fb4-1ff6-43c4-b3db-ea67b039bd7c"), "HR", "Croatia");

        public static Country Cuba = 
                new Country(new Guid("feae6fbd-b29b-452e-b9bb-100bd0679bee"), "CU", "Cuba");

        public static Country Cyprus = 
                new Country(new Guid("103dcff4-2b49-4f54-9648-7dd39970db00"), "CY", "Cyprus");

        public static Country CzechRepublic = 
                new Country(new Guid("9a6484d4-bfdc-4958-bfc3-bbcf0df3fdd7"), "CZ", "Czech Republic");

        public static Country DemocraticRepublicOfTheCongo = 
                new Country(new Guid("962e1b82-b27c-4810-87c4-5e4835bc6b5d"), "CD", "Democratic Republic Of The Congo");

        public static Country Denmark = 
                new Country(new Guid("c88df556-8cdb-4e30-91e1-b0d9449a91a6"), "DK", "Denmark");

        public static Country Djibouti = 
                new Country(new Guid("0622b7b8-b707-435d-b64b-f72d365a73ec"), "DJ", "Djibouti");

        public static Country Dominica = 
                new Country(new Guid("c6664d4a-49ba-4a83-8f68-9491ddc10375"), "DM", "Dominica");

        public static Country DominicanRepublic = 
                new Country(new Guid("d2bf1355-2bdd-4e02-83b7-ac1268f528df"), "DO", "Dominican Republic");

        public static Country Ecuador = 
                new Country(new Guid("dedfa38a-91c1-4002-9f21-49f5d7eb5819"), "EC", "Ecuador");

        public static Country Egypt = 
                new Country(new Guid("d187fa2c-072d-4fdf-a96c-8bda73db5831"), "EG", "Egypt");

        public static Country ElSalvador = 
                new Country(new Guid("c1e72f87-5a86-4777-93a4-b9a6935fd153"), "SV", "El Salvador");

        public static Country EquatorialGuinea = 
                new Country(new Guid("bc26cec4-1a40-4efe-8585-8f50854bcfb8"), "GQ", "Equatorial Guinea");

        public static Country Eritrea = 
                new Country(new Guid("9674380e-6ade-4b69-90f1-0f9edf6ddf33"), "ER", "Eritrea");

        public static Country Estonia = 
                new Country(new Guid("71f35426-b9ab-4e7d-980d-b7f11c9c6f51"), "EE", "Estonia");

        public static Country Ethiopia = 
                new Country(new Guid("282c4a76-9159-45c0-88eb-a0266bdf1b6f"), "ET", "Ethiopia");

        public static Country FalklandIslandsMalvinas = 
                new Country(new Guid("dd6f08aa-ebec-44cf-8a28-5ae4ce730c50"), "FK", "Falkland Islands (Malvinas)");

        public static Country FaroeIslands = 
                new Country(new Guid("efd347ff-1e61-4ae8-841c-d439159c4843"), "FO", "Faroe Islands");

        public static Country Fiji = 
                new Country(new Guid("2704d269-5083-4b9e-a9df-6b7f7f2a3613"), "FJ", "Fiji");

        public static Country Finland = 
                new Country(new Guid("4efb9fa3-0dc5-47f8-82ff-3e29552ebd06"), "FI", "Finland");

        public static Country France = 
                new Country(new Guid("859866d8-a622-4199-86a9-9adec75a096a"), "FR", "France");

        public static Country FrenchGuiana = 
                new Country(new Guid("d47082b7-dd67-43cd-b4b5-69f7fdedec48"), "GF", "French Guiana");

        public static Country FrenchPolynesia = 
                new Country(new Guid("4ab4c375-a5ad-4de7-941b-30b8a802a534"), "PF", "French Polynesia");

        public static Country FrenchSouthernTerritories = 
                new Country(new Guid("8030de84-8a7f-4e6a-8bbe-ad539550d09d"), "TF", "French Southern Territories");

        public static Country Gabon = 
                new Country(new Guid("85bfe9c2-586d-45db-af3e-7c41924435a1"), "GA", "Gabon");

        public static Country Gambia = 
                new Country(new Guid("ebe1eddb-bbc9-46ab-97eb-e3b8bf31562c"), "GM", "Gambia");

        public static Country Georgia = 
                new Country(new Guid("49075427-906e-4d21-9ada-57d3ea207c80"), "GE", "Georgia");

        public static Country Germany = 
                new Country(new Guid("0265315b-4605-4eb4-952e-6b2d42566a90"), "DE", "Germany");

        public static Country Ghana = 
                new Country(new Guid("e1644d0e-88f7-46a8-b15e-380c0c317311"), "GH", "Ghana");

        public static Country Gibraltar = 
                new Country(new Guid("ddd54f3a-a50b-4eec-ba2a-34cbe5c2c6e8"), "GI", "Gibraltar");

        public static Country Greece = 
                new Country(new Guid("af5e50b3-fa8a-4528-91a2-a3d1b2cee1b4"), "GR", "Greece");

        public static Country Greenland = 
                new Country(new Guid("aabf9fa7-a020-451d-aa03-fcb46468f0a9"), "GL", "Greenland");

        public static Country Grenada = 
                new Country(new Guid("91a75b6e-d00e-4a5f-9424-cde1d4f3d22e"), "GD", "Grenada");

        public static Country Guadeloupe = 
                new Country(new Guid("3a89593e-800e-438a-99a2-3e2ad391ce3b"), "GP", "Guadeloupe");

        public static Country Guam = 
                new Country(new Guid("5ac9c36b-2c4f-40c1-8bc1-c2783416a5b2"), "GU", "Guam");

        public static Country Guatemala = 
                new Country(new Guid("03050db2-552b-4f7b-8dff-727505fa3ebf"), "GT", "Guatemala");

        public static Country Guernsey = 
                new Country(new Guid("7963da99-8e31-40c0-af86-4daddccd8be0"), "GG", "Guernsey");

        public static Country Guinea = 
                new Country(new Guid("7a7082a5-38dd-49c5-894f-5f4d9254e17e"), "GN", "Guinea");

        public static Country GuineaBissau = 
                new Country(new Guid("879a4593-9e76-4605-b7c4-0872c3147355"), "GW", "Guinea-Bissau");

        public static Country Guyana = 
                new Country(new Guid("1aae259e-f10d-43de-8355-d273d78ffd53"), "GY", "Guyana");

        public static Country Haiti = 
                new Country(new Guid("424d59cf-0a45-4bfb-a918-6c2d6b395987"), "HT", "Haiti");

        public static Country HeardIsland = 
                new Country(new Guid("b2cb45f8-67aa-4ee7-865d-a77e834077e0"), "HM", "Heard Island");

        public static Country Honduras = 
                new Country(new Guid("62a291df-3200-4a80-b4d5-8cd6de7f14b3"), "HN", "Honduras");

        public static Country HongKong = 
                new Country(new Guid("b270b090-d0a2-4bc6-99c1-0b56002f57ad"), "HK", "Hong Kong");

        public static Country Hungary = 
                new Country(new Guid("03f5ec34-054c-42fb-8552-f7160d55ea44"), "HU", "Hungary");

        public static Country Iceland = 
                new Country(new Guid("2b461721-70fa-4c97-82ad-901f740b9950"), "IS", "Iceland");

        public static Country India = 
                new Country(new Guid("3065b8cd-1ae9-46e6-9069-adcd90dfb4f8"), "IN", "India");

        public static Country Indonesia = 
                new Country(new Guid("d5dc9e73-464d-462b-b7a5-d69c160cd8e8"), "ID", "Indonesia");

        public static Country IranIslamicRepublicOf = 
                new Country(new Guid("90925bc6-787a-47f8-9f8e-d974a60c83f1"), "IR", "Iran - Islamic Republic Of");

        public static Country Iraq = 
                new Country(new Guid("a7d272ac-c2f1-4961-883e-7d1dff2d9ea8"), "IQ", "Iraq");

        public static Country Ireland = 
                new Country(new Guid("3d7b75d4-f2ee-417e-8796-759d7104e3f9"), "IE", "Ireland");

        public static Country IsleOfMan = 
                new Country(new Guid("74aeaf7e-35fc-4022-8f6a-28a5e6428b79"), "IM", "Isle Of Man");

        public static Country Israel = 
                new Country(new Guid("cf39cefc-67f9-4c46-9ae3-b88981cadd9e"), "IL", "Israel");

        public static Country Italy = 
                new Country(new Guid("6c245ecd-14ad-42b2-a75d-d5cccdda8c8a"), "IT", "Italy");

        public static Country Jamaica = 
                new Country(new Guid("005366e0-ebdf-4b9a-be28-71f3c6b35ef6"), "JM", "Jamaica");

        public static Country Japan = 
                new Country(new Guid("4f7fd135-8bb0-48d3-997b-d7d2c0f2c2f6"), "JP", "Japan");

        public static Country Jersey = 
                new Country(new Guid("836e680a-50bc-46e7-8aac-2d98cfb2be5e"), "JE", "Jersey");

        public static Country Jordan = 
                new Country(new Guid("dc6116fe-c107-472d-8234-46af583c28d6"), "JO", "Jordan");

        public static Country Kazakhstan = 
                new Country(new Guid("271bcb87-62dc-4dc4-964b-a35471cc26f8"), "KZ", "Kazakhstan");

        public static Country Kenya = 
                new Country(new Guid("ffe9582b-64b1-4730-9ed2-d9092389e5c5"), "KE", "Kenya");

        public static Country Kiribati = 
                new Country(new Guid("63fee3c5-7435-4eda-aa9b-82223780524f"), "KI", "Kiribati");

        public static Country KoreaDemocraticPeoplesRepublicOf = 
                new Country(new Guid("f6fdc51c-96ba-41e6-99e9-eeb3a17168db"), "KP", "Korea - Democratic Peoples Republic Of");

        public static Country KoreaRepublicOf = 
                new Country(new Guid("2a811ad0-49bb-4f6a-8f7f-1cbd107f1db8"), "KR", "Korea - Republic Of");

        public static Country Kuwait = 
                new Country(new Guid("d3f9388d-7157-4184-ac00-890e79751913"), "KW", "Kuwait");

        public static Country Kyrgyzstan = 
                new Country(new Guid("abf48c30-e40c-437a-ba23-8196d52f5915"), "KG", "Kyrgyzstan");

        public static Country LaoPeoplesDemocraticRepublic = 
                new Country(new Guid("ffead3af-d9fc-46ed-ada2-d02d7efe51fb"), "LA", "Lao Peoples Democratic Republic");

        public static Country Latvia = 
                new Country(new Guid("5940345b-d36a-43f0-8f96-8357183dff7e"), "LV", "Latvia");

        public static Country Lebanon = 
                new Country(new Guid("3ccc17ef-848c-499f-9db0-ed30eaeabbb8"), "LB", "Lebanon");

        public static Country Lesotho = 
                new Country(new Guid("be8e0cce-0305-471f-8db1-3fe4f3a58d7c"), "LS", "Lesotho");

        public static Country Liberia = 
                new Country(new Guid("1d4e0942-7e1d-482b-b8d7-01e2269727cb"), "LR", "Liberia");

        public static Country LibyanArabJamahiriya = 
                new Country(new Guid("a1cad268-59fe-4d13-8cba-ef70fa161052"), "LY", "Libyan Arab Jamahiriya");

        public static Country Liechtenstein = 
                new Country(new Guid("ca815f8a-15d6-4316-b7cc-3dbcbf5f5969"), "LI", "Liechtenstein");

        public static Country Lithuania = 
                new Country(new Guid("06e53920-6aec-48a4-bdfd-b66f7ccf1b92"), "LT", "Lithuania");

        public static Country Luxembourg = 
                new Country(new Guid("0bc2a8a0-31f6-40a1-8651-5babfbf87e31"), "LU", "Luxembourg");

        public static Country Macao = 
                new Country(new Guid("394c5737-f5a6-4484-9470-5d87a683ec0c"), "MO", "Macao");

        public static Country MacedoniaTheRepublicOf = 
                new Country(new Guid("354befa2-2bac-45e6-9dd0-ce699fb71c72"), "MK", "Macedonia - The Republic Of");

        public static Country Madagascar = 
                new Country(new Guid("b9d2c2b2-03c3-4454-9e2b-6cfe10dd91d8"), "MG", "Madagascar");

        public static Country Malawi = 
                new Country(new Guid("963c65ed-2707-4fa6-87d4-8562ad5f8819"), "MW", "Malawi");

        public static Country Malaysia = 
                new Country(new Guid("430cc23d-fbf1-4c38-aa8c-a1c7f4e151ce"), "MY", "Malaysia");

        public static Country Maldives = 
                new Country(new Guid("0743a2cf-41f0-4b14-8644-2ce21ab841ab"), "MV", "Maldives");

        public static Country Mali = 
                new Country(new Guid("88070151-77ad-42a5-9fbf-313d415b2dc8"), "ML", "Mali");

        public static Country Malta = 
                new Country(new Guid("34c104c1-ca65-44a4-b822-15b0d9d949cb"), "MT", "Malta");

        public static Country MarshallIslands = 
                new Country(new Guid("6333ea61-4b90-40d3-b309-01a6e9b6d7e1"), "MH", "Marshall Islands");

        public static Country Martinique = 
                new Country(new Guid("057a1669-df84-4bf3-b8aa-194202806dbb"), "MQ", "Martinique");

        public static Country Mauritania = 
                new Country(new Guid("94043cac-8994-4679-8a39-f1caba82fe63"), "MR", "Mauritania");

        public static Country Mauritius = 
                new Country(new Guid("1eb675c1-382c-427d-b8ff-33e1173ad38d"), "MU", "Mauritius");

        public static Country Mayotte = 
                new Country(new Guid("d2d38ec3-8bbe-4baa-bb1c-c8b960832d5d"), "YT", "Mayotte");

        public static Country Mexico = 
                new Country(new Guid("72d42281-7a3c-46cd-9e3c-f9603c86478a"), "MX", "Mexico");

        public static Country MicronesiaFederatedStatesOf = 
                new Country(new Guid("f0c63047-1e21-4fd7-9dbf-d37c963c0a42"), "FM", "Micronesia - Federated States Of");

        public static Country MoldovaRepublicOf = 
                new Country(new Guid("7c999b88-e491-4c77-8e9f-89a38b83b08e"), "MD", "Moldova - Republic Of");

        public static Country Monaco = 
                new Country(new Guid("e6838f63-1b72-4c33-963a-ff694eb11e9a"), "MC", "Monaco");

        public static Country Mongolia = 
                new Country(new Guid("7f0a4b04-3628-4c45-845b-48ff174b1072"), "MN", "Mongolia");

        public static Country Montenegro = 
                new Country(new Guid("2af22501-467e-4378-91a7-9ac5c062c23f"), "ME", "Montenegro");

        public static Country Montserrat = 
                new Country(new Guid("91896d22-a3f2-4e09-8d69-6ef852c93d3e"), "MS", "Montserrat");

        public static Country Morocco = 
                new Country(new Guid("fc45dc40-98aa-41fd-97a6-f0240f2358bb"), "MA", "Morocco");

        public static Country Mozambique = 
                new Country(new Guid("84bdd7e7-5520-49a4-96bd-cf668762012b"), "MZ", "Mozambique");

        public static Country Myanmar = 
                new Country(new Guid("d65c85e8-8e91-475f-ac54-67628ff46e4a"), "MM", "Myanmar");

        public static Country Namibia = 
                new Country(new Guid("2f53fd41-d35d-4f00-9510-17aa02597089"), "NA", "Namibia");

        public static Country Nauru = 
                new Country(new Guid("3a67c859-ab26-4248-9289-0d173bd0dc42"), "NR", "Nauru");

        public static Country Nepal = 
                new Country(new Guid("9dc87e20-0819-4bec-93e7-c7bc3edf8aaa"), "NP", "Nepal");

        public static Country Netherlands = 
                new Country(new Guid("f7eb6780-5c0b-4603-b8f2-a41cda9fbfbc"), "NL", "Netherlands");

        public static Country NetherlandsAntilles = 
                new Country(new Guid("a08bab69-fe73-404e-86ac-edea488d12ff"), "AN", "Netherlands Antilles");

        public static Country NewCaledonia = 
                new Country(new Guid("7f0934b1-a32a-4827-b629-90c98252fcd9"), "NC", "New Caledonia");

        public static Country NewZealand = 
                new Country(new Guid("cbdfb610-9421-4db1-bc2e-feca50343846"), "NZ", "New Zealand");

        public static Country Nicaragua = 
                new Country(new Guid("35a34a62-f7ae-49d5-9ad3-aa881a69ba0c"), "NI", "Nicaragua");

        public static Country Niger = 
                new Country(new Guid("f1bcfc0d-9479-4f2e-b772-4f361bb00701"), "NE", "Niger");

        public static Country Nigeria = 
                new Country(new Guid("150a6d73-40ca-46cf-a348-5d267299415f"), "NG", "Nigeria");

        public static Country Niue = 
                new Country(new Guid("7022359c-0103-4f5d-a06b-7958779cd5a0"), "NU", "Niue");

        public static Country NorfolkIsland = 
                new Country(new Guid("f5d849e5-87ca-4782-9eab-64365c80ecfe"), "NF", "Norfolk Island");

        public static Country NorthernMarianaIslands = 
                new Country(new Guid("e81ddad0-67df-46ee-b22b-eca3a4549347"), "MP", "Northern Mariana Islands");

        public static Country Norway = 
                new Country(new Guid("e32fc625-95ab-401c-9537-75de3edd30a6"), "NO", "Norway");

        public static Country Oman = 
                new Country(new Guid("911ac573-436e-4edd-9576-40720f2abdea"), "OM", "Oman");

        public static Country Pakistan = 
                new Country(new Guid("fdf40525-b4ff-4c5a-b5d5-c1ca9a709e96"), "PK", "Pakistan");

        public static Country Palau = 
                new Country(new Guid("0e8593d9-fe57-4d52-856b-f7965bc751ed"), "PW", "Palau");

        public static Country PalestinianTerritoryOccupied = 
                new Country(new Guid("5299abb3-a992-4965-9e8d-e4c357525591"), "PS", "Palestinian Territory - Occupied");

        public static Country Panama = 
                new Country(new Guid("4d92be6c-71ab-453a-a2b9-9aefd23bc0d9"), "PA", "Panama");

        public static Country PapuaNewGuinea = 
                new Country(new Guid("34fa1c9a-9aef-4d8a-9d0f-26e0c3576869"), "PG", "Papua New Guinea");

        public static Country Paraguay = 
                new Country(new Guid("230e3eff-2cd2-4ae3-b4b9-a24b0770b7ec"), "PY", "Paraguay");

        public static Country Peru = 
                new Country(new Guid("726cd8de-a0cc-429d-a36f-b12266a39873"), "PE", "Peru");

        public static Country Philippines = 
                new Country(new Guid("9e869175-c4c7-45fb-9a2c-889ab1eb815a"), "PH", "Philippines");

        public static Country Pitcairn = 
                new Country(new Guid("3fca2d9c-6335-4194-891a-f2f14ce633c6"), "PN", "Pitcairn");

        public static Country Poland = 
                new Country(new Guid("f0999040-4280-4c6a-86d4-e12bd03ff7aa"), "PL", "Poland");

        public static Country Portugal = 
                new Country(new Guid("a12920b3-7569-48e1-ae20-5ac5ebb95733"), "PT", "Portugal");

        public static Country PuertoRico = 
                new Country(new Guid("76a1c759-9aae-47ae-b354-3a5439287272"), "PR", "Puerto Rico");

        public static Country Qatar = 
                new Country(new Guid("ebfccfa3-fad8-492d-9f67-35b1d4841c6f"), "QA", "Qatar");

        public static Country Reunion = 
                new Country(new Guid("3382a9bf-0339-4ba1-8e04-9541d41d6862"), "RE", "Reunion");

        public static Country Romania = 
                new Country(new Guid("63fc9c6a-a022-4122-8437-c26b69834cde"), "RO", "Romania");

        public static Country RussianFederation = 
                new Country(new Guid("c1dfe37e-ac59-488b-a186-50179e3fa509"), "RU", "Russian Federation");

        public static Country Rwanda = 
                new Country(new Guid("0a4826e3-9ab3-4859-a3a7-fa1c0917909a"), "RW", "Rwanda");

        public static Country SaintBarthelemy = 
                new Country(new Guid("1babda08-30b1-40fc-a4f5-7af868c7371a"), "BL", "Saint Barthelemy");

        public static Country SaintKittsAndNevis = 
                new Country(new Guid("43d97492-1a53-4047-855e-105658b0300a"), "KN", "Saint Kitts And Nevis");

        public static Country SaintLucia = 
                new Country(new Guid("5db4b933-0030-4830-9c83-0c19a90f1dcb"), "LC", "Saint Lucia");

        public static Country SaintMartin = 
                new Country(new Guid("c314ff25-01e9-46d4-a1c4-d1bc43152886"), "MF", "Saint Martin");

        public static Country SaintPierreAndMiquelon = 
                new Country(new Guid("f4b585c4-a31f-483b-8bc4-2aac97490a2e"), "PM", "Saint Pierre And Miquelon");

        public static Country SaintVincentAndTheGrenadines = 
                new Country(new Guid("acf4043e-98d4-4f8c-9012-3cba692a9de6"), "VC", "Saint Vincent And The Grenadines");

        public static Country Samoa = 
                new Country(new Guid("e3be22aa-f290-4a32-b32d-5574a749d8b6"), "WS", "Samoa");

        public static Country SanMarino = 
                new Country(new Guid("a93cc9ad-bf18-4bea-9913-6c4e663a71fd"), "SM", "San Marino");

        public static Country SaoTomeAndPrincipe = 
                new Country(new Guid("8d870ccf-6a53-4a79-b7da-8a0481d80355"), "ST", "Sao Tome And Principe");

        public static Country SaudiArabia = 
                new Country(new Guid("b7e2807c-453a-4688-9b23-b471af9796b4"), "SA", "Saudi Arabia");

        public static Country Senegal = 
                new Country(new Guid("e365bc3d-fe9a-40c5-b2ec-0460373e1c02"), "SN", "Senegal");

        public static Country Serbia = 
                new Country(new Guid("54f0c8b8-565a-4e0e-93c3-b49cb1d76232"), "RS", "Serbia");

        public static Country Seychelles = 
                new Country(new Guid("722dbc38-386d-478b-8161-cba9d0a83f43"), "SC", "Seychelles");

        public static Country SierraLeone = 
                new Country(new Guid("7762973b-263c-4453-bda4-ec7e8b11c018"), "SL", "Sierra Leone");

        public static Country Singapore = 
                new Country(new Guid("173a7cdd-77de-47be-9ef2-aa039392d9b9"), "SG", "Singapore");

        public static Country Slovakia = 
                new Country(new Guid("231fe05e-27b1-400b-a0e0-3e7d8708da4b"), "SK", "Slovakia");

        public static Country Slovenia = 
                new Country(new Guid("b5d2f5ca-49eb-439f-a570-88f4fa8f04a9"), "SI", "Slovenia");

        public static Country SolomonIslands = 
                new Country(new Guid("b262aee3-8771-4fd2-94d8-9d54ee2c5c01"), "SB", "Solomon Islands");

        public static Country Somalia = 
                new Country(new Guid("a5df138d-fc6e-4f2f-a1c3-57f0564425b7"), "SO", "Somalia");

        public static Country SouthAfrica = 
                new Country(new Guid("c253988e-5309-4959-8187-e20e82e5e6b6"), "ZA", "South Africa");

        public static Country SouthGeorgiaIslands = 
                new Country(new Guid("6eed093d-9f30-466c-aaf8-9be86a322d4e"), "GS", "South Georgia Islands");

        public static Country Spain = 
                new Country(new Guid("25452cea-bd32-40c5-bcd9-867f6e5437f6"), "ES", "Spain");

        public static Country SriLanka = 
                new Country(new Guid("c4b5d195-36af-4832-be1e-1275c05b50ea"), "LK", "Sri Lanka");

        public static Country Sudan = 
                new Country(new Guid("cb420675-4f2e-491f-a775-2e6a3b9f6f0f"), "SD", "Sudan");

        public static Country Suriname = 
                new Country(new Guid("95122b90-8070-45c6-9b2c-f2d422750967"), "SR", "Suriname");

        public static Country SvalbardAndJanMayen = 
                new Country(new Guid("0322b5d4-d30e-4fa0-b3c7-c57c9dd2c02e"), "SJ", "Svalbard And Jan Mayen");

        public static Country Swaziland = 
                new Country(new Guid("10685925-d383-4cb6-9acc-0e705a787bee"), "SZ", "Swaziland");

        public static Country Sweden = 
                new Country(new Guid("9361ea4d-be2b-447b-84ff-8c668ec4aba3"), "SE", "Sweden");

        public static Country Switzerland = 
                new Country(new Guid("5ef527f6-3412-454a-9388-e8a64d3f3741"), "CH", "Switzerland");

        public static Country SyrianArabRepublic = 
                new Country(new Guid("67fab6d2-2f00-4eaa-a928-30d7feb904bf"), "SY", "Syrian Arab Republic");

        public static Country TaiwanProvinceOfChina = 
                new Country(new Guid("f085c24e-01b2-499d-92d8-0f1ff6a82e80"), "TW", "Taiwan - Province Of China");

        public static Country Tajikistan = 
                new Country(new Guid("b2d3ab53-4a7b-446c-aa42-68deded52efb"), "TJ", "Tajikistan");

        public static Country TanzaniaUnitedRepublicOf = 
                new Country(new Guid("9821c290-085d-46f7-b9c7-c03645220f43"), "TZ", "Tanzania - United Republic Of");

        public static Country Thailand = 
                new Country(new Guid("77d4a560-dddb-4645-af38-63acfb2abf23"), "TH", "Thailand");

        public static Country TimorLeste = 
                new Country(new Guid("1acb9543-d373-4eca-93dd-a3b8e062f772"), "TL", "Timor-Leste");

        public static Country Togo = 
                new Country(new Guid("871a0eeb-af1b-4824-8b80-04bf65ce7fc5"), "TG", "Togo");

        public static Country Tokelau = 
                new Country(new Guid("68ba7049-0941-42ab-99b8-12aea7ca2139"), "TK", "Tokelau");

        public static Country Tonga = 
                new Country(new Guid("fba9301f-7b84-4c46-b007-ad302d5f8eaf"), "TO", "Tonga");

        public static Country TrinidadAndTobago = 
                new Country(new Guid("d81ba0c8-b435-48c8-9a11-3471434eed68"), "TT", "Trinidad And Tobago");

        public static Country Tunisia = 
                new Country(new Guid("66166e62-c0ae-45bc-bd64-b344ab5a0f52"), "TN", "Tunisia");

        public static Country Turkey = 
                new Country(new Guid("fef549f9-231d-4ba3-9520-204c30216853"), "TR", "Turkey");

        public static Country Turkmenistan = 
                new Country(new Guid("ca267aa3-db24-4c4f-b791-f1f913e078c0"), "TM", "Turkmenistan");

        public static Country TurksAndCaicosIslands = 
                new Country(new Guid("b1eb7441-6a8e-4649-b9b3-a7d52a394188"), "TC", "Turks And Caicos Islands");

        public static Country Tuvalu = 
                new Country(new Guid("0f0eb9a9-a7b2-46fc-83d4-d52ca4a206ba"), "TV", "Tuvalu");

        public static Country Uganda = 
                new Country(new Guid("ab71a732-a667-4960-bc6d-134789629a01"), "UG", "Uganda");

        public static Country Ukraine = 
                new Country(new Guid("ad70d4b7-f432-4103-9127-be05ae0b19de"), "UA", "Ukraine");

        public static Country UnitedArabEmirates = 
                new Country(new Guid("a9e54182-10d4-46d3-ac11-03cf0fb9d9d8"), "AE", "United Arab Emirates");

        public static Country UnitedKingdom = 
                new Country(new Guid("0a405fde-73a3-4001-9b7c-b316b5c3c5be"), "GB", "United Kingdom");

        public static Country UnitedStates = 
                new Country(new Guid("77df51cc-675f-4f75-b177-1ca9f39fbe03"), "US", "United States");

        public static Country Uruguay = 
                new Country(new Guid("e909cf27-633d-4c5a-81f8-54e1e5703d0b"), "UY", "Uruguay");

        public static Country Uzbekistan = 
                new Country(new Guid("954165a9-ca58-4f37-9dc4-e1891a0c3e6f"), "UZ", "Uzbekistan");

        public static Country Vanuatu = 
                new Country(new Guid("13ad3a69-203a-4477-a717-13638d6ed182"), "VU", "Vanuatu");

        public static Country Venezuela = 
                new Country(new Guid("16d51eb8-f5fc-46c1-8955-d48d3b5117d9"), "VE", "Venezuela");

        public static Country VietNam = 
                new Country(new Guid("121700fc-541d-4c78-8b57-1d99556cb01f"), "VN", "Viet Nam");

        public static Country VirginIslandsBritish = 
                new Country(new Guid("462df1e3-2e48-49d5-a27b-00288468e382"), "VG", "Virgin Islands - British");

        public static Country VirginIslandsUS = 
                new Country(new Guid("33992c29-3cea-4d6f-8453-55cbe6149c2d"), "VI", "Virgin Islands - U.S.");

        public static Country WallisAndFutuna = 
                new Country(new Guid("bee264fc-ab66-431c-85d1-c8c966e4b36d"), "WF", "Wallis And Futuna");

        public static Country WesternSahara = 
                new Country(new Guid("4fc30dab-7046-4135-b1e2-20078e35583b"), "EH", "Western Sahara");

        public static Country Yemen = 
                new Country(new Guid("c53f8a0d-1a3b-4668-84fb-7218a81ca024"), "YE", "Yemen");

        public static Country Zambia = 
                new Country(new Guid("5055effd-e95d-4504-bbde-2cf138f26e5d"), "ZM", "Zambia");

        public static Country Zimbabwe = 
                new Country(new Guid("00b61a8a-e3ac-42c1-9dc9-dc2f4a152ad5"), "ZW", "Zimbabwe");

        public static IEnumerable<Country> GetValues()
        {
            yield return Afghanistan;
            yield return AlandIslands;
            yield return Albania;
            yield return Algeria;
            yield return AmericanSamoa;
            yield return Andorra;
            yield return Angola;
            yield return Anguilla;
            yield return Antarctica;
            yield return AntiguaAndBarbuda;
            yield return Argentina;
            yield return Armenia;
            yield return Aruba;
            yield return Australia;
            yield return Austria;
            yield return Azerbaijan;
            yield return Bahamas;
            yield return Bahrain;
            yield return Bangladesh;
            yield return Barbados;
            yield return Belarus;
            yield return Belgium;
            yield return Belize;
            yield return Benin;
            yield return Bermuda;
            yield return Bhutan;
            yield return Bolivia;
            yield return BosniaAndHerzegovina;
            yield return Botswana;
            yield return BouvetIsland;
            yield return Brazil;
            yield return BritishIndianOceanTerritory;
            yield return BruneiDarussalam;
            yield return Bulgaria;
            yield return BurkinaFaso;
            yield return Burundi;
            yield return Cambodia;
            yield return Cameroon;
            yield return Canada;
            yield return CapeVerde;
            yield return CaymanIslands;
            yield return CentralAfricanRepublic;
            yield return Chad;
            yield return Chile;
            yield return China;
            yield return ChristmasIsland;
            yield return CocosKeelingIslands;
            yield return Colombia;
            yield return Comoros;
            yield return Congo;
            yield return CookIslands;
            yield return CostaRica;
            yield return Croatia;
            yield return Cuba;
            yield return Cyprus;
            yield return CzechRepublic;
            yield return DemocraticRepublicOfTheCongo;
            yield return Denmark;
            yield return Djibouti;
            yield return Dominica;
            yield return DominicanRepublic;
            yield return Ecuador;
            yield return Egypt;
            yield return ElSalvador;
            yield return EquatorialGuinea;
            yield return Eritrea;
            yield return Estonia;
            yield return Ethiopia;
            yield return FalklandIslandsMalvinas;
            yield return FaroeIslands;
            yield return Fiji;
            yield return Finland;
            yield return France;
            yield return FrenchGuiana;
            yield return FrenchPolynesia;
            yield return FrenchSouthernTerritories;
            yield return Gabon;
            yield return Gambia;
            yield return Georgia;
            yield return Germany;
            yield return Ghana;
            yield return Gibraltar;
            yield return Greece;
            yield return Greenland;
            yield return Grenada;
            yield return Guadeloupe;
            yield return Guam;
            yield return Guatemala;
            yield return Guernsey;
            yield return Guinea;
            yield return GuineaBissau;
            yield return Guyana;
            yield return Haiti;
            yield return HeardIsland;
            yield return Honduras;
            yield return HongKong;
            yield return Hungary;
            yield return Iceland;
            yield return India;
            yield return Indonesia;
            yield return IranIslamicRepublicOf;
            yield return Iraq;
            yield return Ireland;
            yield return IsleOfMan;
            yield return Israel;
            yield return Italy;
            yield return Jamaica;
            yield return Japan;
            yield return Jersey;
            yield return Jordan;
            yield return Kazakhstan;
            yield return Kenya;
            yield return Kiribati;
            yield return KoreaDemocraticPeoplesRepublicOf;
            yield return KoreaRepublicOf;
            yield return Kuwait;
            yield return Kyrgyzstan;
            yield return LaoPeoplesDemocraticRepublic;
            yield return Latvia;
            yield return Lebanon;
            yield return Lesotho;
            yield return Liberia;
            yield return LibyanArabJamahiriya;
            yield return Liechtenstein;
            yield return Lithuania;
            yield return Luxembourg;
            yield return Macao;
            yield return MacedoniaTheRepublicOf;
            yield return Madagascar;
            yield return Malawi;
            yield return Malaysia;
            yield return Maldives;
            yield return Mali;
            yield return Malta;
            yield return MarshallIslands;
            yield return Martinique;
            yield return Mauritania;
            yield return Mauritius;
            yield return Mayotte;
            yield return Mexico;
            yield return MicronesiaFederatedStatesOf;
            yield return MoldovaRepublicOf;
            yield return Monaco;
            yield return Mongolia;
            yield return Montenegro;
            yield return Montserrat;
            yield return Morocco;
            yield return Mozambique;
            yield return Myanmar;
            yield return Namibia;
            yield return Nauru;
            yield return Nepal;
            yield return Netherlands;
            yield return NetherlandsAntilles;
            yield return NewCaledonia;
            yield return NewZealand;
            yield return Nicaragua;
            yield return Niger;
            yield return Nigeria;
            yield return Niue;
            yield return NorfolkIsland;
            yield return NorthernMarianaIslands;
            yield return Norway;
            yield return Oman;
            yield return Pakistan;
            yield return Palau;
            yield return PalestinianTerritoryOccupied;
            yield return Panama;
            yield return PapuaNewGuinea;
            yield return Paraguay;
            yield return Peru;
            yield return Philippines;
            yield return Pitcairn;
            yield return Poland;
            yield return Portugal;
            yield return PuertoRico;
            yield return Qatar;
            yield return Reunion;
            yield return Romania;
            yield return RussianFederation;
            yield return Rwanda;
            yield return SaintBarthelemy;
            yield return SaintKittsAndNevis;
            yield return SaintLucia;
            yield return SaintMartin;
            yield return SaintPierreAndMiquelon;
            yield return SaintVincentAndTheGrenadines;
            yield return Samoa;
            yield return SanMarino;
            yield return SaoTomeAndPrincipe;
            yield return SaudiArabia;
            yield return Senegal;
            yield return Serbia;
            yield return Seychelles;
            yield return SierraLeone;
            yield return Singapore;
            yield return Slovakia;
            yield return Slovenia;
            yield return SolomonIslands;
            yield return Somalia;
            yield return SouthAfrica;
            yield return SouthGeorgiaIslands;
            yield return Spain;
            yield return SriLanka;
            yield return Sudan;
            yield return Suriname;
            yield return SvalbardAndJanMayen;
            yield return Swaziland;
            yield return Sweden;
            yield return Switzerland;
            yield return SyrianArabRepublic;
            yield return TaiwanProvinceOfChina;
            yield return Tajikistan;
            yield return TanzaniaUnitedRepublicOf;
            yield return Thailand;
            yield return TimorLeste;
            yield return Togo;
            yield return Tokelau;
            yield return Tonga;
            yield return TrinidadAndTobago;
            yield return Tunisia;
            yield return Turkey;
            yield return Turkmenistan;
            yield return TurksAndCaicosIslands;
            yield return Tuvalu;
            yield return Uganda;
            yield return Ukraine;
            yield return UnitedArabEmirates;
            yield return UnitedKingdom;
            yield return UnitedStates;
            yield return Uruguay;
            yield return Uzbekistan;
            yield return Vanuatu;
            yield return Venezuela;
            yield return VietNam;
            yield return VirginIslandsBritish;
            yield return VirginIslandsUS;
            yield return WallisAndFutuna;
            yield return WesternSahara;
            yield return Yemen;
            yield return Zambia;
            yield return Zimbabwe;
        }
    }
}