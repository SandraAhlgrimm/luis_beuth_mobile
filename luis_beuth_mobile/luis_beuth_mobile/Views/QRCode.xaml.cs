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
		}
    }
}
