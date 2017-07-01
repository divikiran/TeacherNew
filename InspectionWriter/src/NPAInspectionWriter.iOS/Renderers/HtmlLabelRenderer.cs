using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Foundation;
using NPAInspectionWriter;
using NPAInspectionWriter.iOS.Renderers;
using NPAInspectionWriter.Views.Layouts;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
//[assembly: ExportRenderer(typeof(HtmlLabel), typeof(HtmlLabelRenderer))]
namespace NPAInspectionWriter.iOS.Renderers
{
	public class HtmlLabelRenderer : LabelRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Label> e)
		{
			base.OnElementChanged(e);

			if (Control != null && Element != null && !string.IsNullOrWhiteSpace(Element.Text))
			{
				//var attr = new NSAttributedStringDocumentAttributes();
				//attr.DocumentType = NSDocumentType.HTML;

				//var htmlText = "<html><head></head><body><div id='htmlLabel'>" + Element.Text + "</div></body></html>";

				//var nsError = new NSError();
				//var myHtmlData = NSData.FromString(htmlText, NSStringEncoding.Unicode);
				Control.Lines = 3;
				//Control.AttributedText = new NSAttributedString(myHtmlData, attr, ref nsError);
			}
		}




        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

			if (e.PropertyName == Label.TextProperty.PropertyName)
            {
                if (Control != null && Element != null && !string.IsNullOrWhiteSpace(Element.Text))
                {
                    //var attr = new NSAttributedStringDocumentAttributes();
                    //var nsError = new NSError();
                    //attr.DocumentType = NSDocumentType.HTML;

                    //var htmlText = "<html><head></head><body><div id='htmlLabel'>" + Element.Text + "</div></body></html>";

                    //var myHtmlData = NSData.FromString(htmlText, NSStringEncoding.Unicode);
                    Control.Lines = 3;
                    //Task.Delay(1000);
                    //Control.AttributedText = new NSAttributedString(myHtmlData, attr, ref nsError);
                }
            }
		}
	}
}
