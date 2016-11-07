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
		ZXingBarcodeImageView barcode;

		public QRCode() : base ()
        {
			InitializeComponent();

            Debug.WriteLine("Generating QR-Code");

			barcode = new ZXingBarcodeImageView
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
			};
			barcode.BarcodeFormat = ZXing.BarcodeFormat.QR_CODE;
			barcode.BarcodeOptions.Width = 300;
			barcode.BarcodeOptions.Height = 300;
			barcode.BarcodeOptions.Margin = 10;

            if (Application.Current.Properties.ContainsKey("studentId"))
            {
                var studentId = Application.Current.Properties["studentId"] as string;
                Debug.WriteLine(studentId);
                barcode.BarcodeValue = studentId;
            }
            else
            {
                // TODO: avoid setting barcodevalue to S0000000
                barcode.BarcodeValue = "S0000000";
                StudentLogin(this, EventArgs.Empty);
            }

            Debug.WriteLine("LOG: Generating QR Code with Value: " + barcode.BarcodeValue);

            Content = barcode;

            
        }

        private async void StudentLogin(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new BarcodeScannerLogin()));
        }
    }
}
