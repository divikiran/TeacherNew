using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPA.CodeGen
{
    public partial class State : IEquatable<State>, IComparable<State>
    {
        public Guid StateGuid { get; private set; }
        public int StateId { get; private set; }
        public string Abbreviation { get; private set; }
        public string FullName { get; private set; }

        private State() { }

        private State(Guid stateGuid, int stateId, string abbreviation, string fullName)
        {
            this.StateGuid = stateGuid;
            this.StateId = stateId;
            this.Abbreviation = abbreviation;
            this.FullName = fullName;
        }
        
        public bool Equals(State other)
        {
            return this.StateGuid.Equals(other.StateGuid);
        }

        public int CompareTo(State other)
        {
            return this.Abbreviation.CompareTo(other.Abbreviation);
        }

        public override string ToString()
        {
            return FullName;
        }

        public static State FromGuid(Guid guid)
        {
            return GetValues().SingleOrDefault(s => s.StateGuid == guid);
        }

        public static State FromId(int id)
        {
            return GetValues().SingleOrDefault(s => s.StateId == id);
        }

        public static State FromAbbreviation(string abbrev)
        {
            return GetValues().SingleOrDefault(s => string.Compare(s.Abbreviation, abbrev, true) == 0);
        }

        public static State FromFullName(string fullName)
        {
            return GetValues().SingleOrDefault(s => string.Compare(s.FullName, fullName, true) == 0);
        }

        public static State Alaska = 
                new State(new Guid("9a9a3fee-5f93-4a37-8fa4-21d6d461147f"), 2, "AK", "Alaska");

        public static State Alabama = 
                new State(new Guid("a7234689-bf7a-423f-beb6-bdd692023ea8"), 1, "AL", "Alabama");

        public static State Arkansas = 
                new State(new Guid("f954d27b-a3e7-491e-8ee7-c2b1b8b2935f"), 5, "AR", "Arkansas");

        public static State Arizona = 
                new State(new Guid("f82a89a7-e222-4036-b749-8bcc78785827"), 4, "AZ", "Arizona");

        public static State California = 
                new State(new Guid("86141b63-7918-4440-a2e1-9cb271cf34c6"), 6, "CA", "California");

        public static State Colorado = 
                new State(new Guid("ed09a592-f947-49ea-80af-f4a0c57f46b6"), 7, "CO", "Colorado");

        public static State Connecticut = 
                new State(new Guid("0cd14261-692a-4d29-8878-f1138c9d8a52"), 8, "CT", "Connecticut");

        public static State DistrictOfColumbiaUs = 
                new State(new Guid("109e8adf-4221-48af-802f-9b713d628dd2"), 10, "DC", "District Of Columbia (Us)");

        public static State Delaware = 
                new State(new Guid("60ee4a2f-2da7-43a9-829a-01ca0525812a"), 9, "DE", "Delaware");

        public static State Florida = 
                new State(new Guid("1708ddfc-854c-4abb-a55d-fbd93e4dfc3a"), 12, "FL", "Florida");

        public static State Georgia = 
                new State(new Guid("e74d9b7b-5b6d-4441-8875-867eaa5e7f34"), 13, "GA", "Georgia");

        public static State GuamUs = 
                new State(new Guid("22a6c4d4-9095-40d2-b016-e2adef0970a6"), 64, "GU", "Guam (Us)");

        public static State Hawaii = 
                new State(new Guid("c2a49496-df37-407f-bb83-68ba9c86274e"), 15, "HI", "Hawaii");

        public static State Iowa = 
                new State(new Guid("da149b98-e6f5-4019-8a82-b4a7e432c8f0"), 19, "IA", "Iowa");

        public static State Idaho = 
                new State(new Guid("1276fbab-3ad2-46cc-a60c-f4750c5652c4"), 16, "ID", "Idaho");

        public static State Illinois = 
                new State(new Guid("e3333601-a649-46bf-b6e4-aa0dceb1939c"), 17, "IL", "Illinois");

        public static State Indiana = 
                new State(new Guid("d0b92d09-5d0b-4e2e-9dae-52faf1a6ce60"), 18, "IN", "Indiana");

        public static State Kansas = 
                new State(new Guid("57def620-0d98-49df-9b56-5aab8c287094"), 20, "KS", "Kansas");

        public static State Kentucky = 
                new State(new Guid("370c0b42-e552-4f47-bc33-b8fbd4dae9e4"), 21, "KY", "Kentucky");

        public static State Louisiana = 
                new State(new Guid("a4d23258-62de-49c7-b473-8a9b96202db0"), 22, "LA", "Louisiana");

        public static State Massachusetts = 
                new State(new Guid("bb630a35-39fa-4da2-b204-31635ddc02fc"), 26, "MA", "Massachusetts");

        public static State Maryland = 
                new State(new Guid("443cf7c9-7ce5-4886-80e3-dd22e28b4674"), 25, "MD", "Maryland");

        public static State Maine = 
                new State(new Guid("ed6d86b3-87e5-4ce9-a9ed-2885903cf465"), 23, "ME", "Maine");

        public static State Michigan = 
                new State(new Guid("e8f56ca0-85c0-47e0-9f4e-1c7228d125ed"), 27, "MI", "Michigan");

        public static State Minnesota = 
                new State(new Guid("0c1abd59-579d-49db-86e2-2da39e987dc2"), 28, "MN", "Minnesota");

        public static State Missouri = 
                new State(new Guid("ce05458c-95fe-409d-a373-9dfbedc4694e"), 30, "MO", "Missouri");

        public static State Mississippi = 
                new State(new Guid("bc0a8d75-d38b-4ae7-aa2b-f5b608b59072"), 29, "MS", "Mississippi");

        public static State Montana = 
                new State(new Guid("d8a699a7-9e9c-4cfe-bf09-1c4b62f0d214"), 31, "MT", "Montana");

        public static State NorthCarolina = 
                new State(new Guid("49af108c-5c15-4d1b-b6a1-efdb501b5d3d"), 38, "NC", "North Carolina");

        public static State NorthDakota = 
                new State(new Guid("4163cbde-1fe7-4a4c-9a5c-a07758fce510"), 39, "ND", "North Dakota");

        public static State Nebraska = 
                new State(new Guid("e316cce4-6c4d-486b-b545-1bb68a1bcc06"), 32, "NE", "Nebraska");

        public static State NewHampshire = 
                new State(new Guid("62dd2e03-7c32-44b8-af26-4cf529434000"), 34, "NH", "New Hampshire");

        public static State NewJersey = 
                new State(new Guid("58f22d52-e6a7-42fb-a84f-e49c7053be0e"), 35, "NJ", "New Jersey");

        public static State NewMexico = 
                new State(new Guid("c83b1141-e08f-4fc0-ba69-0976f8f032a6"), 36, "NM", "New Mexico");

        public static State Nevada = 
                new State(new Guid("ead23593-e61e-490b-94f4-309995696bb6"), 33, "NV", "Nevada");

        public static State NewYork = 
                new State(new Guid("eaccee95-1188-4efb-ae02-a309d12480fc"), 37, "NY", "New York");

        public static State Ohio = 
                new State(new Guid("5bf2a94d-7222-45d7-aa36-5d47398ee744"), 41, "OH", "Ohio");

        public static State Oklahoma = 
                new State(new Guid("aa35337d-71c4-4146-9cb5-dd550731fe69"), 42, "OK", "Oklahoma");

        public static State Oregon = 
                new State(new Guid("02cfc44f-ac25-4025-86f0-5fc5062f6faa"), 43, "OR", "Oregon");

        public static State Pennsylvania = 
                new State(new Guid("e8b8f1c7-051b-4c3b-8629-c1b0bc6a3278"), 45, "PA", "Pennsylvania");

        public static State PuertoRicoUs = 
                new State(new Guid("08babf68-a109-48d0-a67c-9cd80dc7a4da"), 63, "PR", "Puerto Rico (Us)");

        public static State RhodeIsland = 
                new State(new Guid("a68c9774-d9e8-4e29-88de-34eef01ee403"), 47, "RI", "Rhode Island");

        public static State SouthCarolina = 
                new State(new Guid("c3742d19-8e29-4f76-b6c6-1836e9883233"), 48, "SC", "South Carolina");

        public static State SouthDakota = 
                new State(new Guid("a9767107-d2f2-4821-9ba2-ff50d6f1eccc"), 49, "SD", "South Dakota");

        public static State Tennessee = 
                new State(new Guid("59acb7b6-2f32-4aa5-841f-1ed3088ac9ac"), 50, "TN", "Tennessee");

        public static State Texas = 
                new State(new Guid("f43531ba-b99a-4841-83b6-525c51163f3d"), 51, "TX", "Texas");

        public static State Utah = 
                new State(new Guid("d596e6bf-21d5-4352-b4a8-4986c43518f3"), 52, "UT", "Utah");

        public static State Virginia = 
                new State(new Guid("69f3f2a4-04d3-4663-93b2-a0420bb0e66e"), 55, "VA", "Virginia");

        public static State Vermont = 
                new State(new Guid("f16b7a28-02ae-4459-8afc-7404a0ed6132"), 53, "VT", "Vermont");

        public static State Washington = 
                new State(new Guid("84bc0eb4-d326-dd11-9dca-0019b9b35da2"), 60, "WA", "Washington");

        public static State Wisconsin = 
                new State(new Guid("4521cfab-2d9e-4004-836f-5a7148929f01"), 58, "WI", "Wisconsin");

        public static State WestVirginia = 
                new State(new Guid("85bc0eb4-d326-dd11-9dca-0019b9b35da2"), 61, "WV", "West Virginia");

        public static State Wyoming = 
                new State(new Guid("170ea3fd-860a-4842-8a0c-3ed1eb16b7ec"), 59, "WY", "Wyoming");

        public static IEnumerable<State> GetValues()
        {
            yield return Alaska;
            yield return Alabama;
            yield return Arkansas;
            yield return Arizona;
            yield return California;
            yield return Colorado;
            yield return Connecticut;
            yield return DistrictOfColumbiaUs;
            yield return Delaware;
            yield return Florida;
            yield return Georgia;
            yield return GuamUs;
            yield return Hawaii;
            yield return Iowa;
            yield return Idaho;
            yield return Illinois;
            yield return Indiana;
            yield return Kansas;
            yield return Kentucky;
            yield return Louisiana;
            yield return Massachusetts;
            yield return Maryland;
            yield return Maine;
            yield return Michigan;
            yield return Minnesota;
            yield return Missouri;
            yield return Mississippi;
            yield return Montana;
            yield return NorthCarolina;
            yield return NorthDakota;
            yield return Nebraska;
            yield return NewHampshire;
            yield return NewJersey;
            yield return NewMexico;
            yield return Nevada;
            yield return NewYork;
            yield return Ohio;
            yield return Oklahoma;
            yield return Oregon;
            yield return Pennsylvania;
            yield return PuertoRicoUs;
            yield return RhodeIsland;
            yield return SouthCarolina;
            yield return SouthDakota;
            yield return Tennessee;
            yield return Texas;
            yield return Utah;
            yield return Virginia;
            yield return Vermont;
            yield return Washington;
            yield return Wisconsin;
            yield return WestVirginia;
            yield return Wyoming;
        }
    }
}