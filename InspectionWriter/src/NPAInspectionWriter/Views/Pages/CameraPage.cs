using System;

using Xamarin.Forms;
using NPAInspectionWriter.Controls;
using NPAInspectionWriter.ViewModels;
using Rg.Plugins.Popup.Extensions;
using System.Diagnostics;
using NPAInspectionWriter.Extensions;
using System.Linq;

namespace NPAInspectionWriter.Views.Pages
{
    public class CameraPage : NPABasePage
    {
		InspectionTabbedPage _tabbed;
		public InspectionTabbedPage tabbed
		{
			get { return _tabbed; }
			set
			{
				_tabbed = value;
				BindingContext = _tabbed.BindingContext;
			}
		}

	    protected override void OnAppearing()
        {
            base.OnAppearing();
            var modal = new CameraModal();
            modal.BindingContext = BindingContext;
            Navigation.PushModalAsync(modal);
            tabbed.CurrentPage = tabbed.Children[2];
        }
    }
}

