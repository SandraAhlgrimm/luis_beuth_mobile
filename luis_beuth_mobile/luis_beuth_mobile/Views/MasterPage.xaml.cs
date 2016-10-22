using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace luis_beuth_mobile.Views
{
    public partial class MasterPage : ContentPage
    {
        public MasterPage()
        {
            InitializeComponent();
            var detailPages = new List<string>();
            detailPages.Add("QRCode");

            listView.ItemsSource = detailPages;
        }
    }
}
