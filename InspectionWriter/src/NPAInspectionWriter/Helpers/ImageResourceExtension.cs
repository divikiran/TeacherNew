using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NPAInspectionWriter.Helpers
{
    internal class ImageResourceExtension : ImageResourceExtensionBase
    {
        protected override Assembly GetAssembly() => typeof( ImageResourceExtension ).GetTypeInfo().Assembly;
    }

    // Reference https://developer.xamarin.com/guides/xamarin-forms/working-with/images/#Using_Xaml
    // Needed to automagically load images in Xaml
    [ContentProperty( "Source" )]
    public abstract class ImageResourceExtensionBase : IMarkupExtension
    {
        public string Source { get; set; }

        public object ProvideValue( IServiceProvider serviceProvider )
        {
            if( Source == null ) return null;

            var imageSource = ImageSource.FromStream( GetStream );

            return imageSource;
        }

        protected virtual Stream GetStream()
        {
            var assembly = GetAssembly();
            var resource = assembly.GetManifestResourceNames().FirstOrDefault( r => r.EndsWith( Source ) );
            return assembly.GetManifestResourceStream( resource );
        }

        protected abstract Assembly GetAssembly();
    }
}
