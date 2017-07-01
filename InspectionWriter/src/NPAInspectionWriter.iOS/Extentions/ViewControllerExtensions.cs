using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundation;
using UIKit;

namespace NPAInspectionWriter.iOS.Extensions
{
    public static class ViewControllerExtensions
    {
        public static void DismissViewController( this UIViewController controller )
        {
            controller.DismissViewController( true, null );
        }

        public static Task DismissViewControllerAsync( this UIViewController controller )
        {
            return controller.DismissViewControllerAsync( true );
        }
    }
}
