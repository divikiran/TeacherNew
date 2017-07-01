using NPAInspectionWriter.Helpers;

namespace NPAInspectionWriter.iOS.Models
{
    public enum OSSLicense
    {
        [Description( "Apache License 2.0" )]
        Apache,

        [Description( "BSD 3-Clause License" )]
        BSD3,

        [Description( "BSD 2-Clause License" )]
        BSD2,

        [Description( "GNU General Public License (GPL)" )]
        GPL,

        [Description( "GNU Library or \"Lesser\" General Public License (LGPL)" )]
        LGPL,

        [Description( "MIT License" )]
        MIT,

        [Description( "Mozilla Public License 2.0" )]
        Mozilla,

        [Description( "Beerware" )]
        Beerware,

        [Description( "Universal Permissive License (UPL)" )]
        UPL
    }
}
