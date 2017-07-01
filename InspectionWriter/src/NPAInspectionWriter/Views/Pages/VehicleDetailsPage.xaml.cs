﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using NPAInspectionWriter.Models;
using NPAInspectionWriter.Services;
using NPAInspectionWriter.ViewModels;
using Xamarin.Forms;
using System.Linq;

namespace NPAInspectionWriter.Views.Pages
{
    public partial class VehicleDetailsPage : NPABasePage
    {
        VehicleDetailsViewModel vm;
        void InspectionSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            if (vm == null)
                vm = this.BindingContext as VehicleDetailsViewModel;
            var inspection = (e.SelectedItem as InspectionDisplayList).inspection;
            vm.Navigation.PushAsync(new InspectionTabbedPage(inspection, vm.Vehicle));
        }

        public VehicleDetailsPage()
        {
            vm = this.BindingContext as VehicleDetailsViewModel;
            InitializeComponent();
        }
    }
}