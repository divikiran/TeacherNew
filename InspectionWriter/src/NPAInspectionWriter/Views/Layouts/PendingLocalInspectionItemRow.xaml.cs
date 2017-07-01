﻿using System;
using System.Windows.Input;
using NPAInspectionWriter.Models;
using Xamarin.Forms;
using System.Diagnostics;

namespace NPAInspectionWriter.Views.Layouts
{
    public partial class PendingLocalInspectionItemRow : BaseLocalInspectionItemRow
    {

		public ICommand UploadInspectionCommand
		{
			get
			{
                return new Command(() =>
				{
					Debug.WriteLine("Upload Inspection");
                    MessagingCenter.Send<string>(InspectionId.ToString(), "Upload Inspection");
				});
			}
		}

        public PendingLocalInspectionItemRow()
        {
            InitializeComponent();
            UploadButton.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = UploadInspectionCommand
		    });
        }

		public void OnDelete(object sender, EventArgs e)
		{
            // Send message to delete this object from list
			var mi = ((MenuItem)sender);
            var inspection = mi.CommandParameter as LocalInspection;
            MessagingCenter.Send<LocalInspection>(inspection, "Delete Inspection");
		}
    }
}
