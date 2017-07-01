using System;
using Rg.Plugins.Popup.Pages;
using NPAInspectionWriter.Controls;
using Xamarin.Forms;
namespace NPAInspectionWriter.Views.Pages
{
    public class CameraModal : ContentPage
    {
        public CameraModal()
        {
            WidthRequest = 750;
            HeightRequest = 1000;
            Content = new CameraView();
        }

    }
}
