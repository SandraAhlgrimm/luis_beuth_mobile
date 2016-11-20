using luis_beuth_mobile.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace luis_beuth_mobile.Views
{
    public partial class MasterPage : ContentPage
    {
        public ListView ListView { get { return listView; } }

        public MasterPage()
        {
            InitializeComponent();
            var detailPages = new List<DetailItem>();
            detailPages.Add(new DetailItem {Title="QRCode", targetPage=typeof(QRCode) });
            detailPages.Add(new DetailItem {Title="Klausur Übersicht", targetPage=typeof(ExamOverview) });
            detailPages.Add(new DetailItem { Title = "FSR Login", targetPage = typeof(FSRLogin) });
			detailPages.Add(new DetailItem { Title = "Barcode Scanner", targetPage = typeof(BarcodeScanner) });
            detailPages.Add(new DetailItem { Title = "Studenten wechseln", targetPage = typeof(BarcodeScannerLogin) });
            listView.ItemsSource = detailPages;
        }
    }
}
