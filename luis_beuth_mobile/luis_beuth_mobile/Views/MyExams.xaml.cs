using luis_beuth_mobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace luis_beuth_mobile.Views
{
    
    public partial class MyExams : ContentPage
    {
        MyExamsViewModel vm;
        public MyExams()
        {
            InitializeComponent(); vm = new MyExamsViewModel();
            BindingContext = vm;

        }

        protected override void OnAppearing()
        {
            vm.GetExams();
            base.OnAppearing();

        }
    }
}
