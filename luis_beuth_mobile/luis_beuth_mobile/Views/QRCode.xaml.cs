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
                SetStudentId();
            }
            else
            {
                barcode.BarcodeValue = "S0000000";
                StudentLogin(this, EventArgs.Empty);
            }

            Content = barcode;           
        }

        private async void StudentLogin(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new BarcodeScannerLogin()));
            MessagingCenter.Subscribe<string>(this, "LoginSuccessful", (msg) =>
            {
                SetStudentId();
            });
        }

        private void SetStudentId()
        {
            var studentId = Application.Current.Properties ["studentId"] as string;
            barcode.BarcodeValue = studentId;
            Content = barcode;
        }

    }
}
