using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using NPA.Common;

namespace InspectionWriterWebApi.Utilities
{
    public class ImageUtils
    {
        public static void ShrinkAndSaveImage(string file, string newFullPath, int newWidth, int newHeight)
        {
            Bitmap img = ShrinkImage(file, newWidth, newHeight);

            EncoderParameters eps = new EncoderParameters(1);
            eps.Param[0] = new EncoderParameter(Encoder.Quality, 70L);
            ImageCodecInfo ici = GetEncoderInfo(ImageFormat.Jpeg);  //"image/jpeg"

            if (newFullPath.IsEmpty())
            {
                System.IO.File.Delete(file);
                img.Save(file, ici, eps);
            }
            else
            {
                img.Save(newFullPath, ici, eps);
            }
            img.Dispose();
        }

        public static Bitmap ShrinkImage(string sFile, int newWidth, int newHeight)
        {
            Bitmap newBmp = null;
            Image img = null;

            try
            {
                img = new Bitmap(sFile);


                //scale the drawing if necessary
                if (newWidth > 0 && newHeight > 0)
                {
                    double dScaleBy = 1;
                    double dWidthRatio = Convert.ToDouble(newWidth) / Convert.ToDouble(img.Width);
                    double dHeightRatio = Convert.ToDouble(newHeight) / Convert.ToDouble(img.Height);

                    if (dWidthRatio < dHeightRatio)
                    {
                        dScaleBy = dWidthRatio;
                    }
                    else
                    {
                        dScaleBy = dHeightRatio;
                    }

                    if (dScaleBy < 1) //really needs scaling
                    {
                        newBmp = new Bitmap((int)System.Math.Round(img.Width * dScaleBy, System.MidpointRounding.AwayFromZero),
                           (int)System.Math.Round(img.Height * dScaleBy, System.MidpointRounding.AwayFromZero));
                    }
                    else //already within limits
                    {
                        newBmp = new Bitmap(img.Width, img.Height);
                    }
                }
                else //user opted for no scaling
                {
                    newBmp = new Bitmap(img.Width, img.Height);
                }

                //move PropertyItems such as GPS data over to the new image
                System.Drawing.Imaging.PropertyItem[] pis = img.PropertyItems;
                foreach (System.Drawing.Imaging.PropertyItem pi in pis)
                {
                    newBmp.SetPropertyItem(pi);
                }

                Graphics g = Graphics.FromImage(newBmp);
                g.DrawImage(img, 0, 0, newBmp.Width - 1, newBmp.Height - 1);
                g.Dispose();

                EncoderParameters eps = new EncoderParameters(1);
                eps.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 70);
                ImageCodecInfo ici = GetEncoderInfo(ImageFormat.Jpeg);  //"image/jpeg"

                img.Dispose();
            }
            catch (Exception ex)
            {
                LogUtil.WriteLogEntry("InspectionWriterWebApi", "Error during ShrinkImage: " + ex, System.Diagnostics.EventLogEntryType.Error);
                IwLogUtil.SendExceptionNotification(ex, "Exception during image resizing: ");
            }
            return newBmp;
        }

        private static ImageCodecInfo GetEncoderInfo(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }
    }
}