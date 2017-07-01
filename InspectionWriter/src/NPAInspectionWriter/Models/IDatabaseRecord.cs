using System;

namespace NPAInspectionWriter.iOS.Models
{
    public interface IDatabaseRecord
    {
        DateTime LastSync { get; set; }
    }
}
