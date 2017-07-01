using System;
using NPAInspectionWriter.iOS.Renderers;
using NPAInspectionWriter.Views.Pages;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Rg.Plugins.Popup.IOS.Renderers;


[assembly: ExportRenderer(typeof(CameraModal), typeof(LandscapePageRenderer))]
namespace NPAInspectionWriter.iOS.Renderers
{
    public class LandscapePageRenderer : PageRenderer
	{
		public override UIInterfaceOrientation PreferredInterfaceOrientationForPresentation() =>
		UIInterfaceOrientation.LandscapeLeft;

		public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations() =>
		UIInterfaceOrientationMask.Landscape;
	}
}
