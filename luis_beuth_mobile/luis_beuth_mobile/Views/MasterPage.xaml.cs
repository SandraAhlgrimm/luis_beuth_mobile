using luis_beuth_mobile.Model;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using Xamarin.Forms;

namespace luis_beuth_mobile.Views
{
    public partial class MasterPage : ContentPage
    {
        public Xamarin.Forms.ListView ListView { get { return ListView; } }

        public MasterPage()
        {
            InitializeComponent();
            var detailPages = new List<DetailItem>();

            detailPages.Add(new DetailItem { Title = "Login", IconSource = "ic_exit_to_app_black_18dp.png", targetPage = typeof(BarcodeScannerLogin) });
            detailPages.Add(new DetailItem { Title = "Mein Profil", IconSource = "ic_exit_to_app_black_18dp.png", targetPage = typeof(StudentProfile) });
            detailPages.Add(new DetailItem { Title = "Alle Klausuren", IconSource = "ic_exit_to_app_black_18dp.png", targetPage = typeof(ExamOverview) });
            detailPages.Add(new DetailItem { Title = "Meine Klausuren", IconSource = "ic_exit_to_app_black_18dp.png", targetPage = typeof(MyExams) });
            detailPages.Add(new DetailItem { Title="Mein QR Code", IconSource = "ic_exit_to_app_black_18dp.png", targetPage = typeof(QRCode) });
            detailPages.Add(new DetailItem { Title = "FSR Login", IconSource = "ic_exit_to_app_black_18dp.png", targetPage = typeof(FSRLogin) });
            //detailPages.Add(new DetailItem { Title = "Klausur scannen", targetPage = typeof(BarcodeScanner) });
            detailPages.Add(new DetailItem { Title = "Feedback & Hilfe", IconSource = "ic_exit_to_app_black_18dp.png", targetPage = typeof(Help) });
            detailPages.Add(new DetailItem { Title = "Impressum", IconSource = "ic_exit_to_app_black_18dp.png", targetPage = typeof(Impressum) });
            ListView.ItemsSource = detailPages;
        }
    }
}
