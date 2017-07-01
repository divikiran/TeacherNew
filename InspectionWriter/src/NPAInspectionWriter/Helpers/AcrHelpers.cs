using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
using System.Drawing;

namespace NPAInspectionWriter.Helpers
{
    public static class AcrHelpers
    {
        public static ToastConfig GetToastConfig( string message, ToastType toastType )
        {
            switch( toastType )
            {
                case ToastType.Failure:
                    return new ToastConfig( message )
                    {
                        BackgroundColor = Color.DarkRed,
                        MessageTextColor = Color.White
                    };
                case ToastType.Warning:
                    return new ToastConfig( message )
                    {
                        BackgroundColor = Color.Yellow,
                        MessageTextColor = Color.White
                    };
                default:
                    return new ToastConfig( message )
                    {
                        BackgroundColor = Color.Green,
                        MessageTextColor = Color.White
                    };
            }
        }

        public static ToastConfig GetToastConfig( string message, TimeSpan duration, ToastType toastType )
        {
            switch( toastType )
            {
                case ToastType.Failure:
                    return new ToastConfig( message )
                    {
                        Duration = duration,
                        BackgroundColor = Color.DarkRed,
                        MessageTextColor = Color.White
                    };
                case ToastType.Warning:
                    return new ToastConfig( message )
                    {
                        Duration = duration,
                        BackgroundColor = Color.Yellow,
                        MessageTextColor = Color.White
                    };
                default:
                    return new ToastConfig( message )
                    {
                        Duration = duration,
                        BackgroundColor = Color.Green,
                        MessageTextColor = Color.White
                    };
            }
        }

        public static void ShowError( this IUserDialogs userDialogs, Exception e, int timeoutMillis = 2000 ) =>
            Toast( userDialogs, e.Message, timeoutMillis, ToastType.Failure );

        public static IDisposable Toast( this IUserDialogs userDialogs, string message, ToastType toastType ) =>
            userDialogs.Toast( GetToastConfig( message, toastType ) );

        public static IDisposable Toast( this IUserDialogs userDialogs, string message, int timeoutMillis, ToastType toastType ) =>
            Toast( userDialogs, message, TimeSpan.FromMilliseconds( timeoutMillis ), toastType );

        public static IDisposable Toast( this IUserDialogs userDialogs, string message, TimeSpan duration, ToastType toastType ) =>
            userDialogs.Toast( GetToastConfig( message, duration, toastType ) );
    }
}
