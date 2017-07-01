using System;
using Xamarin.Forms;
using Acr.UserDialogs;
using NPAInspectionWriter.Services;
using NPAInspectionWriter.ViewModels;

namespace NPAInspectionWriter.Views.Pages
{
    public class ImageGalleryPage : Page
    {
        public InspectionTabbedPage tabbed { get; set; }

        private InspectionTabbedPageViewModel _parentViewModel;
		public InspectionTabbedPageViewModel ParentViewModel
		{
            get { return _parentViewModel; }
			set
			{
				_parentViewModel = value;
				BindingContext = _parentViewModel;
			}
		}

        public ImageGalleryViewModel ViewModel
        {
            get;
            set;
        }

        public ImageGalleryPage()
        {
            BindingContext = ViewModel = new ImageGalleryViewModel() { ParentViewModel = ParentViewModel };
        }
    }
}