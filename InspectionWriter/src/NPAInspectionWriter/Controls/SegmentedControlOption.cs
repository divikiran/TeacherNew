using System;
using Xamarin.Forms;

namespace NPAInspectionWriter.Controls
{
	public class SegmentedControlOption : View
	{
		public static readonly BindableProperty TextProperty =
			BindableProperty.Create(nameof(Text), typeof(string), typeof(SegmentedControlOption), string.Empty);

		public static readonly BindableProperty IsSelectedProperty =
			BindableProperty.Create(nameof(IsSelected), typeof(bool), typeof(SegmentedControlOption), false);

		public static readonly BindableProperty SegmentWidthProperty =
			BindableProperty.Create(nameof(SegmentWidth), typeof(float), typeof(SegmentedControlOption), 0f);

		public static readonly BindableProperty IsSegmentEnabledProperty =
			BindableProperty.Create(nameof(IsSegmentEnabled), typeof(bool), typeof(SegmentedControlOption), true,
            BindingMode.TwoWay, propertyChanged: OnIsEnabledChanged);

		private static void OnIsEnabledChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var sco = bindable as SegmentedControlOption;
			MessagingCenter.Send(sco, "IsSegmentEnabled");
		}

		public string Text
		{
			get { return (string)GetValue(TextProperty); }
			set { SetValue(TextProperty, value); }
		}

		public bool IsSelected
		{
			get { return (bool)GetValue(IsSelectedProperty); }
			set { SetValue(IsSelectedProperty, value); }
		}

		public float SegmentWidth
		{
			get { return (float)GetValue(SegmentWidthProperty); }
			set { SetValue(SegmentWidthProperty, value); }
		}

		public bool IsSegmentEnabled
		{
			get { return (bool)GetValue(IsSegmentEnabledProperty); }
			set { SetValue(IsSegmentEnabledProperty, value); }
		}
	}
}
