using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

namespace luis_beuth_mobile.Views
{
    public partial class QRCode : ContentPage
    {
		//ZXingScannerView zxing;
		//ZXingDefaultOverlay overlay;

		ZXingBarcodeImageView barcode;

		public QRCode() : base ()
        {
			InitializeComponent();

			barcode = new ZXingBarcodeImageView
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
			};
			barcode.BarcodeFormat = ZXing.BarcodeFormat.QR_CODE;
			barcode.BarcodeOptions.Width = 300;
			barcode.BarcodeOptions.Height = 300;
			barcode.BarcodeOptions.Margin = 10;
			barcode.BarcodeValue = "ZXing.Net.Mobile";

			Content = barcode;

			/*zxing = new ZXingScannerView
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand
			};
			zxing.OnScanResult += (result) =>
				Device.BeginInvokeOnMainThread(async () =>
				{

					// Stop analysis until we navigate away so we don't keep reading barcodes
					zxing.IsAnalyzing = false;

					// Show an alert
					await DisplayAlert("Scanned Barcode", result.Text, "OK");

					// Navigate away
					await Navigation.PopAsync();
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
			Content = grid;*/
		}

		/*protected override void OnAppearing()
		{
			base.OnAppearing();

			try
			{
				zxing.IsScanning = true;

			}
			catch (Exception ex)
			{
				Debug.WriteLine("Error: " + ex.Message);
			}
		}

		protected override void OnDisappearing()
		{
			zxing.IsScanning = false;

			base.OnDisappearing();
		}*/
    }
}
