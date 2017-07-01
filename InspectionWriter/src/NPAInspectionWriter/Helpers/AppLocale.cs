using System.Globalization;

namespace NPAInspectionWriter.Helpers
{
    public class AppLocale
    {
        ILocale _locale { get; }

        public AppLocale( ILocale locale )
        {
            _locale = locale;
        }

        public CultureInfo GetCultureInfo()
        {
            return new CultureInfo( _locale.GetCurrent() );
        }
    }
}
