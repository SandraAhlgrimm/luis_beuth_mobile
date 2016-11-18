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
			Content = new StackLayout
			{
				Padding = new Thickness(10, 40, 10, 10),
				Children = {
					new Label { Text = "Passwort" },
					passwordEntry
				}
			};

		}
		public void Login( object sender, EventArgs e)
		{
			Navigation.PushAsync(new ScanView());
		}
	}
}