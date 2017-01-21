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

        String studentID = "";
        String examID = "";

        Label scanLabel;

        public BarcodeScanner()
        {
            zxing = new ZXingScannerView
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            if (studentID.Length == 0)
            {
                scanStudentID();
            }

            overlay = new ZXingDefaultOverlay
            {
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

            scanLabel = new Label { Text = "QR-Code eines Studenten scannen", VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.Center, TextColor = Color.White };
            grid.Children.Add(scanLabel, 0, 0);

            Content = grid;
        }

        private void scanStudentID()
        {
            
            zxing.OnScanResult += (result) => Device.BeginInvokeOnMainThread(() =>
            {
                // Stop analysis until we navigate away so we don't keep reading barcodes
                zxing.IsAnalyzing = false;
                if (studentID.Length == 0 && examID.Length == 0)
                {
                    var res = result.Text.Replace(System.Environment.NewLine, "");

                    if (res.Length == 7 && isDigitsOnly(res))
                    {
                        studentID = res;
                        scanExamQR();
                    }
                    else
                    {
                        DisplayAlert("Barcode Scanner", "Bitte einen validen QR-Code eines Studenten einscannen!", "OK");
                    }
                }
      
            });
            
        }

        private void scanExamQR()
        {

            scanLabel.Text = "QR-Code einer Klausur einscannen";
            zxing.OnScanResult += (result) => Device.BeginInvokeOnMainThread(() =>
            {
                // Stop analysis until we navigate away so we don't keep reading barcodes
                zxing.IsAnalyzing = false;

                if (studentID.Length != 0 && examID.Length == 0)
                {
                    var res = result.Text.Replace(System.Environment.NewLine, "");

                    if (res.Length > 7 && !isDigitsOnly(res))
                    {
                        examID = res;
                        sendData();
                    }
                    else
                    {
                        DisplayAlert("Barcode Scanner", "Bitte einen Validen QR-Code für die Klausur einscannen!", "OK");
                    }
                }
            });
            
        }

        private async Task sendData()
        {
            scanLabel.Text = "NICE!";
            Debug.WriteLine("DEBUG_SEND DATA HERE");
            Debug.WriteLine("DEBUG_DATA_StudentID: " + studentID);
            Debug.WriteLine("DEBUG_DATA_Exam: " + examID);

            var signInClient = new RESTRents();
            await signInClient.rentExam(1, 1);
        }

        bool isDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }
            return true;
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

