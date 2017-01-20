using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace luis_beuth_mobile.Views
{
    public partial class Help : ContentPage
    {
        public Help()
        {
            InitializeComponent();

            Button emailButton = new Button { Text = "Mail" };
            emailButton.Clicked += (sender, e) =>
            {
                Device.OpenUri(new Uri("mailto:ryan.hatfield@test.com"));
            };
        }


    }
}
