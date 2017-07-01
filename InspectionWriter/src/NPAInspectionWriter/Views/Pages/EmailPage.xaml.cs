using System;
using System.Collections.Generic;

using Xamarin.Forms;
using NPAInspectionWriter.ViewModels;

namespace NPAInspectionWriter.Views.Pages
{
    public partial class EmailPage : ContentPage
    {
        EmailViewModel vm;
        public EmailPage()
        {
            BindingContext = vm = new EmailViewModel();
            InitializeComponent();
        }
    }
}
