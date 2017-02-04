using luis_beuth_mobile.Model;
using luis_beuth_mobile.Models.Data;
using System.Collections.Generic;


namespace luis_beuth_mobile.ViewModels
{
	class ExamOverviewViewModel : BaseViewModel
	{
		List<Exam> _allExams;
		public List<Exam> AllExams { get { return _allExams; } set { _allExams = value; NotifyPropertyChanged(); } }


		public ExamOverviewViewModel()
		{
			AllExams = new List<Exam>();
			RestExams restApi = new RestExams();
			var exams = restApi.Get();
		}

		public async void GetExams()
		{
			RestExams restApi = new RestExams();
			var exams = await restApi.Get();
			AllExams = exams;
		}


	}
}
