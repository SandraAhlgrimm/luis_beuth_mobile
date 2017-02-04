using luis_beuth_mobile.Model;
using luis_beuth_mobile.Models.Data;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using luis_beuthluis_beuth_mobile.Models.Data;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;

namespace luis_beuth_mobile.ViewModels
{
	class ExamOverviewViewModel : BaseViewModel
	{
		List<Exam> _allExams;
		public List<Exam> AllExams { get { return _allExams; } set { _allExams = value; NotifyPropertyChanged(); } }


		public ExamOverviewViewModel()
		{
			AllExams = new List<Exam>();
		}

		public async void GetExams()
		{
            RESTExams restApi = new RESTExams();
            var studentId = Application.Current.Properties["studentId"] as string;
            var rents = await restApi.GetAllRentedExams(int.Parse(studentId));
            var exams = await restApi.Get();

            var result = new List<Exam>();

            foreach (var t in rents)
            {
                Exam res = exams.Find(item => item.Id == t.ExamId);
                result.Add(res);
            }

            foreach (var t in exams)
            {
                t.Rents = new List<Rent>();

                var matches = rents.Where(item => item.ExamId == t.Id);
                if (matches.ToList().Count != 0)
                {
                    t.Rents = new List<Rent>();
                }
                else
                {
                    t.Rents = null;
                }
            }
            AllExams = exams;
        }


	}
}
