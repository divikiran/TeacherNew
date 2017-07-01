using Foundation;
using System;
using UIKit;

namespace NPAInspectionWriter.iOS
{
    public partial class ImageCollectionCell : UICollectionViewCell
    {
		public static readonly NSString Key = new NSString("ImageCell");
		public static readonly UINib Nib;

		#region Computed Properties
		public string Title
		{
			get
			{
				return TextLabel.Text;
			}
			set
			{
				TextLabel.Text = value;
			}
		}
		#endregion
		
        public ImageCollectionCell (IntPtr handle) : base (handle)
        {
        }
    }
}