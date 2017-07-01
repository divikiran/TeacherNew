using System;
using System.Collections.Generic;
using NPAInspectionWriter.ViewModels;
using Xamarin.Forms;

namespace NPAInspectionWriter.Views.Pages
{
    public partial class TestPage : ContentPage
    {
        public TestPage()
        {
            BindingContext = new TestViewModel();
            InitializeComponent();
        }
    }
}
