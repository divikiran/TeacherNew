namespace NPAInspectionWriter.Helpers
{
    public interface IFeedbackManager
    {
        bool IsEnabled { get; }
        void ShowFeedbackComposeView();
        void ShowFeedbackComposeViewWithGeneratedScreenshot();
        void ShowFeedbackComposeViewWithPreparedItems( params object[] args );
    }
}
