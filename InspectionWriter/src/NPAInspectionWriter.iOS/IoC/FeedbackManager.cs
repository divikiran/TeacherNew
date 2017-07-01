using System.Collections.Generic;
using Foundation;
using HockeyApp.iOS;
using NPAInspectionWriter.Helpers;

namespace NPAInspectionWriter.iOS.IoC
{
    public class FeedbackManager : IFeedbackManager
    {
        BITFeedbackManager Manager { get; }

        public FeedbackManager()
        {
            Manager = BITHockeyManager.SharedHockeyManager.FeedbackManager;
        }

        public bool IsEnabled { get { return !BITHockeyManager.SharedHockeyManager.DisableFeedbackManager; } }

        public void ShowFeedbackComposeView()
        {
            if( IsEnabled )
                Manager.ShowFeedbackComposeView();
        }

        public void ShowFeedbackComposeViewWithGeneratedScreenshot()
        {
            if( IsEnabled )
                Manager.ShowFeedbackComposeViewWithGeneratedScreenshot();
        }

        public void ShowFeedbackComposeViewWithPreparedItems( params object[] args )
        {
            if( !IsEnabled ) return;

            var list = new List<NSObject>();
            foreach( var arg in args )
                list.Add( NSObject.FromObject( arg ) );

            Manager.ShowFeedbackComposeViewWithPreparedItems( list.ToArray() );
        }
    }
}
