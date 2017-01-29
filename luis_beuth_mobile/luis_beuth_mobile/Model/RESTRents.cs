using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace luis_beuth_mobile
{
    class RESTRents
    {
        public async Task rentExam(int studentId, int examId)
        {
            var httpClient = new HttpClient();

            dynamic data = new ExpandoObject();
            data.student = new ExpandoObject() as dynamic;
            data.student.matriculationNumber = studentId;
            data.examId = examId;
            
            var rentJSON = JsonConvert.SerializeObject(data);
            string url = "http://luis-beuth.azurewebsites.net/api/rent/";

            var content = new StringContent(rentJSON.ToString(), Encoding.UTF8, "application/json");
            
            var result = httpClient.PostAsync(url, content).Result;
            Debug.WriteLine("RESTRent_rentExam()_content: " + rentJSON.ToString());
            Debug.WriteLine("RESTRent_rentExam()_result: " + result);
        }

        public async Task returnExam(int examId)
        {
            Debug.WriteLine("RESTRent_returnExam()_start");
            var httpClient = new HttpClient();

            dynamic data = new ExpandoObject();
            data.examId = examId;

            var rentJson = JsonConvert.SerializeObject(data);
            string url = "http://luis-beuth.azurewebsites.net/api/rent/";

            var content = new StringContent(rentJson.ToString(), Encoding.UTF8, "application/json");
            var result = httpClient.PutAsync(url, content).Result;
            
            Debug.WriteLine("RESTRent_returnExam()_content: " + rentJson.ToString());
            Debug.WriteLine("RESTRent_returnExam()_result: " + result);
        }
    }
}
