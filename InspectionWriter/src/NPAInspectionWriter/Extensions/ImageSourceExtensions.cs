using System;
using NPAInspectionWriter.Models;
using NPAInspectionWriter.Logging;
using NPAInspectionWriter.Helpers;
using Xamarin.Forms;

namespace NPAInspectionWriter.Extensions
{
    public static class ImageSourceExtensions
    {
        const int cacheValidity = 45;

        public static ImageSource GetVehicleImageFromUrl( this VehicleRecord record )
        {
            return GetImageSourceFromUrl( record.PrimaryPictureUrl );
        }

        public static ImageSource GetVehicleImageFromUrl( this Vehicle vehicle )
        {
            return GetImageSourceFromUrl( vehicle.PrimaryPictureUrl );
        }

        private static ImageSource GetImageSourceFromUrl( string url )
        {
            try
            {
                Uri imageUri;

                if( !string.IsNullOrWhiteSpace( url ) && Uri.TryCreate( url, UriKind.Absolute, out imageUri ) )
                {
                    return new UriImageSource()
                    {
                        CachingEnabled = true,
                        CacheValidity = TimeSpan.FromDays( cacheValidity ),
                        Uri = imageUri
                    };
                }
            }
            catch( Exception e )
            {
                StaticLogger.ExceptionLogger?.Invoke( e );
            }

            return ImageSource.FromResource( Constants.VehicleNoImageSource );
        }
    }
}
