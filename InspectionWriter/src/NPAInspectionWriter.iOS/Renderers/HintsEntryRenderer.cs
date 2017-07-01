//using NPAInspectionWriter.Controls;
//using NPAInspectionWriter.Helpers;
//using NPAInspectionWriter.iOS.Renderers;
//using Xamarin.Forms;
//using Xamarin.Forms.Platform.iOS;

//[assembly: ExportRenderer( typeof( HintsEntry ), typeof( HintsEntryRenderer ) )]
//namespace NPAInspectionWriter.iOS.Renderers
//{
//    public class HintsEntryRenderer : EntryRenderer
//    {
//        protected override void OnElementChanged( ElementChangedEventArgs<Entry> e )
//        {
//            base.OnElementChanged( e );
//            if( Control != null )
//            {
//                Control.SpellCheckingType = UIKit.UITextSpellCheckingType.Yes;
//                Control.AutocorrectionType = UIKit.UITextAutocorrectionType.No;
//                Control.AutocapitalizationType = UIKit.UITextAutocapitalizationType.AllCharacters;

//                var hintType = ( Element as HintsEntry ).HintType;
//                if( hintType == KeyboardHintType.None ) return;

//                var hints = KeyboardHints.GetHints( hintType );


//            }

//        }
//    }
//}
