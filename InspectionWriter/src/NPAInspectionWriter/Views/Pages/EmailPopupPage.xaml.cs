using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Rg.Plugins.Popup.Pages;
using NPAInspectionWriter.Models;
using NPAInspectionWriter.ViewModels;
using System.Threading.Tasks;

namespace NPAInspectionWriter.Views.Pages
{
    public partial class EmailPopupPage : PopupPage
    {
        EmailViewModel vm;
        public EmailPopupPage()
        {
            BindingContext = vm = new EmailViewModel();
            vm.Navigation = Navigation;
            InitializeComponent();
        }

		// Method for animation child in PopupPage
		// Invoced after custom animation end
		protected virtual Task OnAppearingAnimationEnd()
		{
			return Content.FadeTo(0.5);
		}

		// Method for animation child in PopupPage
		// Invoked before custom animation begin
		protected virtual Task OnDisappearingAnimationBegin()
		{
			return Content.FadeTo(1); ;
		}

		protected override bool OnBackButtonPressed()
		{
			// Prevent hide popup
			//return base.OnBackButtonPressed();
			return true;
		}

		// Invoced when background is clicked
		protected override bool OnBackgroundClicked()
		{
			// Return default value - CloseWhenBackgroundIsClicked
			return base.OnBackgroundClicked();
		}
	}

}
