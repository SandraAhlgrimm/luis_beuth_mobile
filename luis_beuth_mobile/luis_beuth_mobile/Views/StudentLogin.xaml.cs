using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace luis_beuth_mobile.Views
{
	public partial class StudentLogin : ContentPage
	{
		public StudentLogin()
		{
			InitializeComponent();
		}
		public void Login( object sender, EventArgs e)
		{
            if (String.IsNullOrEmpty(studentName.Text))
			{
				DisplayAlert("Fehler", "Bitte gib deinen Namen ein", "Re-try");
			}
			else {
				SaveName(studentName.Text);
			}
		}

	    private async void SaveName(string text)
		{
            Application.Current.Properties["name"] = text;
            await Navigation.PopModalAsync();
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}