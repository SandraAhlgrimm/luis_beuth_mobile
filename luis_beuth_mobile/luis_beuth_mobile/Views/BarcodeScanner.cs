using System;
using System.Diagnostics;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;
using System.Threading.Tasks;
using Android.Media;

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

            if (studentID.Length == 0 && examID.Length == 0)
            {
                checkQRCode();
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

            scanLabel = new Label { Text = "QR-Code einer Klausur oder eines Studenten einscannen", VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.Center, TextColor = Color.White };
            grid.Children.Add(scanLabel, 0, 0);

            Content = grid;
        }

        private void checkQRCode()
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
                        if (isExamQR(res))
                        {
                            examID = res;
                            sendReturnData();
                        }
                        else
                        {
                            DisplayAlert("Barcode Scanner", "Bitte einen validen QR-Code einscannen!", "OK");
                        }
                    }
                }
            });   
        }

        private void scanExamQR()
        {
            playSound();
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
                        sendRentData();
                    }
                    else
                    {
                        DisplayAlert("Barcode Scanner", "Bitte einen Validen QR-Code für die Klausur einscannen!", "OK");
                    }
                }
            });
        }

        private async Task sendRentData()
        {
            playSound();
            scanLabel.Text = "Klausur ausgeliehen!";
            Debug.WriteLine("DEBUG_RENT");
            Debug.WriteLine("DEBUG_DATA_StudentID: " + studentID);
            Debug.WriteLine("DEBUG_DATA_Exam: " + parseExamId(examID));

            var rentClient = new RESTRents();
            await rentClient.rentExam(Int32.Parse(studentID), parseExamId(examID));
        }

        private async Task sendReturnData()
        {
            playSound();
            scanLabel.Text = "Klausur zurückgegeben!";
            Debug.WriteLine("DEBUG_RETURN");
            Debug.WriteLine("DEBUG_DATA_ExamID: " + parseExamId(examID));

            await rewriteLabel();
            var rentClient = new RESTRents();
            //await rentClient.returnExam(parseExamId(examID));
        }

        private int parseExamId(String exam)
        {
            return Int32.Parse(exam.Split('_')[0].Split(':')[1]);
        }

        private async Task rewriteLabel()
        {
            
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var running = true;

            while (running)
            {
                Debug.WriteLine("DEBUG_SW: " + sw.ElapsedMilliseconds);
                if (sw.ElapsedMilliseconds > 2500)
                {
                    scanLabel.Text = "Weiteren QR Code einscannen!";
                    running = false;
                    sw.Reset();
                } else
                {
                    scanLabel.Text = "Klausur zurückgegeben!";
                }
            }
        }

        bool isExamQR(string str)
        {
            var strings = str.Split('_');

            if(strings[0].Split(':')[0] != "id")
            {
                return false;
            }
            if (strings[1].Split(':')[0] != "semester")
            {
                return false;
            }
            return true;
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

        public void playSound()
        {
            var duration = 1000;
            var sampleRate = 8000;
            var numSamples = duration * sampleRate;
            var sample = new double[numSamples];
            var freqOfTone = 10000;
            byte[] generatedSnd = new byte[2 * numSamples];

            for (int i = 0; i < numSamples; ++i)
            {
                sample[i] = Math.Sin(2 * Math.PI * i / (sampleRate / freqOfTone));
            }

            int idx = 0;
            foreach (double dVal in sample)
            {
                short val = (short)(dVal * 32767);
                generatedSnd[idx++] = (byte)(val & 0x00ff);
                generatedSnd[idx++] = (byte)((val & 0xff00) >> 8);
            }

            var track = new AudioTrack(global::Android.Media.Stream.Music, sampleRate, ChannelOut.Mono, Encoding.Pcm16bit, numSamples, AudioTrackMode.Static);
            track.Write(generatedSnd, 0, numSamples);
            Debug.WriteLine("BEEEEEP");

            track.Play();
        }
    }
}

