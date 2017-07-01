using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Acr.UserDialogs;

namespace NPAInspectionWriter.ViewModels
{
    public class ImageGalleryViewModel : CRWriterBase
    {
        public override string Title => "Default";

        public override ImageSource Icon => "Default";

        public InspectionTabbedPageViewModel ParentViewModel { get; internal set; }

        public async override Task OnAppearing()
        {
            try
            {
                UserDialogs.Instance.ShowLoading("Loading Images...");
                await base.OnAppearing();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }


        }
    }
}
