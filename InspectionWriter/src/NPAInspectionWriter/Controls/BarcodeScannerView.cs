using System;
using System.Diagnostics;
using System.Windows.Input;
using Xamarin.Forms;

namespace NPAInspectionWriter.Controls
{
    public class BarcodeScannerView : View
    {
		public static readonly BindableProperty CommandProperty =
			BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(BarcodeScannerView), null);

		public ICommand Command
		{
			get { return (ICommand)GetValue(CommandProperty); }
			set { SetValue(CommandProperty, value); }
		}

        public Action<string> BarcodeScanned;

        public BarcodeScannerView()
        {
			BarcodeScanned += (string barcode) =>
			{
				Debug.WriteLine(barcode);
                Command.Execute(barcode);
			};
        }
    }
}
