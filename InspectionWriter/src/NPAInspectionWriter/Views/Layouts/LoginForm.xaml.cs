using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;

namespace NPAInspectionWriter.Views
{
    public partial class LoginForm : ContentView
    {
        public LoginForm()
        {
            InitializeComponent();
        }

		#region Bindable Properties

		public static readonly BindableProperty AppNameProperty =
			BindableProperty.Create(nameof(AppName), typeof(string), typeof(LoginForm), defaultValue: string.Empty, defaultBindingMode: BindingMode.TwoWay);

		public static readonly BindableProperty AppVersionProperty =
			BindableProperty.Create(nameof(AppVersion), typeof(string), typeof(LoginForm), defaultValue: string.Empty, defaultBindingMode: BindingMode.TwoWay);

		public static readonly BindableProperty UserNameProperty =
			BindableProperty.Create(nameof(UserName), typeof(string), typeof(LoginForm), defaultValue: string.Empty, defaultBindingMode: BindingMode.TwoWay);

		public static readonly BindableProperty PasswordProperty =
			BindableProperty.Create(nameof(Password), typeof(string), typeof(LoginForm), defaultValue: string.Empty, defaultBindingMode: BindingMode.TwoWay);

		public static readonly BindableProperty LoginCommandProperty =
			BindableProperty.Create(nameof(LoginCommand), typeof(ICommand), typeof(LoginForm), defaultValue: null);

		public static readonly BindableProperty LoginTextProperty =
			BindableProperty.Create(nameof(LoginText), typeof(string), typeof(LoginForm), defaultValue: "Login", defaultBindingMode: BindingMode.TwoWay);

		#endregion

		#region Properties

		public string AppName
		{
			get { return (string)GetValue(AppNameProperty); }
			set { SetValue(AppNameProperty, value); }
		}

		public string AppVersion
		{
			get { return (string)GetValue(AppVersionProperty); }
			set { SetValue(AppVersionProperty, value); }
		}

		public string UserName
		{
			get { return (string)GetValue(UserNameProperty); }
			set { SetValue(UserNameProperty, value); }
		}

		public string Password
		{
			get { return (string)GetValue(PasswordProperty); }
			set { SetValue(PasswordProperty, value); }
		}

		public ICommand LoginCommand
		{
			get { return (ICommand)GetValue(LoginCommandProperty); }
			set { SetValue(LoginCommandProperty, value); }
		}

		public string LoginText
		{
			get { return (string)GetValue(LoginTextProperty); }
			set { SetValue(LoginTextProperty, value); }
		}

		#endregion
	}
}
