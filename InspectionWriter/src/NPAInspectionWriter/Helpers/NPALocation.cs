using System;

namespace NPAInspectionWriter.Helpers
{
    public struct NPALocation
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public bool IsPhysicalLocation { get; set; }
    }
}
