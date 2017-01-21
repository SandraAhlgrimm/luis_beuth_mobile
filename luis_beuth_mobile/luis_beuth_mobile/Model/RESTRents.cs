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
            data.studentId = studentId;
            data.examId = examId;

            var studentJSON = JsonConvert.SerializeObject(data);
            string url = "http://luis-beuth.azurewebsites.net/api/rent/";

            var content = new StringContent(studentJSON.ToString(), Encoding.UTF8, "application/json");
            var result = httpClient.PostAsync(url, content).Result;


            Debug.WriteLine("RESULT: " + result);
        }
    }
}
