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
            
            var studentJSON = JsonConvert.SerializeObject(data);
            string url = "http://luis-beuth.azurewebsites.net/api/rent/";

            var content = new StringContent(studentJSON.ToString(), Encoding.UTF8, "application/json");
            
            var result = httpClient.PostAsync(url, content).Result;
            Debug.WriteLine("RESTRent_rentExam()_content: " + content.ToString());
            Debug.WriteLine("RESTRent_rentExam()_result: " + result);
        }

        public async Task returnExam(int examId)
        {
            var httpClient = new HttpClient();

            dynamic data = new ExpandoObject();
            data.rent = new ExpandoObject() as dynamic;
            data.rent.id = 1;

            var studentJSON = JsonConvert.SerializeObject(data);
            string url = "http://luis-beuth.azurewebsites.net/api/rent/1";

            var content = new StringContent(studentJSON.ToString(), Encoding.UTF8, "application/json");
            var result = httpClient.PutAsync(url, content).Result;
            
            Debug.WriteLine("RESTRent_returnExam()_content: " + content.ToString());
            Debug.WriteLine("RESTRent_returnExam()_result: " + result);
        }
    }
}
