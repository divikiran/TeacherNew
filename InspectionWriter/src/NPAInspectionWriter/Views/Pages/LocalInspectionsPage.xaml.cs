﻿using System;
using System.Collections.Generic;

using Xamarin.Forms;
using NPAInspectionWriter.ViewModels;
using System.Linq;
using System.Reflection;
using NPAInspectionWriter.Views.Layouts;

namespace NPAInspectionWriter.Views.Pages
{
    public partial class LocalInspectionsPage : NPABasePage
    {
        LocalInspectionsViewModel vm;
        public LocalInspectionsPage()
        {
			NavigationPage.SetHasBackButton(this, true);
			NavigationPage.SetBackButtonTitle(this, "Back");
            BindingContext = vm = new LocalInspectionsViewModel();
            InitializeComponent();

            //TODO: Jarid: figure out why bindings aren't working for segmented control
            foreach(var x in SearchTypeControl.Children)
            {
                switch(x.Text){
                    case "Pending Upload":
                        x.IsSegmentEnabled = vm.PendingExist;
                        break;
					case "Failed Upload":
                        x.IsSegmentEnabled = vm.FailedExist;
						break;
					case "Incomplete":
                        x.IsSegmentEnabled = vm.IncompleteExist;
						break;
                    default:
                        x.IsSegmentEnabled = vm.InspectionsExist;
                        break;
                }
                MessagingCenter.Send(SearchTypeControl, "IsSegmentEnabled");
            }
        }
    }
}
