using System;
using System.ComponentModel;
using NPAInspectionWriter.Controls;
using NPAInspectionWriter.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ExtendedSearchBar), typeof(ExtendedSearchBarRenderer))]

namespace NPAInspectionWriter.iOS.Renderers
{
    public class ExtendedSearchBarRenderer : SearchBarRenderer
	{

		protected override void OnElementChanged(ElementChangedEventArgs<SearchBar> e)
		{
			base.OnElementChanged(e);
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (e?.PropertyName == "IsFocused" && Element.IsFocused)
			{
				SetKeyboardType();
			}
		}

		private void SetKeyboardType()
		{
			var keyboard = (Element as ExtendedSearchBar)?.Keyboard;
			Control.AutocapitalizationType = UITextAutocapitalizationType.None;
			Control.AutocorrectionType = UITextAutocorrectionType.No;
			Control.SpellCheckingType = UITextSpellCheckingType.No;

			if (keyboard == Keyboard.Chat)
			{
				Control.AutocapitalizationType = UITextAutocapitalizationType.Sentences;
				Control.AutocorrectionType = UITextAutocorrectionType.Yes;
			}
			else if (keyboard == Keyboard.Email)
			{
				Control.KeyboardType = UIKeyboardType.EmailAddress;
			}
			else if (keyboard == Keyboard.Numeric)
			{
				Control.KeyboardType = UIKeyboardType.NumberPad;
			}
			else if (keyboard == Keyboard.Telephone)
			{
				Control.KeyboardType = UIKeyboardType.PhonePad;
			}
			else if (keyboard == Keyboard.Text)
			{
				Control.AutocapitalizationType = UITextAutocapitalizationType.Sentences;
				Control.AutocorrectionType = UITextAutocorrectionType.Yes;
				Control.SpellCheckingType = UITextSpellCheckingType.Yes;
			}
			else if (keyboard == Keyboard.Url)
			{
				Control.KeyboardType = UIKeyboardType.Url;
			}
			else
			{
				Control.AutocapitalizationType = UITextAutocapitalizationType.Sentences;
				Control.AutocorrectionType = UITextAutocorrectionType.Default;
				Control.SpellCheckingType = UITextSpellCheckingType.Default;
				Control.KeyboardType = UIKeyboardType.Default;
			}
		}

	}
}
