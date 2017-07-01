using System;
using Xamarin.Forms;
namespace NPAInspectionWriter.Controls
{
    public class ExtendedSearchBar : SearchBar
    {
		public static readonly BindableProperty KeyboardProperty =
			BindableProperty.Create(nameof(Keyboard), typeof(Keyboard), typeof(ExtendedSearchBar), Keyboard.Default);

		public Keyboard Keyboard
		{
			get { return (Keyboard)GetValue(KeyboardProperty); }
			set { SetValue(KeyboardProperty, value); }
		}

		public static readonly BindableProperty MaxLengthProperty =
			BindableProperty.Create(nameof(MaxLength), typeof(int), typeof(ExtendedSearchBar), -1); // unlimited by default

		public int MaxLength
		{
			get { return (int)GetValue(MaxLengthProperty); }
			set { SetValue(MaxLengthProperty, value); }
		}

		public static readonly BindableProperty ForceUppercaseProperty =
			BindableProperty.Create(nameof(ForceUppercase), typeof(bool), typeof(ExtendedSearchBar), false); // allow multi-case by default

		public bool ForceUppercase
		{
			get { return (bool)GetValue(ForceUppercaseProperty); }
			set { SetValue(ForceUppercaseProperty, value); }
		}

		public static readonly BindableProperty ForceSingleSpacingProperty =
			BindableProperty.Create(nameof(ForceSingleSpacing), typeof(bool), typeof(ExtendedSearchBar), false); // allow extra spaces by default

		public bool ForceSingleSpacing
		{
			get { return (bool)GetValue(ForceSingleSpacingProperty); }
			set { SetValue(ForceSingleSpacingProperty, value); }
		}

		public ExtendedSearchBar()
		{
			TextChanged += ValidateTextInput;
		}

		public void ValidateTextInput(object sender, TextChangedEventArgs args)
		{
			var sb = sender as SearchBar; if (sb == null) { return; }
			string val = sb?.Text; if (string.IsNullOrEmpty(val)) { return; }

			if (string.IsNullOrEmpty(val)) { return; }

			// force max length if requested
			if (MaxLength > 0 && val.Length > MaxLength)
			{
				val = val.Remove(val.Length - 1);
			}

			// force uppercase if requested
			if (ForceUppercase)
			{
				val = val.ToUpper();
			}

			// force single spaces if requested
			if (ForceSingleSpacing)
			{
				val = val.Replace("  ", " ");
			}

			sb.Text = val;
		}
    }
}
