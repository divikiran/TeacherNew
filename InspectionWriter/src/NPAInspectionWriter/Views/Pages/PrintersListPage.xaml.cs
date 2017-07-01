using System;
using System.Collections.Generic;
using NPAInspectionWriter.Models;
using NPAInspectionWriter.ViewModels;
using Xamarin.Forms;

namespace NPAInspectionWriter.Views.Pages
{
    public partial class PrintersListPage : ContentPage
    {
        private VehicleDetailsViewModel vehicleDetailsViewModel;

        public PrintersListPage(VehicleDetailsViewModel vehicleDetailsViewModel)
        {
            this.vehicleDetailsViewModel = vehicleDetailsViewModel;
            //BindingContext = vehicleDetailsViewModel;
            InitializeComponent();
        }
    }
}
