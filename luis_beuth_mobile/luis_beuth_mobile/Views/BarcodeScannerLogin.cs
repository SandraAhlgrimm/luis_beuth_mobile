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

		public BarcodeScannerLogin()
		{
            zxing = new ZXingScannerView
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand
			};
			zxing.OnScanResult += (result) =>
				Device.BeginInvokeOnMainThread(async () =>
				{

					// Stop analysis until we navigate away so we don't keep reading barcodes
					zxing.IsAnalyzing = false;

                    
                    // Validate result
                    var text = result.Text;
                    var length = text.Length;
                    var first = text[0];
                    var last = text[length - 1];

                    if ( length == 8 && first.Equals('S') && last.Equals('0'))
                    {
                        // Saving scanned barcode as student id
                        Application.Current.Properties["studentId"] = result.Text;
                        await DisplayAlert("Login", "Erfolgreich mit " + result.Text + " eingeloggt!", "OK");
                        MessagingCenter.Send(result.Text, "LoginSuccessful");

                        await Navigation.PopModalAsync();
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

            await DisplayAlert("Login", "Scanne bitte den Barcode deines Studentenausweises ein", "OK");

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

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}

