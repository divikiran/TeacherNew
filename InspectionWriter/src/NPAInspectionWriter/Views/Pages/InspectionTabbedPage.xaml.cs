using System;
using System.Collections.Generic;

using Xamarin.Forms;
using NPAInspectionWriter.Models;
using Acr.UserDialogs;
using NPAInspectionWriter.ViewModels;
using NPAInspectionWriter.Services;
using NPAInspectionWriter.Helpers;

namespace NPAInspectionWriter.Views.Pages
{
    public partial class InspectionTabbedPage : TabbedPage
    {
        InspectionTabbedPageViewModel vm;
        public InspectionTabbedPage(LocalInspection inspection = null, Vehicle vehicle = null)
        {
            BindingContext = vm = new InspectionTabbedPageViewModel();
            vm.Navigation = Navigation;
            if (inspection?.Vehicle == null)
                inspection.Vehicle = vehicle;
            vm.CurrentInspection = inspection;
            InitializeComponent();
            this.BarBackgroundColor = Color.Black;
            InspectionCameraPage.tabbed = OverviewPage.tabbed = InspectionGalleryPage.tabbed = this;
            InspectionGalleryPage.ParentViewModel = vm;

            this.CurrentPageChanged += (sender, e) =>
            {
                var currentPage = ((InspectionTabbedPage)sender).CurrentPage as ImageGalleryPage;

                if (currentPage != null)
                { 
                    MessagingCenter.Send<InspectionTabbedPage, int>(this, "GalleryUpdated", 10);
                }

            };

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();


        }

        protected async override void OnDisappearing()
        {
            base.OnDisappearing();

            if (vm.CurrentInspection.IsLocalInspection)
                await AppRepository.Instance.SaveLocalInspection(vm.CurrentInspection);
        }
    }
}
