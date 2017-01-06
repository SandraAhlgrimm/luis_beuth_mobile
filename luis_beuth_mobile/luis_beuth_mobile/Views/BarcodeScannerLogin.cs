using System;
using System.Diagnostics;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;
using System.Threading.Tasks;

namespace luis_beuth_mobile
{
	public class BarcodeScannerLogin : ContentPage
	{
		ZXingScannerView zxing;
		ZXingDefaultOverlay overlay;

        Boolean called = false;

		public BarcodeScannerLogin()
		{

            Debug.WriteLine("LOG: BARCODESCANNERLOGIN");

            zxing = new ZXingScannerView
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand
			};
			zxing.OnScanResult += (result) =>
				Device.BeginInvokeOnMainThread(async () =>
				{
                    Debug.WriteLine("LOG: CALL");
					// Stop analysis until we navigate away so we don't keep reading barcodes
					zxing.IsAnalyzing = false;

                    // Validate result
                    var text = result.Text.Substring(1, result.Text.Length - 1);
                    var length = text.Length;
                    var first = result.Text[0];
                    var last = text[length - 1];

                    if ( length == 7 && first.Equals('S') && last.Equals('0'))
                    {
                        // Saving scanned barcode as student id
                        if (!Application.Current.Properties.ContainsKey("studentId"))
                        {
                            Application.Current.Properties["studentId"] = text;

                            await PostStudent(Int32.Parse(text), (string)Application.Current.Properties["name"]);

                            MessagingCenter.Send(result.Text, "LoginSuccessful");
                            await DisplayAlert("Login", "Erfolgreich mit " + result.Text + " eingeloggt!", "OK");

                            Debug.WriteLine(Navigation.NavigationStack);
                            Debug.WriteLine(Navigation.NavigationStack.Count);

                            await Navigation.PopModalAsync();
                        }
                       
                    } else
                    {
                        await DisplayAlert("Login", "Ungültigen Code eingescannt!", "Nochmal einscannen");
                    }
      
				});

			overlay = new ZXingDefaultOverlay
			{
				TopText = "Hold your phone up to the barcode",
				BottomText = "Scanning will happen automatically",
				ShowFlashButton = zxing.HasTorch,
			};
			overlay.FlashButtonClicked += (sender, e) =>
			{
				zxing.IsTorchOn = !zxing.IsTorchOn;
			};
			var grid = new Grid
			{
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand,
			};
			grid.Children.Add(zxing);
			grid.Children.Add(overlay);

			// The root page of your application
			Content = grid;
		}

		protected async override void OnAppearing()
		{
			base.OnAppearing();
            await CheckForCameraPermissions();

            if (!called)
            {
                called = true;
                await Navigation.PushModalAsync(new NavigationPage(new Views.StudentLogin()));
            }
                            
        }

		protected override void OnDisappearing()
		{
			zxing.IsScanning = false;
			base.OnDisappearing();
		}

		private async Task CheckForCameraPermissions()
		{
			try
			{
				var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
				if (status != PermissionStatus.Granted)
				{
					if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Camera))
					{
						await DisplayAlert("Kamera Zugriff", "Zum Scannen von Barcodes wird Zugriff auf die Kamera benötigt.", "OK");
					}

					var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Camera });
					status = results[Permission.Camera];
				}

				if (status == PermissionStatus.Granted)
				{
					zxing.IsScanning = true;
				}
				else if (status != PermissionStatus.Unknown)
				{
					await DisplayAlert("Kamera Zugriff", "Kann keinen Barcode Scannen ohne Zugriff auf die Kamera.", "OK");
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Error: " + ex.Message);
				//LabelGeolocation.Text = "Error: " + ex;
			}
		}

        private async Task PostStudent(int id, string name)
        {
            Debug.WriteLine("LOG: DEBUG");
            Debug.WriteLine(id + " : " + name);
            var signInClient = new RESTSTudentSignUp();
            await signInClient.addStudent(name, id);
        }

        protected override bool OnBackButtonPressed()
        {
            return (!Application.Current.Properties.ContainsKey("studentId"));
        }
    }
}

