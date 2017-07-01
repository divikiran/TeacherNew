using System;
using System.Collections;
using System.Collections.Generic;

namespace InspectionWriterWebApi.Common.Tests.Data
{
    public class MasterWithPreviousValData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator() => _categoryData.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        IEnumerable<object[]> _categoryData
        {
            get
            {
                return new[]
                {
                    new object[]
                    {
                        new Guid( "dc3f6956-e7db-40e8-a7f8-7e0e509fa456" ),
                        "*MX/OFFROAD",
                        true,
                        100,
                        new Guid( "8713ae8a-de8c-4ead-a5a4-3683c5990464" ),
                        "*ENGINE MECHANICAL",
                        25,
                        10,
                        true,
                        new Guid( "dfa1bbcd-b30e-e311-93f7-ac162d7cc3cb" ),
                        "RUNS GOOD/NORMAL",
                        9
                    },
                    new object[]
                    {
                        new Guid( "dc3f6956-e7db-40e8-a7f8-7e0e509fa456" ),
                        "*MX/OFFROAD",
                        true,
                        100,
                        new Guid( "42805301-122a-460d-97d0-6347eb6cd65e" ),
                        "FRAME OFF-ROAD",
                        15,
                        10,
                        true,
                        new Guid( "91074866-9d24-e311-93fb-ac162d7cbbd1" ),
                        "COSMETIC LIGHT",
                        8
                    },
                    new object[]
                    {
                        new Guid( "dc3f6956-e7db-40e8-a7f8-7e0e509fa456" ),
                        "*MX/OFFROAD",
                        true,
                        100,
                        new Guid( "f1ca4a02-282d-41a8-b5e5-12b275d47221" ),
                        "*STAND",
                        1,
                        10,
                        false,
                        null,
                        string.Empty,
                        0
                    }
                };
            }
        }
    }
}
