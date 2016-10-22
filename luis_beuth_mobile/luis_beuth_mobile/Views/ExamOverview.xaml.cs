using luis_beuth_mobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace luis_beuth_mobile.Views
{
    public partial class ExamOverview : ContentPage
    {
        public ExamOverview()
        {
            InitializeComponent();
            this.BindingContext = new ExamOverviewViewModel();
        }
    }
}
