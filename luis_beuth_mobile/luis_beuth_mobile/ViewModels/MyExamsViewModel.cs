using luis_beuth_mobile.Model;
using luis_beuth_mobile.Models.Data;
using System.Collections.Generic;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
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
            RestExams restApi = new RestExams();
            var studentId = Application.Current.Properties["studentId"] as string;
            var rents = await restApi.GetAllRentedExams(int.Parse(studentId));
            var exams = await restApi.Get();

            var result = new List<Exam>();

            foreach (var t in rents)
            {
                Exam res = exams.Find(item => item.Id == t.ExamId);
                result.Add(res);
            }
            

            AllExams = result;
        }


    }
}
