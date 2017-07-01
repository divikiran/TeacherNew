using System;
using System.ComponentModel;
using System.Drawing;
using AVFoundation;
using CoreFoundation;
using Foundation;
using NPAInspectionWriter.Controls;
using NPAInspectionWriter.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using System.Windows.Input;
using System.Linq;
using System.Threading.Tasks;

[assembly: ExportRenderer(typeof(BarcodeScannerView), typeof(BarcodeScannerViewRenderer))]
namespace NPAInspectionWriter.iOS.Renderers
{
    public class BarcodeScannerViewRenderer : ViewRenderer<BarcodeScannerView, BarcodeScannerViewRenderer.BarcodeScannerView>
    {
        BarcodeScannerView barcodeScannerView;

		protected override void OnElementChanged(ElementChangedEventArgs<Controls.BarcodeScannerView> e)
		{
			base.OnElementChanged(e);
            if (!UIImagePickerController.IsSourceTypeAvailable(UIImagePickerControllerSourceType.Camera))
                return;
			if (Control == null)
			{
                var action = (e.NewElement as Controls.BarcodeScannerView).BarcodeScanned;
				barcodeScannerView = new BarcodeScannerView(action);
				SetNativeControl(barcodeScannerView);
			}
		}

		public class BarcodeScannerView : UIView, IComponent
		{
			public AVCaptureSession session;
			AVCaptureMetadataOutput metadataOutput;

			Action<string> BarcodeScanned;

            public BarcodeScannerView(Action<string> barcodeAction)
			{
                // Set Action for barcode scanner
				BarcodeScanned = barcodeAction;

                // Capture sesison for barcode
				session = new AVCaptureSession();
				AVCaptureDevice device =
					AVCaptureDevice.GetDefaultDevice(AVMediaType.Video);
				NSError error = null;
				AVCaptureDeviceInput input =
				   AVCaptureDeviceInput.FromDevice(device, out error);

                // Validate Input
				if (input == null)
					Console.WriteLine("Error: " + error);
				else
					session.AddInput(input);

                // Create output delegate for session
				metadataOutput = new AVCaptureMetadataOutput();
				var metadataDelegate = new BarcodeScannedOutputDelegate();
				metadataOutput.SetDelegate(metadataDelegate, DispatchQueue.MainQueue);
                metadataDelegate.barcodeScanned = BarcodeScanned;
				session.AddOutput(metadataOutput);

                // Specify barcode types, currently only need to support ITF
                metadataOutput.MetadataObjectTypes = AVMetadataObjectType.DataMatrixCode;

                // Create Preview Layer, this can be customized further later
				AVCaptureVideoPreviewLayer previewLayer = new AVCaptureVideoPreviewLayer(session);
				previewLayer.Frame = new RectangleF(0, 0, (int)UIScreen.MainScreen.Bounds.Width, 300);
				previewLayer.VideoGravity = AVLayerVideoGravity.ResizeAspectFill;
				Layer.AddSublayer(previewLayer);

                // Start scanning session
				session.StartRunning();
			}

			public ISite Site { get; set; }

			public event EventHandler Disposed;
		}

		public class BarcodeScannedOutputDelegate : AVCaptureMetadataOutputObjectsDelegate
		{
			public Action<string> barcodeScanned;
			string lastBarcode;

			public override void DidOutputMetadataObjects(AVCaptureMetadataOutput captureOutput, AVMetadataObject[] metadataObjects, AVCaptureConnection connection)
			{
				foreach (var m in metadataObjects)
				{
					if (m is AVMetadataMachineReadableCodeObject)
					{
                        if (barcodeScanned == null) return;
                        var code = ((AVMetadataMachineReadableCodeObject)m).StringValue;
                        Console.WriteLine($"Barcode Scanned: {code}");
                        // Check if barcode has already been scanned, if so ignore
                        if (lastBarcode != null && lastBarcode == code)
						{
							return;
						}
						else
						{
							barcodeScanned(code);
                            barcodeScanned = null;
						}

					}
				}
			}

		}
    }
}
