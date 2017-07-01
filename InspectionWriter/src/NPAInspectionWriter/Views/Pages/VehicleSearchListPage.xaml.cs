using System;
using System.Collections.Generic;
using NPAInspectionWriter.Models;
using Xamarin.Forms;
using NPAInspectionWriter.ViewModels;
using MvvmHelpers;
using NPAInspectionWriter.Helpers;

namespace NPAInspectionWriter.Views.Pages
{
    public partial class VehicleSearchListPage : NPABasePage
    {
        void ItemSelectedAction(object sender, SelectedItemChangedEventArgs e)
        {
            vm.ItemSelectedCommand.Execute(e.SelectedItem);
        }

        VehicleSearchListViewModel vm = new VehicleSearchListViewModel();

        public VehicleSearchListPage(List<Vehicle> vehicleCollection)
        {
            BindingContext = vm;
            vm.Navigation = Navigation;
            InitializeComponent();
            vehicleList.ItemsSource = vehicleCollection;
            NavigationPage.SetBackButtonTitle(this, "Back");
            NavigationPage.SetHasBackButton(this, true);
        }
    }
}
