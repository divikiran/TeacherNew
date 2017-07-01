using System;
using MvvmHelpers;
using NPAInspectionWriter.Models;
using System.Windows.Input;
using Xamarin.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using NPAInspectionWriter.Views.Pages;
using NPAInspectionWriter.Services;
using NPAInspectionWriter.Helpers;
using System.Collections.Generic;

namespace NPAInspectionWriter.ViewModels
{
    public class VehicleSearchListViewModel : CRWriterBase
    {
        ObservableRangeCollection<Vehicle> _vehicles = new ObservableRangeCollection<Vehicle>();
        public ObservableRangeCollection<Vehicle> Vehicles
        {
            get { return _vehicles; }
            set { SetProperty(ref(_vehicles), value); }
        }

        public ICommand ItemSelectedCommand {
            get{
                return new Command<Vehicle>(async (vehicle) =>
                {
                    var vehicleService = new VehicleService(new InspectionWriterClient(), AppRepository.Instance);
					var inspections = await vehicleService.GetInspectionsAsync(vehicle);

					foreach (KeyValuePair<string, IEnumerable<Inspection>> entry in inspections)
					{
						var linkedInspections = entry.Value;
						foreach (var inspection in linkedInspections)
						{
							inspection.StockNumber = entry.Key;
							vehicle.AvailableInspections.Add(inspection);
						}
					}
                    var vehicleTabbedpage = new VehicleDetailTabbedPage(vehicle);
                    await Navigation.PushAsync(vehicleTabbedpage);
                });
            }
        }

        public override string Title => "Search Results" ;

        public override ImageSource Icon => "Default";

        public VehicleSearchListViewModel()
        {
            
        }
    }
}