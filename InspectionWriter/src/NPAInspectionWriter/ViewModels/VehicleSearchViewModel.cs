using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using NPAInspectionWriter.Helpers;
using NPAInspectionWriter.Models;
using NPAInspectionWriter.Views.Pages;
using Xamarin.Forms;
using NPAInspectionWriter.Services;
using System.Text.RegularExpressions;
using NPAInspectionWriter.Logging;
using NPAInspectionWriter.AppData;
using NPAInspectionWriter.Controls;
using Rg.Plugins.Popup.Extensions;
using System.Collections.Generic;
using NPAInspectionWriter.Extensions;

namespace NPAInspectionWriter.ViewModels
{
    public class VehicleSearchViewModel : CRWriterBase
    {

        private string _searchQuery = "10077493";
        public string SearchQuery
        {
            get { return _searchQuery; }
            set { SetProperty(ref (_searchQuery), value); }
        }

        public string SearchPlaceholder
        {
            get
            {
                if (SearchByStock)
                    return "Enter the Stock # to search for";
                else
                    return "Enter the VIN # to search for";
            }
        }

        public Keyboard Keyboard
        {
            get
            {
                if (SearchByStock)
                    return Keyboard.Numeric;
                else
                    return Keyboard.Default;
            }
        }

        public int MaxLength
        {
            get
            {
                if (SearchByStock)
                    return 8;
                else
                    return 17;
            }
        }
        public bool SearchByStock = true;

        public ICommand LogoutCommand
        {
            get
            {
                return new Command(() =>
                {
                    Debug.WriteLine("Logging Out");
                    App.HandleExpiredSessionEvent();
                });
            }
        }

        private bool _showScanner;
        public bool ShowScanner
        {
            get { return _showScanner; }
            set { SetProperty(ref (_showScanner), value); }
        }

        public ICommand ImageGalleryCommand
        {
            get;
        }

        public ICommand SearchCommand
        {
            get
            {
                return new Command(async (obj) =>
                {
                    Debug.WriteLine($"Search for {SearchQuery}");

                    if (!IsSearchQueryValid())
                    {
                        string message = SearchByStock ? AppMessages.InvalidStockNumber : AppMessages.InvalidVIN;
                        await UserDialogs.Instance.AlertAsync(AppResources.WhoopsAlertTitle, message, AppResources.OkButtonText);
                        return;
                    }

                    UserDialogs.Instance.ShowLoading("Searching");

                    var search = new VehicleSearchRequest();

                    if (SearchByStock) search.StockNumber = SearchQuery;
                    else search.Vin = SearchQuery;

                    try
                    {
                        var vehicleService = new VehicleService(new InspectionWriterClient(), AppRepository.Instance);

                        var vehicleCollection = await vehicleService.SearchAsync(search);

                        var searchTypeValue = SearchByStock ? SearchType.StockNumber : SearchType.VIN;
                        if (vehicleCollection == null) throw new VehicleNotFoundException(SearchQuery, searchTypeValue);

                        if (vehicleCollection.Count == 1)
                        {
                            var vehicle = vehicleCollection.FirstOrDefault();
                            vehicle.AvailableInspections = new List<Inspection>();
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

                            var vehicleDetailsPage = new VehicleDetailTabbedPage(vehicleCollection.FirstOrDefault());
                            await Navigation.PushAsync(vehicleDetailsPage);
                        }
                        else
                        {
                            var vehicleSearchListPage = new VehicleSearchListPage(vehicleCollection);
                            await Navigation.PushAsync(vehicleSearchListPage);
                        }
                    }
                    catch (ObjectNotFoundException onfe)
                    {
                        //_userDialogs.ShowError( onfe );
                        await UserDialogs.Instance.AlertAsync(AppResources.WhoopsAlertTitle, onfe.Message, AppResources.OkButtonText);
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e);
                        //_userDialogs.Toast( $"{e.GetType()} - {e.Message}", ToastType.Failure );
                        await UserDialogs.Instance.AlertAsync(AppResources.WhoopsAlertTitle, $"{e.GetType()} - {e.Message}", AppResources.OkButtonText);
                    }
                    finally
                    {
                        UserDialogs.Instance.HideLoading();
                    }
                });
            }
        }

        private bool IsSearchQueryValid()
        {
            if (string.IsNullOrWhiteSpace(SearchQuery)) return false;

            if (SearchByStock &&
                Regex.IsMatch(SearchQuery.Trim(), NPAConstants.StockNumberValidationPattern))
                return true;

            if (!SearchByStock &&
                Regex.IsMatch(SearchQuery, NPAConstants.SimpleVinValidationPattern))
                return true;

            return false;
        }

        public ICommand ScanBarcodeCommand
        {
            get
            {
                return new Command(() =>
                {
                    ShowScanner = !ShowScanner;
                });
            }
        }

        public ICommand BarcodeScannedCommand
        {
            get
            {
                return new Command((obj) =>
                {
                    ShowScanner = false;
                    SearchQuery = obj.ToString();
                    SearchCommand.Execute(obj);
                });
            }
        }

        public ICommand SendFeedbackCommand
        {
            get
            {
                return new Command(async () =>
                {
                    var page = new EmailPopupPage();

                    await Navigation.PushPopupAsync(page);
                });
            }
        }

        public ICommand ViewLocalInspectionsCommand
        {
            get
            {
                return new Command(() =>
                {
                    Navigation.PushAsync(new LocalInspectionsPage());
                });
            }
        }

        public ICommand UpdateSearchTypeCommand
        {
            get
            {
                return new Command(() =>
                {
                    SearchByStock = !SearchByStock;
                    RaisePropertyChanged("SearchPlaceholder");
                    RaisePropertyChanged("MaxLength");
                    RaisePropertyChanged("Keyboard");
                });
            }
        }

        public override string Title => "Vehicle Search";

        public override ImageSource Icon => "Default";

        private async Task HandleLoggedInUserSession()
        {
            try
            {
                var db = AppRepository.Instance;
                var user = (db.Query<OfflineUserAuthentication>("SELECT * FROM OfflineUserAuthentication oua " +
                                                                  "INNER JOIN CurrentObject co ON oua.UserAccountId = co.ObjectValue " +
                                                                  "WHERE co.Key = ?", Constants.CurrentUserKey))?.FirstOrDefault();

                var client = new InspectionWriterClient();
                if (user == null)
                {
                    await UserDialogs.Instance.AlertAsync("Please check your information and try again", "Invaid User Information", "OK");
                    return;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }
    }
}