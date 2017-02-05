using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using luis_beuthluis_beuth_mobile.Models.Data;
using luis_beuth_mobile.Model;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

namespace luis_beuth_mobile.Views
{
    public class BarcodeScanner : ContentPage
    {
        private readonly ZXingScannerView _zxing;

        private string _studentId = "";
        private string _examId = "";

        private readonly Label _scanLabel;

        public BarcodeScanner()
        {
            _zxing = new ZXingScannerView
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            CheckQrCode();

            var overlay = new ZXingDefaultOverlay
            {
                ShowFlashButton = _zxing.HasTorch,
            };
            overlay.FlashButtonClicked += (sender, e) =>
            {
                _zxing.IsTorchOn = !_zxing.IsTorchOn;
            };
            var grid = new Grid
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };
            grid.Children.Add(_zxing);
            grid.Children.Add(overlay);

            _scanLabel = new Label { Text = "QR-Code einer Klausur oder eines Studenten einscannen", VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.Center, TextColor = Color.White };
            grid.Children.Add(_scanLabel, 0, 0);

            Content = grid;
        }

        private async Task<int> GetRentId(int examId)
        {
            var restRents = new RestRents();
            var rentId = await restRents.getRentId(examId);
            return rentId;
        }

        private async Task<Boolean> CheckExamAvailability(int examId)
        {
            var restRents = new RestRents();
            List<Rent> allRents = await restRents.getAllRents();

            foreach (var rent in allRents)
            {
                Debug.WriteLine(rent.ExamId);
                if (rent.ExamId == examId)
                {
                    return false;
                }
            }
            return true;
        }

        private void CheckQrCode()
        {
            _zxing.OnScanResult += (result) => Device.BeginInvokeOnMainThread(async () =>
            {
                // Stop analysis until we navigate away so we don't keep reading barcodes
                _zxing.IsAnalyzing = false;

                // removing invalid signs from result
                var res = result.Text.Replace(Environment.NewLine, "");

                // check if QR Code is Valid
                if ((res.Length == 7 && IsDigitsOnly(res)) || (res.Length > 7 && IsExamQr(res)))
                {
                    // Process StudentID
                    if (_studentId.Length == 0 && _examId.Length == 0 && IsDigitsOnly(res))
                    {
                        _studentId = res;
                        _scanLabel.Text = "Zum Klausuren ausleihen Klausur-QR-Code einscannen!";
                    }

                    // Return Exam
                    if (_studentId.Length == 0 && _examId.Length == 0 && IsExamQr(res))
                    {
                        _examId = res;

                        var rentId = await GetRentId(ParseExamId(_examId));

                        if (rentId >= 1)
                        {
                            await SendReturnData(rentId);
                        }
                        else
                        {
                            await DisplayAlert("Eingescannte Klausur bereits zurückgegeben!", "Scanne bitte eine andere Klausur ein", "OK");
                            await RewriteLabel();
                        }   
                    }

                    // Rent Exam
                    if (_studentId.Length == 7 && _examId.Length == 0 && IsExamQr(res))
                    {
                        _examId = res;
                        if (await CheckExamAvailability(ParseExamId(_examId)))
                        {
                            await SendRentData();
                        }
                        else
                        {
                            await DisplayAlert("Klausur bereits ausgeliehen", "Scanne bitte eine andere Klausur ein", "OK");
                            await RewriteLabel();
                        }
                        
                    }
                }
                else
                {
                   await DisplayAlert("Barcode Scanner", "Bitte einen validen QR-Code einscannen!", "OK");
                }

            });   
        }
        

        private async Task SendRentData()
        {
            _scanLabel.TextColor = Color.Green;
            _scanLabel.Text = "Klausur ausgeliehen!";
           
            var rentClient = new RestRents();
            await rentClient.RentExam(int.Parse(_studentId), ParseExamId(_examId));
            await RewriteLabel();
        }

        private async Task SendReturnData(int rentId)
        {
            _scanLabel.TextColor = Color.Green;
            _scanLabel.Text = "Klausur zurückgegeben!";
            
            var rentClient = new RestRents();
            
            await rentClient.ReturnExam(ParseExamId(_examId), rentId);
            await RewriteLabel();
        }

        private static int ParseExamId(string exam)
        {
            return int.Parse(exam.Split('_')[0].Split(':')[1]);
        }

        private async Task RewriteLabel()
        {
            await Task.Delay(1000);
            _examId = "";
            _scanLabel.TextColor = Color.White;
            _scanLabel.Text = "Weiteren QR-Code scannen";
        }

        bool IsExamQr(string str)
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

        private static bool IsDigitsOnly(string str)
        {
            return str.Cast<char>().All(c => c >= '0' && c <= '9');
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await CheckForCameraPermissions();
        }

        protected override void OnDisappearing()
        {
            _zxing.IsScanning = false;
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

                    var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Camera);
                    status = results[Permission.Camera];
                }

                if (status == PermissionStatus.Granted)
                {
                    _zxing.IsScanning = true;
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

        /*public void playSound()
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
        }*/
    }
}

