using System;
using Xamarin.Forms;

namespace luis_beuth_mobile.Views
{
	public partial class StudentProfile : ContentPage
	{
		public StudentProfile()
		{
			InitializeComponent();
            BindingContext = new ViewModels.StudentProfileViewModel();
        }
       

    }
}