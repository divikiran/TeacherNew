using NPAInspectionWriter.iOS.Renderers;
using NPAInspectionWriter.Views;
using NPAInspectionWriter.Views.Pages;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(NPABasePage), typeof(PageRendererOverride))]
[assembly: ExportRenderer(typeof(NavigationPage), typeof(NavigationRendererOverride))]
[assembly: ExportRenderer(typeof(TabbedPage), typeof(TabbedRendererOverride))]
namespace NPAInspectionWriter.iOS.Renderers
{
    static class OrientationOverrides
    {
        public static UIInterfaceOrientation PreferredInterfaceOrientationForPresentation() =>
            UIInterfaceOrientation.Portrait;

        public static UIInterfaceOrientationMask GetSupportedInterfaceOrientations()
        {
            if (PlatformSettings.Current.PortraitLock)
                return UIInterfaceOrientationMask.Portrait;

            return UIInterfaceOrientationMask.All;
        }
    }

    public class PageRendererOverride : PageRenderer
    {
        public override UIInterfaceOrientation PreferredInterfaceOrientationForPresentation() =>
            OrientationOverrides.PreferredInterfaceOrientationForPresentation();

        public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations() =>
            OrientationOverrides.GetSupportedInterfaceOrientations();
    }

    public class NavigationRendererOverride : NavigationRenderer
    {
        public override UIInterfaceOrientation PreferredInterfaceOrientationForPresentation() =>
            OrientationOverrides.PreferredInterfaceOrientationForPresentation();

        public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations() =>
            OrientationOverrides.GetSupportedInterfaceOrientations();
    }

    public class TabbedRendererOverride : TabbedRenderer
    {
        public override UIInterfaceOrientation PreferredInterfaceOrientationForPresentation() =>
            OrientationOverrides.PreferredInterfaceOrientationForPresentation();

        public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations() =>
            OrientationOverrides.GetSupportedInterfaceOrientations();

        /// <summary>
        /// Handles the <see cref="E:ElementChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="VisualElementChangedEventArgs"/> instance containing the event data.</param>
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            var page = (TabbedPage)Element;

            var user = new UILabel()
            {
                Text = $"{App.AppInfo.Location}\\{App.AppInfo.UserName}",
                TextColor = UIColor.White,
                TextAlignment = UITextAlignment.Center
            };
            user.Frame = new CoreGraphics.CGRect(1, (int)UIScreen.MainScreen.Bounds.Bottom - 105, 200, 30);
            user.Font = UIFont.FromName("HelveticaNeue", 15);
            var version = new UILabel()
            {
                Text = $"{App.AppInfo.AppVersion}",
			    TextColor = UIColor.White,
                TextAlignment = UITextAlignment.Center
            };
            version.Frame = new CoreGraphics.CGRect((int)UIScreen.MainScreen.Bounds.Width - 185, (int)UIScreen.MainScreen.Bounds.Bottom - 105, 200, 30);
            version.Font = UIFont.FromName("HelveticaNeue", 15);

            var subview = new UIView();
            subview.Add(user);
            subview.Add(version);

            View.Add(subview);
            View.BringSubviewToFront(subview);

        }

    }
}
