using System;
using System.Diagnostics;
using Java.Net;
using Xamarin.Forms;

namespace luis_beuth_mobile.Views
{
	public partial class FSRLogin : ContentPage
	{
		public FSRLogin()
		{
			InitializeComponent();
		}
		public void Login( object sender, EventArgs e)
		{
			if (String.IsNullOrEmpty(passwordEntry.Text))
			{
				DisplayAlert("Validation Error", "Password is required", "Re-try");
			}
			else {
				SendToBe(passwordEntry.Text);
			   
			}
		}


	    private async void SendToBe(string text)
		{
            var studentId = Application.Current.Properties["studentId"] as string;
		    var matriculationNr = Int32.Parse(studentId);

            var restLogin = new RESTLogin();
		    var response = await restLogin.Validate(matriculationNr, text);

		    if (response)
		    {
		        Navigation.PushAsync(new BarcodeScanner());
		    }
		    else
		    {
                DisplayAlert("Server Error", "Wrong Password", "Re-try");
            }
		}
    }
}
