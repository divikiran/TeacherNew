using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;
using System.Diagnostics;

namespace NPAInspectionWriter.Controls
{
	[ContentProperty(nameof(Children))]
	public class SegmentedControl : View, IViewContainer<SegmentedControlOption>
	{
		public static readonly BindableProperty OptionsProperty =
			BindableProperty.Create(nameof(Options), typeof(IEnumerable), typeof(SegmentedControl), new List<string>());

		public static readonly BindableProperty SelectedValueProperty =
			BindableProperty.Create(nameof(SelectedValue), typeof(string), typeof(SegmentedControl), string.Empty, BindingMode.TwoWay);

		public static readonly BindableProperty CommandProperty =
			BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(SegmentedControl), null);

		public static readonly BindableProperty SegmentWidthProperty =
			BindableProperty.Create(nameof(SegmentWidth), typeof(float), typeof(SegmentedControl), 0f);

		public SegmentedControl()
		{
			Children = new List<SegmentedControlOption>();
			OnSelectedValue += (s, e) =>
			{
				if (Command != null && Command.CanExecute(e.SelectedItem))
					Command.Execute(e.SelectedItem);
			};
		}

		public event ValueChangedEventHandler ValueChanged;

		public delegate void ValueChangedEventHandler(object sender, EventArgs e);

		public IEnumerable Options
		{
			get { return (IEnumerable)GetValue(OptionsProperty); }
			set
			{
				SetValue(OptionsProperty, value);
				var children = new List<SegmentedControlOption>();
				var isDictionary = value is Dictionary<object, bool>;
				foreach (var item in value)
				{
					SegmentedControlOption child = new SegmentedControlOption();
					if (isDictionary)
					{
						var pair = (KeyValuePair<object, bool>)item;
						child.Text = $"{pair.Key}";
						child.IsSegmentEnabled = pair.Value;
					}
					else
					{
						child.Text = $"{item}";
					}

					if (SegmentWidth > 0)
						child.SegmentWidth = SegmentWidth;

					children.Add(child);
				}

				Children = children;
				OnPropertyChanged(nameof(Children));
			}
		}

		public IList<SegmentedControlOption> Children 
        { 
            get; 
            set; 
        }

		public object SelectedValue
		{
			get { return GetValue(SelectedValueProperty); }
			set
			{
				SetValue(SelectedValueProperty, value);
				ValueChanged?.Invoke(this, EventArgs.Empty);
			}
		}

		public ICommand Command
		{
			get { return (ICommand)GetValue(CommandProperty); }
			set { SetValue(CommandProperty, value); }
		}

		public float SegmentWidth
		{
			get { return (float)GetValue(SegmentWidthProperty); }
			set { SetValue(SegmentWidthProperty, value); }
		}

		public event EventHandler<SelectedItemChangedEventArgs> OnSelectedValue;

		public void SendOnSelectedValue(SelectedItemChangedEventArgs args)
		{
			SelectedValue = args.SelectedItem;
			OnSelectedValue?.Invoke(this, args);
            Debug.WriteLine($"Sending Selected Value: {args.SelectedItem}");
		}
	}
}
