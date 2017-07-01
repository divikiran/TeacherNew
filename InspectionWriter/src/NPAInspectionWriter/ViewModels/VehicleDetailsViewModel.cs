using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using NPAInspectionWriter.AppData;
using NPAInspectionWriter.Helpers;
using NPAInspectionWriter.Models;
using NPAInspectionWriter.Services;
using NPAInspectionWriter.Views;
using NPAInspectionWriter.Views.Pages;
using Xamarin.Forms;
using System.Diagnostics;
using NPAInspectionWriter.Device;

namespace NPAInspectionWriter.ViewModels
{
    public class VehicleDetailsViewModel : CRWriterBase
    {
        string _searchQuery;
        public string SearchQuery
        {
            get { return _searchQuery?.ToUpper(); }
            set
            {
                //_searchQuery = _searchQuery?.ToUpper();
                SetProperty(ref _searchQuery, value);
            }
        }

        Vehicle _vehicle;
        public Vehicle Vehicle
        {
            get { return _vehicle; }
            set
            {
                SetProperty(ref _vehicle, value);
                RaisePropertyChanged(nameof(YearAndBrand));
                RaisePropertyChanged(nameof(AvailableInspectionsList));
            }
        }

        string _yearAndBrand;
        public string YearAndBrand
        {
            get
            {
                if (Vehicle != null)
                {
                    _yearAndBrand = $"{Vehicle.Year} {Vehicle.Brand}";
                }
                return _yearAndBrand;

            }
        }

        public override string Title => "Details";

        public override ImageSource Icon => "Default";

        public ObservableCollection<ObservableGroupCollection<string, InspectionDisplayList>> AvailableInspectionsList
        {
            get
            {
                if (Vehicle != null)
                {
                    var inspections = Vehicle.AvailableInspections;
                    var listData = new ObservableCollection<InspectionDisplayList>();
                    if (inspections != null && inspections.Count > 0)
                    {
                        foreach (var item in inspections)
                        {
                            var inspection = new InspectionDisplayList(item);
                            listData.Add(inspection);
                        }
                    }

                    this.AllowNewInspections = Vehicle.AllowNewInspections;

                    var groupedData =
                           listData.OrderBy(e => e.Text)
                           .GroupBy(e => e.GroupText.ToString())
                                      .Select(e => new ObservableGroupCollection<string, InspectionDisplayList>(e))
                           .ToList();
                    var data = new ObservableCollection<ObservableGroupCollection<string, InspectionDisplayList>>(groupedData);
                    return data;
                }
                else
                {
                    return null;
                }
            }

        }

        public PrintService PrintService
        {
            get;
            set;
        } = new PrintService(new InspectionWriterClient());

        public ICommand SelectPrinterAction
        {
            get
            {
                return new Command(async () =>
               {
                    UserDialogs.Instance.ShowLoading("Loading Printers...");
                   if (this.PrintersList == null || this.PrintersList?.Count() == 0)
                   {
                       IsWaiting = true;

                       //Get printers list
                       var user = await AppRepository.Instance.GetCurrentUserAsync();
                       var printersList = await PrintService.GetPrinterModelAsync();

                       if (printersList != null && printersList.Count() >= 0)
                       {
                           this.PrintersList = new ObservableCollection<PrinterModel>(printersList);
                       }
                       IsWaiting = false;
                   }

                   PrintersListPage printersListPage = new PrintersListPage(this)
                   {
                       BindingContext = this
                   };
				   UserDialogs.Instance.HideLoading();
                   await Navigation.PushModalAsync(printersListPage);
               });
            }
        }

        public ICommand PrinterTapped
        {
            get
            {
                return new Command(async (selectedArguments) =>
               {
                   var castedArguments = selectedArguments as SelectedItemChangedEventArgs;
                   SelectedPrinter = castedArguments.SelectedItem as PrinterModel;
                   await Navigation.PopModalAsync();
               });
            }
        }

        public ObservableCollection<PrinterModel> PrintersList { get; private set; }

        async void HandleAction(SelectedItemChangedEventArgs selectedPrinter)
        {
            SelectedPrinter = selectedPrinter.SelectedItem as PrinterModel;
            await Navigation.PopModalAsync();
        }

        PrinterModel _selectedPrinter;
        public PrinterModel SelectedPrinter
        {
            get
            {
                return _selectedPrinter;
            }

            set
            {
                _selectedPrinter = value;
                SelectedPrinterName = _selectedPrinter.PrinterName;
                AppRepository.Instance.SetPrinter(_selectedPrinter);
                RaisePropertyChanged(nameof(SelectedPrinter));
            }
        }

        string _selectedPrinterName;
        public string SelectedPrinterName
        {
            get
            {
                return string.IsNullOrWhiteSpace(_selectedPrinterName) ? "Select Printer" : _selectedPrinterName;
            }
            set
            {
                _selectedPrinterName = value;
                RaisePropertyChanged(nameof(SelectedPrinterName));
            }
        }

        ICommand _printerLabelAction;
        public ICommand PrinterLabelAction
        {
            get
            {
                return new Command(async (obj) =>
                {
                   
                    try
                    {
                        if (SelectedPrinter == null)
                        {
                            await UserDialogs.Instance.AlertAsync(AppResources.SelectPrinterMessage, AppResources.TitleFail, AppResources.OkButtonText);
                            return;
                        }
						UserDialogs.Instance.ShowLoading("Printing...");
                        ReportRequest reportRequest = new ReportRequest()
                        {
                            ReportFile = ReportFile.VehicleLabel,
                            PrinterId = SelectedPrinter.PrinterId,
                            VehicleId = Vehicle.Id,
                            LocationId = Vehicle?.LocationId == null ? Guid.Empty : (Guid)Vehicle?.LocationId, //TODO: double check this id
                        };
                        var printStatus = await PrintService.PrintLabelAsync(reportRequest);
                        UserDialogs.Instance.HideLoading();
                        if (printStatus.StatusCode == HttpStatusCode.OK)
                        {
                            await UserDialogs.Instance.AlertAsync(AppResources.PrintLabelSuccess, AppResources.TitleSuccess, AppResources.OkButtonText);
                        }
                        else
                        {
                            await UserDialogs.Instance.AlertAsync(AppResources.TitlePrintFail, AppResources.TitleFail, AppResources.OkButtonText);
                        }
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                    finally
                    {
                        UserDialogs.Instance.HideLoading();
                    }
                });
            }
        }

        public VehicleService VehicleService
        {
            get;
            set;
        } = new VehicleService(new InspectionWriterClient(), AppRepository.Instance);

        bool _alreadyAlertedUserAboutVehicle = false;
        public bool AlreadyAlertedUserAboutVehicle
        {
            get
            {
                return _alreadyAlertedUserAboutVehicle;
            }

            set
            {
                _alreadyAlertedUserAboutVehicle = value;
                RaisePropertyChanged(nameof(AlreadyAlertedUserAboutVehicle));
            }
        }

        bool _allowNewInspections;
        public bool AllowNewInspections
        {
            get
            {
                return _allowNewInspections && Settings.Current.AppUser.CanCreateInspections;
            }

            set
            {
                _allowNewInspections = value;
                RaisePropertyChanged(nameof(AllowNewInspections));
            }
        }

        public async override Task OnAppearing()
        {
            await base.OnAppearing();

            if (AlreadyAlertedUserAboutVehicle == false)
            {
                UserDialogs.Instance.ShowLoading();
                //Vehicle alerts
                try
                {
                    var vechicleAlerts = await VehicleService?.GetVehicleAlertsAsync(Vehicle);
                    var vinAlerts = await VehicleService?.GetVinAlertsAsync(Vehicle);

                    var alertString = string.Empty;
                    if (vechicleAlerts?.Count() > 0)
                    {
                        alertString += "Vehicle Alert" + Environment.NewLine;
                        foreach (var vechicleAlert in vechicleAlerts)
                        {
                            alertString += $"{vechicleAlert} ";
                        }
                    }

                    if (vinAlerts?.Count() > 0)
                    {
                        alertString += string.IsNullOrWhiteSpace(alertString) ? Environment.NewLine : (Environment.NewLine + Environment.NewLine);
                        foreach (var vinAlert in vinAlerts)
                        {
                            alertString += $"{vinAlert} ";
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(alertString))
                    {
                        UserDialogs.Instance.HideLoading();
                        await UserDialogs.Instance.AlertAsync(alertString, "Alert!", AppResources.OkButtonText);
                        AlreadyAlertedUserAboutVehicle = true;
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    UserDialogs.Instance.HideLoading();
                }
            }
        }

        async void CreateInspectionAction(object obj)
        {
            using (var client = new InspectionWriterClient())
            {
                var vehicleService = new VehicleService(client, _db);
                var availableTypesAndMasters = await vehicleService.GetAvailableInspectionTypesAndMastersAsync(Vehicle);
                List<InspectionMasterRecord> masters = availableTypesAndMasters;
                List<InspectionTypeRecord> types = availableTypesAndMasters;
                var device = DependencyService.Get<IDevice>();
                var currentUser = Settings.Current.AppUser;
                var currentInspection = new LocalInspection()
                {
                    InspectionId = Guid.NewGuid(),
                    AllowEditing = true,
                    Comments = string.Empty,
                    ContextAppVersion = App.AppVersion,
                    ContextiOSVersion = device?.FirmwareVersion,
                    ContextiPadModel = device?.HardwareVersion,
                    ContextLocation = currentUser?.Location,
                    ContextUsername = currentUser?.UserName,
                    ElapsedTime = 0,
                    InspectionColor = Vehicle.Color,
                    InspectionDate = DateTime.Now.ToLocalTime(),
                    InspectionMilesHours = Vehicle.MilesHours,
                    InspectionType = types[0]?.DisplayName ?? "N/A",
                    InspectionTypeId = types[0]?.Id ?? -1,
                    InspectionUser = currentUser?.UserName,
                    InspectionUserId = currentUser.UserAccountId,
                    InspectionValue = Constants.InspectionSource,
                    IsLocalInspection = true,
                    LocationId = currentUser.LocationId,
                    MasterDisplayName = masters[0]?.DisplayName ?? "N/A",
                    MasterId = masters[0]?.MasterId ?? default(Guid),
                    NewColor = Vehicle.Color,
                    NewMileage = Vehicle.MilesHours,
                    NewVehicleModelId = Vehicle.VehicleModelId,
                    NewVIN = Vehicle.Vin,
                    OpenRepair = false,
                    RepairComments = string.Empty,
                    Score = 0,
                    StockNumber = Vehicle.StockNumber,
                    VehicleId = Vehicle.Id
                };

                Debug.WriteLine($"New inspection created for Vehicle: {currentInspection.VehicleId}");

                await _db.AddCurrentObjectAsync(currentInspection);
                await _db.AddCurrentObjectAsync(Constants.CurrentInspectionState, InspectionState.New);

                // Only here to serve as a debug warning incase we have issues with this again....
                var savedInspection = await _db.Table<LocalInspection>().Where(x => x.InspectionId == currentInspection.InspectionId).FirstOrDefaultAsync();
                Debug.WriteLineIf(currentInspection.VehicleId != savedInspection.VehicleId, $"The Vehicle id for the new inspection does not match {currentInspection.VehicleId}. The value saved as {savedInspection.VehicleId}");

                AllowNewInspections = false;

                await Navigation.PushAsync(new InspectionTabbedPage(currentInspection, Vehicle));
            }
        }

        public ICommand CreateInspectionCommand
        {
            get
            {
                return new Command(CreateInspectionAction);
            }
        }
    }
}
