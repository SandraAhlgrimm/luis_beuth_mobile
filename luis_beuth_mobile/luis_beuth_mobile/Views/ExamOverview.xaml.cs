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
        ExamOverviewViewModel vm;
        public ExamOverview()
        {
            InitializeComponent();
            vm = new ExamOverviewViewModel();
            BindingContext = vm;
            
        }

        protected override void OnAppearing()
        {
            vm.GetExams();
            base.OnAppearing();
            
        }
    }
}
