using luis_beuth_mobile.Model;
using luis_beuth_mobile.Models.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


namespace luis_beuth_mobile.ViewModels
{
    class ExamOverviewViewModel : BaseViewModel
    {
        List<Exam> _allExams;
        public List<Exam> AllExams { get { return _allExams; }set { _allExams = value; NotifyPropertyChanged(); } }


        public ExamOverviewViewModel()
        {
            AllExams = new List<Exam>();
            RESTExams restApi = new RESTExams();
            var exams = restApi.Get();
        }

        public async void GetExams()
        {
            RESTExams restApi = new RESTExams();
            var exams = await restApi.Get();
            AllExams =exams;
        }

       
    }
}
