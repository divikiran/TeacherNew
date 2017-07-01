using System;
using System.ComponentModel;
using System.Linq;
using NPAInspectionWriter.Controls;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using NPAInspectionWriter.iOS.Extensions;
using NPAInspectionWriter.Extensions;
using NPAInspectionWriter.iOS.Renderers;

[assembly: ExportRenderer(typeof(SegmentedControl), typeof(SegmentedControlRenderer))]
namespace NPAInspectionWriter.iOS.Renderers
{
	public class SegmentedControlRenderer : ViewRenderer<SegmentedControl, UISegmentedControl>
	{
		public event PropertyChangedEventHandler PropertyChanged;

		public SegmentedControlRenderer() : base()
		{
			MessagingCenter.Subscribe<SegmentedControlOption>(this, "IsSegmentEnabled", HandleOptionIsEnabledChanged);
		}

		private void HandleOptionIsEnabledChanged(SegmentedControlOption segmentOption)
		{
			if (Control == null) return;

			for (int i = 0; i < Element.Children.Count; i++)
			{
				if (Element.Children[i].Text == segmentOption.Text)
					Control.SetEnabled(Element.Children[i].IsSegmentEnabled, i);
			}
		}

		object SelectedValue = string.Empty;

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (e.PropertyName == nameof(SegmentedControl.Options) || e.PropertyName == nameof(SegmentedControl.Children))
			{
				Control.RemoveAllSegments();
				for (int i = 0; i < Element.Options.NullableCount(); i++)
				{
					var child = Element.Children[i];
					Control.InsertSegment(child.Text, i, false);
					Control.SetEnabled(child.IsSegmentEnabled, i);

					if (child.SegmentWidth > 0)
						Control.SetWidth(child.SegmentWidth, i);
				}
			}
		}

		/// <summary>
		/// Sets the Native Segmented Control.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnElementChanged(ElementChangedEventArgs<SegmentedControl> e)
		{
			base.OnElementChanged(e);

			if (e.NewElement == null) return;

			SetNativeControl(CreateSegmentedControlFromElement(e.NewElement));
		}

		UISegmentedControl CreateSegmentedControlFromElement(SegmentedControl element)
		{
			var segmentedControl = new UISegmentedControl();

			for (var i = 0; i < element.Children.Count; i++)
			{
				var child = element.Children[i];
				segmentedControl.InsertSegment(child.Text, i, false);
				segmentedControl.SetEnabled(child.IsSegmentEnabled, i);

				if (child.SegmentWidth > 0)
					segmentedControl.SetWidth(child.SegmentWidth, i);
			}

			segmentedControl.ValueChanged += (sender, eventArgs) =>
			{
				var newVal = segmentedControl.TitleAt(segmentedControl.SelectedSegment);
				this.SelectedValue = element.SelectedValue = newVal;

				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(segmentedControl.SelectedSegment)));
			};

			int selectedIndex = 0;
			SegmentedControlOption selectedOption = null;

			if (!string.IsNullOrWhiteSpace($"{element.SelectedValue}"))
				selectedOption = element.Children.FirstOrDefault(item => item.Text == (string)element.SelectedValue);
			else
				selectedOption = element.Children.FirstOrDefault(item => item.IsSelected == true);

			if (selectedOption != null)
			{
				selectedIndex = element.Children.IndexOf(selectedOption);
				SelectedValue = selectedOption.Text;
			}

			if (selectedIndex >= 0)
			{
				// Set the native control to the index of the Selected Item from the Forms Control
				segmentedControl.SelectedSegment = selectedIndex;
			}

			segmentedControl.ValueChanged += SegmentedControl_ValueChanged;

			return segmentedControl;
		}

		private void SegmentedControl_ValueChanged(object sender, System.EventArgs e)
		{
            var item = Element.Children[(int)Control.SelectedSegment].Text;

			Logging.StaticLogger.Log("Executing SegmentedControl_ValueChanged");
			Element.SendOnSelectedValue(new SelectedItemChangedEventArgs(item));
		}

		protected override void Dispose(bool disposing)
		{
			MessagingCenter.Unsubscribe<SegmentedControlOption>(this, "IsSegmentEnabled");
			base.Dispose(disposing);
		}
	}
}
