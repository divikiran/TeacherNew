using System;

namespace InspectionWriterWebApi.Helpers
{
    public class DisplayTextAttribute : Attribute
    {
        public string Text { get; set; }

        public DisplayTextAttribute( string text )
        {
            Text = text;
        }
    }
}
