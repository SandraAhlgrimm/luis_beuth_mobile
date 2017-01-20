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

            detailPages.Add(new DetailItem { Title = "Login", IconSource = "profile.png", targetPage = typeof(BarcodeScannerLogin) });
            detailPages.Add(new DetailItem { Title = "Mein Profil", targetPage = typeof(StudentProfile) });
            detailPages.Add(new DetailItem { Title = "Alle Klausuren", targetPage = typeof(ExamOverview) });
            detailPages.Add(new DetailItem { Title = "Meine Klausuren", targetPage = typeof(MyExams) });
            detailPages.Add(new DetailItem { Title="Mein QR Code", targetPage=typeof(QRCode) });
            detailPages.Add(new DetailItem { Title = "FSR Login", targetPage = typeof(FSRLogin) });
            detailPages.Add(new DetailItem { Title = "Klausur scannen", targetPage = typeof(BarcodeScanner) });
            detailPages.Add(new DetailItem { Title = "Feedback & Hilfe", targetPage = typeof(Help) });
            detailPages.Add(new DetailItem { Title = "Impressum", targetPage = typeof(Impressum) });

            listView.ItemsSource = detailPages;
        }
    }
}
