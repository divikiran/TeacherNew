using System;
using System.Collections.Generic;

using Xamarin.Forms;
using NPAInspectionWriter.ViewModels;
using NPAInspectionWriter.Models;

namespace NPAInspectionWriter.Views.Pages
{
    public partial class InspectionOverviewPage : ContentPage
    {
        InspectionTabbedPage _tabbed;
        public InspectionTabbedPage tabbed
        {
            get { return _tabbed; } 
            set 
            { 
                _tabbed = value;
                BindingContext = _tabbed.BindingContext;
            }
        }
        public InspectionOverviewPage()
        {
            InitializeComponent();
        }
    }
}
