using System;
using System.Drawing;
using System.IO;
using CoreGraphics;
using Foundation;
using UIKit;

namespace NPAInspectionWriter.iOS.Extensions
{
    public static class UIImageExtensions
    {
        public static UIImage ScaleAndRotateImage( this UIImage imageIn, UIImageOrientation orIn )
        {
            int kMaxResolution = 2048;

            CGImage imgRef = imageIn.CGImage;
            float width = imgRef.Width;
            float height = imgRef.Height;
            CGAffineTransform transform = CGAffineTransform.MakeIdentity();
            RectangleF bounds = new RectangleF( 0, 0, width, height );

            if( width > kMaxResolution || height > kMaxResolution )
            {
                float ratio = width / height;

                if( ratio > 1 )
                {
                    bounds.Width = kMaxResolution;
                    bounds.Height = bounds.Width / ratio;
                }
                else
                {
                    bounds.Height = kMaxResolution;
                    bounds.Width = bounds.Height * ratio;
                }
            }

            float scaleRatio = bounds.Width / width;
            SizeF imageSize = new SizeF( width, height );
            UIImageOrientation orient = orIn;
            float boundHeight;

            switch( orient )
            {
                case UIImageOrientation.Up:                                        //EXIF = 1
                    transform = CGAffineTransform.MakeIdentity();
                    break;

                case UIImageOrientation.UpMirrored:                                //EXIF = 2
                    transform = CGAffineTransform.MakeTranslation( imageSize.Width, 0f );
                    transform = CGAffineTransform.MakeScale( -1.0f, 1.0f );
                    break;

                case UIImageOrientation.Down:                                      //EXIF = 3
                    transform = CGAffineTransform.MakeTranslation( imageSize.Width, imageSize.Height );
                    transform = CGAffineTransform.Rotate( transform, ( float )Math.PI );
                    break;

                case UIImageOrientation.DownMirrored:                              //EXIF = 4
                    transform = CGAffineTransform.MakeTranslation( 0f, imageSize.Height );
                    transform = CGAffineTransform.MakeScale( 1.0f, -1.0f );
                    break;

                case UIImageOrientation.LeftMirrored:                              //EXIF = 5
                    boundHeight = bounds.Height;
                    bounds.Height = bounds.Width;
                    bounds.Width = boundHeight;
                    transform = CGAffineTransform.MakeTranslation( imageSize.Height, imageSize.Width );
                    transform = CGAffineTransform.MakeScale( -1.0f, 1.0f );
                    transform = CGAffineTransform.Rotate( transform, 3.0f * ( float )Math.PI / 2.0f );
                    break;

                case UIImageOrientation.Left:                                      //EXIF = 6
                    boundHeight = bounds.Height;
                    bounds.Height = bounds.Width;
                    bounds.Width = boundHeight;
                    transform = CGAffineTransform.MakeTranslation( 0.0f, imageSize.Width );
                    transform = CGAffineTransform.Rotate( transform, 3.0f * ( float )Math.PI / 2.0f );
                    break;

                case UIImageOrientation.RightMirrored:                             //EXIF = 7
                    boundHeight = bounds.Height;
                    bounds.Height = bounds.Width;
                    bounds.Width = boundHeight;
                    transform = CGAffineTransform.MakeScale( -1.0f, 1.0f );
                    transform = CGAffineTransform.Rotate( transform, ( float )Math.PI / 2.0f );
                    break;

                case UIImageOrientation.Right:                                     //EXIF = 8
                    boundHeight = bounds.Height;
                    bounds.Height = bounds.Width;
                    bounds.Width = boundHeight;
                    transform = CGAffineTransform.MakeTranslation( imageSize.Height, 0.0f );
                    transform = CGAffineTransform.Rotate( transform, ( float )Math.PI / 2.0f );
                    break;

                default:
                    throw new Exception( "Invalid image orientation" );
            }

            UIGraphics.BeginImageContext( bounds.Size );

            CGContext context = UIGraphics.GetCurrentContext();

            if( orient == UIImageOrientation.Right || orient == UIImageOrientation.Left )
            {
                context.ScaleCTM( -scaleRatio, scaleRatio );
                context.TranslateCTM( -height, 0 );
            }
            else
            {
                context.ScaleCTM( scaleRatio, -scaleRatio );
                context.TranslateCTM( 0, -height );
            }

            context.ConcatCTM( transform );
            context.DrawImage( new RectangleF( 0, 0, width, height ), imgRef );

            UIImage imageCopy = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();

            return imageCopy;
        }

        public static UIImage Rotate( this UIImage image, UIImageOrientation preferredOrientation )
        {
            if( image.Orientation == preferredOrientation ) return image;

            CGAffineTransform transform = CGAffineTransform.MakeIdentity();

            switch( image.Orientation )
            {
                case UIImageOrientation.Down:
                case UIImageOrientation.DownMirrored:
                    transform = CGAffineTransform.Translate( transform, image.Size.Width, image.Size.Height );
                    transform = CGAffineTransform.Rotate( transform, ( float )Math.PI );
                    break;
                case UIImageOrientation.Left:
                case UIImageOrientation.LeftMirrored:
                    transform = CGAffineTransform.Translate( transform, image.Size.Width, 0 );
                    transform = CGAffineTransform.Rotate( transform, ( float )Math.PI / 2.0f );
                    break;
                case UIImageOrientation.Right:
                case UIImageOrientation.RightMirrored:
                    transform = CGAffineTransform.Translate( transform, 0, image.Size.Height );
                    transform = CGAffineTransform.Rotate( transform, ( float )-Math.PI );
                    break;
                    // no need to update the CGAffineTransform object if the preferred orientation is Portrait
            }

            // Handle Mirrored Images
            switch( image.Orientation )
            {
                case UIImageOrientation.UpMirrored:
                case UIImageOrientation.DownMirrored:
                    transform = CGAffineTransform.Translate( transform, image.Size.Width, 0 );
                    transform = CGAffineTransform.Scale( transform, -1, 1 );
                    break;
                case UIImageOrientation.LeftMirrored:
                case UIImageOrientation.RightMirrored:
                    transform = CGAffineTransform.Translate( transform, image.Size.Height, 0 );
                    transform = CGAffineTransform.Scale( transform, -1, 1 );
                    break;
            }

            // TODO: Finish implementing this.
            // let ctx: CGContextRef = CGBitmapContextCreate( nil, Int( size.width ), Int( size.height ), CGImageGetBitsPerComponent( CGImage ), 0,
            //                              CGImageGetColorSpace( CGImage ), CGImageAlphaInfo.PremultipliedLast.rawValue )!
            var ctx = new CGBitmapContext( data: null,
                                            width:  (nint)image.Size.Width,
                                            height: (nint)image.Size.Height,
                                            bitsPerComponent: image.CGImage.BitsPerComponent,
                                            bytesPerRow: 0,
                                            colorSpace: image.CGImage.ColorSpace,
                                            bitmapInfo: image.CGImage.BitmapInfo );
            // CGContextConcatCTM( ctx, transform )
            ctx.ConcatCTM( transform );

            //switch imageOrientation {
            //    case UIImageOrientation.Left, UIImageOrientation.LeftMirrored, UIImageOrientation.Right, UIImageOrientation.RightMirrored:
            //          CGContextDrawImage( ctx, CGRectMake( 0, 0, size.height, size.width ), CGImage )
            //          break
            //      default:
            //        CGContextDrawImage( ctx, CGRectMake( 0, 0, size.width, size.height ), CGImage )
            //          break
            // }
            switch( preferredOrientation )
            {
                case UIImageOrientation.Left:
                case UIImageOrientation.LeftMirrored:
                case UIImageOrientation.Right:
                case UIImageOrientation.RightMirrored:
                    ctx.DrawImage( new CGRect( 0, 0, image.Size.Height, image.Size.Width ), image.CGImage );
                    break;
                default:
                    ctx.DrawImage( new CGRect( 0, 0, image.Size.Width, image.Size.Height ), image.CGImage );
                    break;
            }

            // let cgImage: CGImageRef = CGBitmapContextCreateImage( ctx )!
            var cgImage = ctx.ToImage();


            // return UIImage( CGImage: cgImage )
            return new UIImage( cgImage );
        }

        public static UIImage Resize( this UIImage image, int width, int height )
        {
            double scale = 1;

            if( width > height && image.Size.Width > image.Size.Height ||
                height > width && image.Size.Height > image.Size.Width )
            {
                scale = width / image.Size.Width;
            }
            else
            {
                // Desired Resize is for a Landscape image and the image is in Portrait
                // Or the image is Landscape and the desired resize is for Portrait
                scale = width / image.Size.Height;
            }

            var newWidth = scale * image.Size.Width;
            var newHeight = scale * image.Size.Height;

            return image.Scale( new CGSize( width, height ) );
        }

        public static bool SaveAndResize( this UIImage image, string path, int width, int height, bool saveAtomically = true ) =>
            image.ScaleAndRotateImage(UIImageOrientation.Up).Resize( width, height ).Save( path, saveAtomically );

        public static bool Save( this UIImage image, string path, bool atomically = true )
        {
            try
            {
                var dir = Path.GetDirectoryName( path );
                if( !Directory.Exists( dir ) )
                {
                    Directory.CreateDirectory( dir );
                }

                string fileExtension = "jpeg";

                if( Path.HasExtension( path ) )
                    fileExtension = Path.GetExtension( path );

                NSData imageData = fileExtension == "png" ? image.AsPNG() : image.AsJPEG();

                NSError err = new NSError();

                return imageData.Save( path, atomically );
            }
            catch( Exception e )
            {
                Logging.StaticLogger.ExceptionLogger( e );
            }

            return false;
        }
    }
}
