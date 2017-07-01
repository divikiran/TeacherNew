using System;
using System.Collections.Generic;

using Xamarin.Forms;
using NPAInspectionWriter.Views;
using NPAInspectionWriter.ViewModels;
using System.Threading.Tasks;
using NPAInspectionWriter.Services;
using NPAInspectionWriter.Controls;

namespace NPAInspectionWriter.Views.Pages
{
    public partial class VehicleSearchPage : NPABasePage
    {
        void BarcodeScanClicked(object sender, System.EventArgs e)
        {
            if (vm.ShowScanner)
            {
                var scanner = new BarcodeScannerView();
                scanner.Command = vm.BarcodeScannedCommand;
                BarcodeScannerView.Content = scanner;
            }
            else
                BarcodeScannerView.Content = null;
        }

        VehicleSearchViewModel vm;
        public VehicleSearchPage()
        {
            BindingContext = vm = new VehicleSearchViewModel();
            vm.Navigation = Navigation;
            InitializeComponent();
            NavigationPage.SetBackButtonTitle(this, "Back");
        }


    }
}
