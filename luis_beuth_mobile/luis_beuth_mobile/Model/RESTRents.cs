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
using luis_beuthluis_beuth_mobile.Models.Data;

namespace luis_beuth_mobile
{
    class RestRents
    {
        private string url = "http://luis-beuth.azurewebsites.net/api/rent/";

        public async Task<List<Rent>> getAllRents()
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Debug.WriteLine("DEBUG_CONTENT: " + content);
                var rents = stringToRent(content);
                return rents;
            }
            else { return null; }
        }

        public async Task<int> getRentId(int examId)
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var rents = stringToRent(content);
                foreach (var rent in rents)
                {
                    if (rent.ExamId == examId)
                    {
                        return rent.Id;
                    }
                }
                return -1;
            }
            else
            {
                return 0;
            }
        }

        public async Task RentExam(int studentId, int examId)
        {
            var httpClient = new HttpClient();

            dynamic data = new ExpandoObject();
            data.student = new ExpandoObject() as dynamic;
            data.student.matriculationNumber = studentId;
            data.examId = examId;
            
            var rentJson = JsonConvert.SerializeObject(data);

            var content = new StringContent(rentJson.ToString(), Encoding.UTF8, "application/json");
            
            var result = httpClient.PostAsync(url, content).Result;
            Debug.WriteLine("RESTRent_rentExam()_content: " + rentJson.ToString());
            Debug.WriteLine("RESTRent_rentExam()_result: " + result);
        }

        public async Task ReturnExam(int examId, int rentId)
        {
            var httpClient = new HttpClient();

            // normal request
            dynamic data = new ExpandoObject();
            data.examId = examId;
            var bodyJson = JsonConvert.SerializeObject(data);
            url = "http://luis-beuth.azurewebsites.net/api/rent/";
            var content = new StringContent(bodyJson.ToString(), Encoding.UTF8, "application/json");
            var result = httpClient.PutAsync(url, content).Result;
            Debug.WriteLine("RESTRent_returnExam()_content: " + bodyJson.ToString());
            Debug.WriteLine("RESTRent_returnExam()_result: " + result);

            // delete request
            url = url + rentId;
            var response = httpClient.DeleteAsync(url);
            Debug.WriteLine("RESTRent_returnExam()_url: " + url);
            Debug.WriteLine("RESTRent_returnExam()_result: " + response.Result);
        }

        private List<Rent> stringToRent(string json)
        {
            return JsonConvert.DeserializeObject<List<Rent>>(json);
        }
    }
}
