using luis_beuth_mobile.Model;
using System.Collections.Generic;

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
<<<<<<< HEAD
            detailPages.Add(new DetailItem {Title="QRCode", targetPage=typeof(QRCode) });
            detailPages.Add(new DetailItem {Title="Meine Klausuren", targetPage=typeof(ExamOverview) });
            detailPages.Add(new DetailItem { Title = "FSR-Login", targetPage = typeof(FSRLogin) });
			detailPages.Add(new DetailItem { Title = "Barcode-Scanner", targetPage = typeof(BarcodeScanner) });
=======
            detailPages.Add(new DetailItem { Title="QRCode", targetPage=typeof(QRCode) });
            detailPages.Add(new DetailItem { Title = "Profil", targetPage = typeof(StudentProfile) });
            detailPages.Add(new DetailItem { Title="Klausur Übersicht", targetPage=typeof(ExamOverview) });
            detailPages.Add(new DetailItem { Title = "FSR Login", targetPage = typeof(FSRLogin) });
			detailPages.Add(new DetailItem { Title = "Barcode Scanner", targetPage = typeof(BarcodeScanner) });
            detailPages.Add(new DetailItem { Title = "Studenten Login", targetPage = typeof(BarcodeScannerLogin) });
>>>>>>> 0d076497fbbb1c0bbe5807e6479aeffd7b8c2d73
            listView.ItemsSource = detailPages;
        }
    }
}
