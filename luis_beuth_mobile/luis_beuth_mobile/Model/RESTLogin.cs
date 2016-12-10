using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace luis_beuth_mobile
{
	public class RESTLogin
	{
		public async Task<Boolean> Validate( int matriculationNumber, string pwHash )
		{
			var http = new HttpClient();
			string url = "http://localhost:5000/api/login?{matriculationNumber}/{pwHash}";
			var response = await http.GetAsync(url);
			return response.IsSuccessStatusCode;
		}
	}
}
