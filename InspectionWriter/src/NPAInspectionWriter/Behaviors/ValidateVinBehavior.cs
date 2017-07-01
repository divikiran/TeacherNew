using System.Text.RegularExpressions;
using NPAInspectionWriter.Helpers;
using Xamarin.Forms;

namespace NPAInspectionWriter.Behaviors
{
    public class ValidateVinBehavior : BehaviorBase<Entry>
    {
        Color backgroundColor = Color.Default;
        Color textColor = Color.Default;
        protected override void OnAttachedTo( Entry bindable )
        {
            base.OnAttachedTo( bindable );
            backgroundColor = AssociatedObject.BackgroundColor;
            textColor = AssociatedObject.TextColor;
            bindable.TextChanged += OnTextChanged;
        }

        protected override void OnDetachingFrom( Entry bindable )
        {
            bindable.TextChanged -= OnTextChanged;
            base.OnDetachingFrom( bindable );
        }

        private async void OnTextChanged( object sender, TextChangedEventArgs args )
        {
            var entry = sender as Entry;

            if( entry != null )
            {
                // color red if the vin is not valid
                if( await VehicleHelpers.IsVinValidAsync( entry.Text ) )
                {
                    entry.BackgroundColor = backgroundColor;
                    entry.TextColor = textColor;
                }
                else
                {
                    entry.BackgroundColor = Color.Red;
                    entry.TextColor = Color.White;
                }
            }
        }
    }
}
