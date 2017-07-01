using System;
using System.Collections.Generic;
using System.Linq;
using NPAInspectionWriter.Models;
using NPAInspectionWriter.ViewModels;
using Xamarin.Forms;

namespace NPAInspectionWriter.Views.Pages
{
    public partial class VehicleDetailTabbedPage : TabbedPage
    {
        public VehicleDetailsViewModel ViewModel
        {
            get;
            set;
        }
		
        public string SearchQuery { get; set; }

        private Vehicle vehicle;

        public VehicleDetailTabbedPage(Vehicle vehicle)
        {
            InitializeComponent();
            Title = "Vehicle Details";
            this.vehicle = vehicle;
			BindingContext = ViewModel = new VehicleDetailsViewModel();

            this.BarBackgroundColor = Color.Black;
		}

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            ViewModel.SearchQuery = SearchQuery;

            ViewModel.Vehicle = vehicle;
            //ViewModel.Vehicle.RepairComments = ViewModel.Vehicle.VehicleComments;
            //ViewModel.Vehicle.PublicAuctionNotes += ViewModel.Vehicle.VehicleComments;
            if (string.IsNullOrWhiteSpace(ViewModel.Vehicle.PrimaryPictureUrl))
            {
                ViewModel.Vehicle.PrimaryPictureUrl = "NoImageVehicle.jpg";
            }
        }
    }
}
