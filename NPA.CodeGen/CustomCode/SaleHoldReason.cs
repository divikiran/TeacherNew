using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPA.CodeGen
{
    public partial class SaleHoldReason
    {
        public static SaleHoldReason ClearHolds = new SaleHoldReason(Guid.Empty, -1, false, "ClearHolds");
    }
}
