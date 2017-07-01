using System;
using Xamarin.Forms;
using NPAInspectionWriter.Models;
using Rg.Plugins.Popup.Extensions;
using System.Linq;
using System.Text.RegularExpressions;
using Acr.UserDialogs;
using System.Threading.Tasks;

namespace NPAInspectionWriter.ViewModels
{
    public class EmailViewModel : CRWriterBase
    {
        public System.Windows.Input.ICommand CancelCommand
        {
            get
            {
                return new Command(async() =>
                {
                    await Navigation.PopPopupAsync();
                }

                );
            }
        }

		public System.Windows.Input.ICommand SendEmailCommand
		{
			get
			{
				return new Command(async () =>
				{
                    if(await ValidateFields())
                    {
                        //Send Email
                        EmailMessage = new Message();
                        await UserDialogs.Instance.AlertAsync("Email Sent!", "Success!");
                        await Navigation.PopPopupAsync();

                    }
				}

				);
			}
		}
        async Task<bool> ValidateFields()
        {
            if (string.IsNullOrWhiteSpace(EmailMessage.To)
                || string.IsNullOrWhiteSpace(EmailMessage.From)
                || string.IsNullOrWhiteSpace(EmailMessage.Body)
                || string.IsNullOrWhiteSpace(EmailMessage.Subject))
            {
				await UserDialogs.Instance.AlertAsync("Please check all fields and try again.", "At least one field is empty", "OK");

				return false;
            }
                
            var ToEmails = EmailMessage.To.Replace(" ", string.Empty).Split(',');
            var regex = new Regex(@"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*" 
                                  + "@" + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$");
            foreach (var email in ToEmails)
            {
                if (!regex.Match(email).Success)
                {
                    await UserDialogs.Instance.AlertAsync("Please check the \"To\" field and try again", "Invalid Email Entered", "OK");
                    return false;
                }
            }
            return true;
        }

        Message message = new Message();
        public Message EmailMessage{
            get { return message; }
            set { SetProperty(ref(message), value); }
		}

		public EmailViewModel()
		{
            var user = Helpers.AsyncHelpers.RunSync(async()=>await AppRepository.Instance.GetCurrentUserAsync());
            EmailMessage.From = user.Email;
		}

        public override string Title => "Email";

        public override ImageSource Icon => "Default";
    }
}
