using System;
using System.Diagnostics;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;
using System.Threading.Tasks;

namespace luis_beuth_mobile
{
    public class BarcodeScanner : ContentPage
    {
        ZXingScannerView zxing;
        ZXingDefaultOverlay overlay;

        String studentID;
        String examID;

        public BarcodeScanner()
        {
            zxing = new ZXingScannerView
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            DisplayAlert("Barcode Scanner", "Bitte den QR-Code eines Studenten einscannen", "OK");

            zxing.OnScanResult += (result) =>
                Device.BeginInvokeOnMainThread(async () =>
                {

                    // Stop analysis until we navigate away so we don't keep reading barcodes
                    zxing.IsAnalyzing = false;
                    
                    studentID = result.Text;

                    // Show an alert
                    await DisplayAlert("Barcode Scanner", "Bitte den QR-Code einer Klausur einscannen", "OK");

                    examID = result.Text;

                    // Navigate away
                    //await Navigation.PopAsync();

                    Debug.WriteLine("DEBUG StudentID: " + studentID);

                    Debug.WriteLine("DEBUGTEST examID: " + examID);

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
            /*try
			{
				//zxing.IsScanning = true;
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Error: " + ex.Message);
			}*/
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
    }
}

