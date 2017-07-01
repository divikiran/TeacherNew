using NPAInspectionWriter.ViewModels;
using Xamarin.Forms;

namespace NPAInspectionWriter.Views
{
    public partial class LoginPage : NPABasePage
    {
        LoginPageViewModel vm;
        public LoginPage()
        {
            BindingContext = vm = new LoginPageViewModel(App.AppInfo);
            vm.Navigation = Navigation;
            InitializeComponent();
        }

        protected override void OnSizeAllocated( double width, double height )
        {
            base.OnSizeAllocated( width, height );

            var imageHeight = height / 10;

            if( imageHeight > 140 ) imageHeight = 140;

            logoImage.HeightRequest = imageHeight;
        }
    }
}
