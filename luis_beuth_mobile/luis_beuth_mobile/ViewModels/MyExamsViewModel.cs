using luis_beuth_mobile.Model;
using luis_beuth_mobile.Models.Data;
using System.Collections.Generic;
using Xamarin.Forms;


namespace luis_beuth_mobile.ViewModels
{
    class MyExamsViewModel : BaseViewModel
    {
        List<Exam> _allExams;
        public List<Exam> AllExams { get { return _allExams; } set { _allExams = value; NotifyPropertyChanged(); } }


        public MyExamsViewModel()
        {
            AllExams = new List<Exam>();
        }

        public async void GetExams()
        {
            RESTExams restApi = new RESTExams();
            var studentId = Application.Current.Properties["studentId"] as string;
            var exams = await restApi.GetAllRentedExams(int.Parse(studentId));
            AllExams = exams;
        }


    }
}
