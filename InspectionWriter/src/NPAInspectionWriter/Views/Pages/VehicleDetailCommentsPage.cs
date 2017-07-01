using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using NPAInspectionWriter.Models;
using NPAInspectionWriter.Services;
using NPAInspectionWriter.ViewModels;
using Xamarin.Forms;

namespace NPAInspectionWriter.Views.Pages
{
    public partial class VehicleDetailCommentsPage : ContentPage
    {
        //public VehicleDetailsViewModel ViewModel
        //{
        //    get;
        //    set;
        //}
        //public string SearchQuery { get; set; }

        public VehicleDetailCommentsPage()
        {
            //BindingContext = ViewModel = new VehicleDetailsViewModel();
            InitializeComponent();
            //NavigationPage.SetBackButtonTitle(this, "Back");
        }

//        protected override async void OnAppearing()
//        {
//            base.OnAppearing();
//            //ViewModel.SearchQuery = SearchQuery;
//#if USE_MOCKS
//            VehicleServiceMock vSM = new VehicleServiceMock();
//            ViewModel.Vehicle = await vSM.SearchAsync(new VehicleSearchRequest());

//#else
//            //TODO: Prod code
//#endif
        //    if (string.IsNullOrEmpty(ViewModel.Vehicle.PrimaryPictureUrl))
        //    {
        //        ViewModel.Vehicle.PrimaryPictureUrl = "https://s-media-cache-ak0.pinimg.com/originals/7d/3c/0b/7d3c0b83cf39b9d81a88a6a643b0de21.jpg";
        //    }


        //}
    }
}
