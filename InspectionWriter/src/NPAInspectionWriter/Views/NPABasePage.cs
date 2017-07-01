using System;
using System.Diagnostics;
using NPAInspectionWriter.ViewModels;
using Xamarin.Forms;

namespace NPAInspectionWriter.Views
{
    public class NPABasePage : ContentPage
    {
        public static readonly BindableProperty IsWaitingProperty = BindableProperty.Create(nameof(IsWaiting), typeof(bool), typeof(NPABasePage), false);
		public static readonly BindableProperty ShowLoadingFrameProperty = BindableProperty.Create(nameof(ShowLoadingFrame), typeof(bool), typeof(NPABasePage), false);
		public static readonly BindableProperty ShowLoadingMessageProperty = BindableProperty.Create(nameof(ShowLoadingMessage), typeof(bool), typeof(NPABasePage), false);
		public static readonly BindableProperty ShadeBackgroundProperty = BindableProperty.Create(nameof(ShadeBackground), typeof(bool), typeof(NPABasePage), false);
		public static readonly BindableProperty LoadingMessageProperty = BindableProperty.Create(nameof(LoadingMessage), typeof(string), typeof(NPABasePage), "Loading...");

		public bool IsWaiting
		{
			get
			{
				return (bool)GetValue(IsWaitingProperty);
			}
			set
			{
				SetValue(IsWaitingProperty, value);
				if (value) ShowIndicator();
				else HideIndicator();
#if DEBUG
				Debug.WriteLine($"IsWaiting: {value}");
#endif
			}
		}

		public string LoadingMessage
		{
			get { return (string)GetValue(LoadingMessageProperty); }
			set { SetValue(LoadingMessageProperty, value); }
		}

		public bool ShowLoadingMessage
		{
			get { return (bool)GetValue(ShowLoadingMessageProperty); }
			set { SetValue(ShowLoadingMessageProperty, value); }
		}

		public bool ShowLoadingFrame
		{
			get { return (bool)GetValue(ShowLoadingFrameProperty); }
			set { SetValue(ShowLoadingFrameProperty, value); }
		}

		public bool ShadeBackground
		{
			get { return (bool)GetValue(ShadeBackgroundProperty); }
			set { SetValue(ShadeBackgroundProperty, value); }
		}

		public new View Content
		{
			get { return waitingPageContent.Content; }
			set { waitingPageContent.Content = value; }
		}

		public ActivityIndicator Indicator { get; set; }

		private ContentView waitingPageContent;

		private Grid contentLayout;

		private Frame frameLayout;

		private Color shadedBackgroundColor = Color.Black.MultiplyAlpha(0.2);

		private Color transparentBackgroundColor = Color.Transparent;

		// Background color of White, Opaque is required for Android
		private Color whiteBackgroundColor = Color.FromRgba(255, 255, 255, 1.0);

		public NPABasePage()
        {
			waitingPageContent = new ContentView
			{
				Content = null,
			};

			contentLayout = new Grid
			{
				VerticalOptions = LayoutOptions.Fill,
				HorizontalOptions = LayoutOptions.Fill,
				Padding = new Thickness(0, 0, 0, 0),
				BackgroundColor = ShadeBackground ? shadedBackgroundColor : transparentBackgroundColor,
			};

			contentLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
			contentLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            NavigationPage.SetHasBackButton(this, false);

			base.Content = contentLayout;
        }

        public CRWriterBase BaseViewModel
		{
			get;
			set;
		}
		protected async override void OnAppearing()
		{
			base.OnAppearing();

			if (BindingContext != null && BindingContext is BaseViewModel)
			{
				BaseViewModel = (CRWriterBase)BindingContext;
				BaseViewModel.CurrentPage = this;
				BaseViewModel.Navigation = this.Navigation;
				await BaseViewModel?.OnAppearing();
			}

			if (Indicator == null)
			{
				Indicator = new ActivityIndicator
				{
					Color = Color.Black,
					Scale = 1.5,
					IsEnabled = true,
					VerticalOptions = LayoutOptions.CenterAndExpand,
					HorizontalOptions = LayoutOptions.Center,
				};
			}

           
			if (IsWaiting) ShowIndicator();

			BuildIndicatorFrame();

			contentLayout.Children.Add(waitingPageContent, 0, 0);
			contentLayout.Children.Add(frameLayout, 0, 0);
		}

		private void BuildIndicatorFrame()
		{
			var loadingLabel = new Label
			{
				TextColor = Color.Black,
				Text = LoadingMessage,
				FontSize = Xamarin.Forms.Device.GetNamedSize(NamedSize.Small, typeof(Label))
			};

			var stack = new StackLayout
			{
				Spacing = 15,
				Children =
				{
					Indicator
				}
			};

            if(ShowLoadingMessage)
                stack.Children.Insert(0, loadingLabel);

			frameLayout = new Frame
			{
				BackgroundColor = ShowLoadingFrame ? whiteBackgroundColor : transparentBackgroundColor,
				OutlineColor = ShowLoadingFrame ? Color.Black : transparentBackgroundColor,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center,
				IsVisible = IsWaiting,
				HasShadow = false,
				Content = stack,
			};
		}

		protected async override void OnDisappearing()
		{
			base.OnDisappearing();
			if (BindingContext != null && BindingContext is BaseViewModel)
			{
                BaseViewModel = (CRWriterBase)BindingContext;
				BaseViewModel.CurrentPage = this;
				BaseViewModel.Navigation = this.Navigation;
				await BaseViewModel?.OnDisappearing();
			}
		}

		private void ShowIndicator()
		{
			if (Indicator != null) Indicator.IsRunning = true;

			if (frameLayout != null) frameLayout.IsVisible = true;

			contentLayout.BackgroundColor = ShadeBackground ? shadedBackgroundColor : transparentBackgroundColor;
		}

		private void HideIndicator()
		{
			if (Indicator != null) Indicator.IsRunning = false;

			if (frameLayout != null) frameLayout.IsVisible = false;

			contentLayout.BackgroundColor = transparentBackgroundColor;
		}
    }
}

