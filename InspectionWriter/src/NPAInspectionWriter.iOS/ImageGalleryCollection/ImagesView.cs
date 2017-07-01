using System;
using NPAInspectionWriter.iOS;
using NPAInspectionWriter.Views.Pages;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Acr.UserDialogs;
using NPAInspectionWriter.Services;
using NPAInspectionWriter.Models;
using System.Linq;
using NPAInspectionWriter.Helpers;
using System.Threading.Tasks;
using System.Collections.Generic;
using Foundation;
using System.Net.Http;
using NPAInspectionWriter.ViewModels;
using System.Diagnostics;

[assembly: ExportRenderer(typeof(ImageGalleryPage), typeof(ImagesView))]
namespace NPAInspectionWriter.iOS
{
    public partial class ImagesView : PageRenderer
    {
        public List<UIImage> ImagesBytes { get; set; }
        public InspectionsService InspectionsService
        {
            get;
            set;
        } = new InspectionsService();

        public ImagesView()
        {
            //MessagingCenter.Subscribe<InspectionTabbedPage, int>(this, "GalleryUpdated", (sender, arg) =>
            //{
            //    if (arg > 0)
            //    {
            //        ImagesUpdated(sender);
            //    };
            //});
        }

        private async void ImagesUpdated(InspectionTabbedPage sender)
        {
			//var currentPage = this.Element as ImageGalleryPage;
			//if (currentPage != null)
			//{
			//    var viewModel = currentPage.ViewModel;
			//    var currentInspection = currentPage?.ParentViewModel?.CurrentInspection;
			//    var imagesForInspection = await InspectionsService.GetInspectionImagesAsync(currentInspection.InspectionId);
			//    AppRepository.Instance.InspectionImages = imagesForInspection;

			//    var baseImageUrl = imagesForInspection.ToList().FirstOrDefault(f => !string.IsNullOrEmpty(f.ImageBaseUrl));
			//    if (baseImageUrl != null)
			//    {                      
                    
			//        ImagesBytes = new List<UIImage>();
			//        InspectionWriterClient client = new InspectionWriterClient();

			//        foreach (var item in imagesForInspection)
			//        {
			//            var formatedUrl = string.Format(baseImageUrl.ImageBaseUrl, item.PictureId);
   //                     Debug.WriteLine("Image url :" + formatedUrl);
			//            var image = await LoadImage(formatedUrl);
			//            ImagesBytes.Add(image);
			//        }
			//    }

			//    ImageGalleryCollectionSource.InspectionUIImages = ImagesBytes;
			//}
		}

        public async override void ViewDidLoad()
        {
            try
            {
                UserDialogs.Instance.ShowLoading("Loading Images...");
                base.ViewDidLoad();

                var currentPage = this.Element as ImageGalleryPage;
                if (currentPage != null)
                {
                    var viewModel = currentPage.ViewModel;
                    var currentInspection = currentPage?.ParentViewModel?.CurrentInspection;
                    var imagesForInspection = await InspectionsService.GetInspectionImagesAsync(currentInspection.InspectionId);
                    AppRepository.Instance.InspectionImages = imagesForInspection;

                    var baseImageUrl = imagesForInspection.ToList().FirstOrDefault(f => !string.IsNullOrEmpty(f.ImageBaseUrl));
                    if (baseImageUrl != null)
                    {
                        ImagesBytes = new List<UIImage>();
                        InspectionWriterClient client = new InspectionWriterClient();

                        foreach (var item in imagesForInspection)
                        {
                            var formatedUrl = string.Format(baseImageUrl.ImageBaseUrl, item.PictureId);
                            Debug.WriteLine("Image url :" + formatedUrl);
                            var image = await LoadImage(formatedUrl);
                            ImagesBytes.Add(image);
                        }
                    }

                    ImageGalleryCollectionSource.InspectionUIImages = ImagesBytes;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }

            // Perform any additional setup after loading the view, typically from a nib.
        }

        public async Task<UIImage> LoadImage(string imageUrl)
        {
            HttpClient httpClient = new HttpClient();

            Task<byte[]> contentsTask = httpClient.GetByteArrayAsync(imageUrl);


            // await! control returns to the caller and the task continues to run on another thread
            var contents = await contentsTask;

            // load from bytes
            return UIImage.LoadFromData(NSData.FromArray(contents));
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }


        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
        }
    }
}

