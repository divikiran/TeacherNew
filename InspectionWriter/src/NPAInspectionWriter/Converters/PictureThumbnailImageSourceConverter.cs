using System;
using System.Globalization;
using System.IO;
using NPAInspectionWriter.Helpers;
using NPAInspectionWriter.Services;
using NPAInspectionWriter.Models;
using Xamarin.Forms;

namespace NPAInspectionWriter.Converters
{
    public class PictureThumbnailImageSourceConverter : IValueConverter
    {
        static ImageSource defaultImage = ImageSource.FromResource( "NPAInspectionWriter.Assets.Images.no-image.jpg" );

        public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
        {
            var model = value as Picture;

            if( model == null ) return defaultImage;

            Uri result = null;
            var format = $"{model.BaseUrl}&width=160&height=120";
            ImageSource source = null;

            if( model.ImageData?.Length > 0 )
            {
                source = ImageSource.FromStream( () => new MemoryStream( model.ImageData ) );
            }
            else if( model.LocalImage && !string.IsNullOrWhiteSpace( model.LocalPath ) )
            {
                try
                {
                    var fileSystem = NPADependencyService.Resolve<IApplicationFileSystem>();
                    source = ImageSource.FromStream( () => fileSystem.GetFileStream( model.LocalPath ) );
                }
                catch( Exception e )
                {
                    NPAInspectionWriter.Logging.StaticLogger.WarningLogger( "Unable to locate Image Asset" );
                    NPAInspectionWriter.Logging.StaticLogger.WarningLogger( e );
                }
            }
            else if( !string.IsNullOrWhiteSpace( model.BaseUrl ) && Uri.TryCreate( string.Format( format, model.Id ), UriKind.Absolute, out result ) )
            {
                source = new UriImageSource()
                {
                    CachingEnabled = true,
                    CacheValidity = TimeSpan.FromDays( 45 ),
                    Uri = result
                };
            }

            if( source == null )
            {
                source = defaultImage;
            }

            return source;
        }

        public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
        {
            throw new NotImplementedException();
        }
    }
}
