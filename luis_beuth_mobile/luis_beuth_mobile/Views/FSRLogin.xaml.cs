using System;
using Xamarin.Forms;

namespace luis_beuth_mobile.Views
{
	public partial class FSRLogin : ContentPage
	{
		public FSRLogin()
		{
			InitializeComponent();

			passwordEntry = new Entry { Text = "" };

		}
		public void Login( object sender, EventArgs e)
		{
			if (String.IsNullOrEmpty(passwordEntry.Text))
			{
				DisplayAlert("Validation Error", "Username and Password are required", "Re-try");
			}
			else {
				sendToBE(passwordEntry.Text );
			}
			Navigation.PushAsync(new ScanView());
		}


		void sendToBE(string text)
		{
			throw new NotImplementedException();
		}
}
}