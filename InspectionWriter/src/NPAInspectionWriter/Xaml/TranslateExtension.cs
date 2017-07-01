using System;
using System.Resources;
using NPAInspectionWriter.Logging;
using NPAInspectionWriter.AppData;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NPAInspectionWriter.Xaml
{
    [ContentProperty( nameof( Text ) )]
    public class TranslateExtension : IMarkupExtension
    {
        public string Text { get; set; }
        public object ProvideValue( IServiceProvider serviceProvider )
        {
            if( string.IsNullOrWhiteSpace( Text ) ) return string.Empty;

            ResourceManager manager = new ResourceManager( typeof( AppResources ) );
            var translation = manager.GetString( Text, AppResources.Culture );

            if( string.IsNullOrWhiteSpace( translation ) )
            {
                translation = Text;
                StaticLogger.DebugLogger?.Invoke( $"No value was found in {nameof( AppResources )} for '{Text}' for culture '{AppResources.Culture.Name}'" );
            }

            return translation;
        }
    }
}
