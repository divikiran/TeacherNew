using System;
using NPAInspectionWriter.Models;
using Xamarin.Forms;
using NPAInspectionWriter.Views.Layouts;

namespace NPAInspectionWriter.Controls
{
	public class LocalInspectionCellSelector : DataTemplateSelector
	{
		protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
		{
			var inspection = item as LocalInspection;

			if (inspection.IsPendingUpload)
				return PendingUpload ?? Default;
			else if (inspection.FailedUpload)
				return FailedUpload ?? Default;

			return Default;
		}

        public DataTemplate PendingUpload { get; set; }

		public DataTemplate FailedUpload { get; set; }

		public DataTemplate Default { get; set; }
	}
}
