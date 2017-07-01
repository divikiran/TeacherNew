﻿using System;
using Xamarin.Forms;
using MvvmHelpers;
using NPAInspectionWriter.Models;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using NPAInspectionWriter.Helpers;
using NPAInspectionWriter.Services;
using Acr.UserDialogs;
using NPAInspectionWriter.Logging;
using NPAInspectionWriter.AppData;
using NPAInspectionWriter.Views.Layouts;
using System.Threading;

namespace NPAInspectionWriter.ViewModels
{
    public class LocalInspectionsViewModel : CRWriterBase
    {
        InspectionWriterClient client;
        public LocalInspectionsViewModel()
        {
            AsyncHelpers.RunSync(async () => await LoadInspections());

            MessagingCenter.Subscribe<string>(this, "Upload Inspection", async (obj) =>
            {
                var inspection = Inspections.Where(x => x.InspectionId == new Guid(obj)).FirstOrDefault();
                await ExecuteUploadInspection(inspection);
            });

            MessagingCenter.Subscribe<InspectionUploadStatus>(this, "InspectionUploadPercentUpdate", (status) => {
				UpdateImageUploadStatus(status);
            });

            MessagingCenter.Subscribe<LocalInspection>(this, "Delete Inspection",(obj) =>
			{
                Inspections.Remove(obj);
			});
        }

        async Task<bool> LoadInspections()
        {
#if !DEBUG
            //_inspections = await _db.GetLocalInspections(i => i.IsLocalInspection, includeVehicle: true) as ObservableRangeCollection<LocalInspection>;
            //_inspections = _inspections != null ? _inspections : new ObservableRangeCollection<LocalInspection>();
#else
            var model = new LocalInspection()
            {
                AllowEditing = true,
                Score = 72,
                StockNumber = "282-10069587",
                LastSync = DateTime.Now,
                LocationId = new Guid("d7935c31-d157-4131-abec-91fdd555c627"),
                InspectionUser = "inspection",
                InspectionType = "Pre-Inspection",
                InspectionDate = DateTime.Today,
                InspectionId = new Guid("00000000-0000-0000-0000-000000000001"),
                FailedUpload = true,
                IsLocalInspection = true,
                VehicleId = new Guid("d7935c31-d157-4131-abec-91fdd555c627"),
                Master = new LocalInspectionMaster() { Categories = new System.Collections.Generic.List<LocalInspectionCategory>() }            
            };
			var model2 = new LocalInspection()
			{
				AllowEditing = true,
				Score = 72,
				StockNumber = "282-10069587",
				LastSync = DateTime.Now,
				LocationId = new Guid("d7935c31-d157-4131-abec-91fdd555c627"),
				InspectionUser = "inspection",
				InspectionType = "Pre-Inspection",
				InspectionDate = DateTime.Today,
                InspectionId = new Guid("00000000-0000-0000-0000-000000000002"),
				FailedUpload = true,
				VehicleId = new Guid("d7935c31-d157-4131-abec-91fdd555c627"),
				Master = new LocalInspectionMaster() { Categories = new System.Collections.Generic.List<LocalInspectionCategory>() }
			};
			var model3 = new LocalInspection()
			{
				AllowEditing = true,
				Score = 72,
				StockNumber = "282-10069587",
				LastSync = DateTime.Now,
				LocationId = new Guid("d7935c31-d157-4131-abec-91fdd555c627"),
				InspectionUser = "inspection",
				InspectionType = "Pre-Inspection",
				InspectionDate = DateTime.Today,
                InspectionId = new Guid("00000000-0000-0000-0000-000000000003"),
				FailedUpload = true,
				VehicleId = new Guid("d7935c31-d157-4131-abec-91fdd555c627"),
				Master = new LocalInspectionMaster() { Categories = new System.Collections.Generic.List<LocalInspectionCategory>() }
			};
			var model4 = new LocalInspection()
			{
				AllowEditing = true,
				Score = 72,
				StockNumber = "282-10069587",
				LastSync = DateTime.Now,
				LocationId = new Guid("d7935c31-d157-4131-abec-91fdd555c627"),
				InspectionUser = "inspection",
				InspectionType = "Pre-Inspection",
				InspectionDate = DateTime.Today,
                InspectionId = new Guid("00000000-0000-0000-0000-000000000004"),
				FailedUpload = true,
				VehicleId = new Guid("d7935c31-d157-4131-abec-91fdd555c627"),
				Master = new LocalInspectionMaster() { Categories = new System.Collections.Generic.List<LocalInspectionCategory>() }
			};
			var model5 = new LocalInspection()
			{
				AllowEditing = true,
				Score = 72,
				StockNumber = "282-10069587",
				LastSync = DateTime.Now,
				LocationId = new Guid("d7935c31-d157-4131-abec-91fdd555c627"),
				InspectionUser = "inspection",
				InspectionType = "Pre-Inspection",
                InspectionId = new Guid("00000000-0000-0000-0000-000000000005"),
				InspectionDate = DateTime.Today,
				FailedUpload = true,
				VehicleId = new Guid("d7935c31-d157-4131-abec-91fdd555c627"),
				Master = new LocalInspectionMaster() { Categories = new System.Collections.Generic.List<LocalInspectionCategory>() }
			};
            model.Master.Categories.Add(new LocalInspectionCategory(){Required = true});
            Inspections.Add(model);
            Inspections.Add(model2);
            Inspections.Add(model3);
            model.FailedUpload = false;
            Inspections.Add(model4);
            Inspections.Add(model5);
#endif
            return true;
        }

		public ICommand UploadInspectionCommand
		{
			get
			{
				return new Command<LocalInspection>((obj) =>
				{
					Debug.WriteLine("Upload Inspection");
                    MessagingCenter.Send<string>(obj.InspectionId.ToString(), "Upload Inspection");
				});
			}
		}

        public ObservableRangeCollection<LocalInspection> UploadingInspections { get; set; } = new ObservableRangeCollection<LocalInspection>();

        public bool IncompleteExist
        {
            get { return Inspections.Where(x => x.IsPendingUpload == false).Count() > 0; }
        }

        public bool PendingExist
        {
            get { return Inspections.Where(x => x.IsPendingUpload).Count() > 0; }

        }

        public bool FailedExist
        {
            get { return Inspections.Where(x => x.FailedUpload).Count() > 0; }
        }

        public bool InspectionsExist
        {
            get { return Inspections.Count() > 0; }
        }

        private async Task ExecuteUploadInspection(LocalInspection inspection)
        {
            try
            {
                UserDialogs.Instance.ShowLoading("Uploading Inspection");
                if (inspection == null) return;
                if (totalPending >= 1)
                {
                    UploadingInspections.Add(inspection);
                    UpdateImageUploadStatus(new InspectionUploadStatus(false, 0, inspection.Pictures.Count));
                    totalPending++;
                    return;
                }
                ShowProgressIndicator = true;
				totalPending++;
				UpdateImageUploadStatus(new InspectionUploadStatus(false, 0, inspection.Pictures.Count));
                await ExecuteUpload(inspection);
            }
            catch (Exception e)
            {
                StaticLogger.ExceptionLogger(e);
                await UserDialogs.Instance.AlertAsync(AppResources.WhoopsAlertTitle, e.Message, AppResources.OkButtonText);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        async Task ExecuteUpload(LocalInspection inspection)
        {
#if !DEBUG
            try
            {
                using (client = new InspectionWriterClient())
                {
                    var inspectionsService = new InspectionsService(client, AppRepository.Instance, DependencyService.Get<IApplicationFileSystem>());
                    await inspectionsService.SaveNewInspectionAsync(inspection);
                }
            }
            catch (OperationCanceledException e)
            {
                UploadingInspections.Remove(inspection);
                inspection.Uploading = false;
                Debug.WriteLine(e);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex);
			}
#else
            await Task.Delay(2000); // simulate upload time
			MessagingCenter.Send(new InspectionUploadStatus(true, 0, 5), "InspectionUploadPercentUpdate");
			await Task.Delay(2000); // simulate upload time
            MessagingCenter.Send(new InspectionUploadStatus(true, 1, 5), "InspectionUploadPercentUpdate");
            await Task.Delay(2000); // simulate upload time
            MessagingCenter.Send(new InspectionUploadStatus(true, 2, 5), "InspectionUploadPercentUpdate");
            await Task.Delay(2000); // simulate upload time
            MessagingCenter.Send(new InspectionUploadStatus(true, 3, 5), "InspectionUploadPercentUpdate");
            await Task.Delay(2000); // simulate upload time
            MessagingCenter.Send(new InspectionUploadStatus(true, 4, 5), "InspectionUploadPercentUpdate");
            await Task.Delay(2000); // simulate upload time
            MessagingCenter.Send(new InspectionUploadStatus(true, 5, 5), "InspectionUploadPercentUpdate");
#endif
            currentCount++;
            if(UploadingInspections.Count > 0){
                inspection.IsLocalInspection = false;
                var inspectionToUpload = UploadingInspections.FirstOrDefault();
                UploadingInspections.Remove(inspectionToUpload);
                await ExecuteUpload(inspectionToUpload);
            }
	        else{
                ShowProgressIndicator = false;
                totalPending = 0;
                currentCount = 0;
	        }
        }

        string _listSortMethod = "All";
        public string ListSortMethod 
        {
            get { return _listSortMethod; }
            set 
            {
                SetProperty(ref(_listSortMethod), value);
                RaisePropertyChanged("Inspections");    
            }
        }
        ObservableRangeCollection<LocalInspection> _inspections = new ObservableRangeCollection<LocalInspection>();
        public ObservableRangeCollection<LocalInspection> Inspections{
            get 
            { 
                switch(ListSortMethod)
                {
                    case "Failed Upload":
                        return new ObservableRangeCollection<LocalInspection>(_inspections.Where(x => x.FailedUpload)); ;
                    case "Pending Upload":
	                    return new ObservableRangeCollection<LocalInspection>(_inspections.Where(x => x.IsPendingUpload)); ;
                    case "Incomplete":
                        return new ObservableRangeCollection<LocalInspection>(_inspections.Where(x => x.IsPendingUpload == false)); ;
                    default:
                        return _inspections;
                }
            }
        }

        public override string Title => "Local Inspections";

        public override ImageSource Icon => "Default";

		private void UpdateImageUploadStatus(InspectionUploadStatus status)
		{
            var currentUpload = ((1.0 / totalPending) * status.PercentUploaded);
            var finishedUpload = (double)((double)currentCount/(double)totalPending);
            UploadProgress = currentUpload + finishedUpload;
		}

		private double _uploadProgress;
		public double UploadProgress
		{
			get { return _uploadProgress; }
			set { SetProperty(ref _uploadProgress, value); }
		}

        private bool _showProgressIndicator;
		public bool ShowProgressIndicator
		{
			get { return _showProgressIndicator; }
			set { SetProperty(ref _showProgressIndicator, value); }
		}

        private int totalPending = 0;
        private int currentCount = 0;
    }

	public class InspectionUploadStatus
	{
		public InspectionUploadStatus(bool inspectionUploaded, int imagesUploaded = 0, int totalImages = 0)
		{
			InspectionUploaded = inspectionUploaded;
			ImagesUploaded = imagesUploaded;
			TotalImages = totalImages;
		}

		public bool InspectionUploaded { get; }

		public int ImagesUploaded { get; }

		private int TotalImages { get; }

		public double PercentUploaded
		{
			get
			{
				if (!InspectionUploaded) return 0;

				return (ImagesUploaded + 1.0) / (TotalImages + 1.0);
			}
		}
	}
}
