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
    public class RESTSTudentSignUp
    {

        public async Task addStudent(string name, int matNum)
        {
            var httpClient = new HttpClient();

            dynamic data = new ExpandoObject();
            data.name = name;
            data.matriculationNumber = matNum;
            
            var studentJSON = JsonConvert.SerializeObject(data);
            string url = "http://luis-beuth.azurewebsites.net/api/student/";

            var content = new StringContent(studentJSON.ToString(), Encoding.UTF8, "application/json");
            var result = httpClient.PostAsync(url, content).Result;
        }
    }
}
