using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ModernHttpClient;
using System.Net.Http;
using System.Net.Http.Headers;

namespace SistemaHiper
{
	public class OmdbApiService
	{
		private const string OmbdDns = "https://www.omdbapi.com/?";


		public OmdbApiService()
		{
		}


		public async Task<OmdbApiResult<Movies>> GetMovies(string filter, string page)
		{
			var querystring = string.Empty;
			if (!string.IsNullOrEmpty(filter))
				querystring += $"s={System.Net.WebUtility.UrlEncode(filter)}";

			querystring += $"&type=movie&page={page}";


			var result = await this.MakeHttpCall<OmdbApiResult<Movies>>(querystring);
			return result;

		}



		private async Task<TOutput> MakeHttpCall<TOutput>(string filter)
		{

			HttpClient client = new HttpClient(new NativeMessageHandler());

			client.DefaultRequestHeaders.Accept.Clear();

			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


			string url = $"{OmbdDns}{filter}";


			HttpResponseMessage response = new HttpResponseMessage();

			try
			{

				response = await client.GetAsync(new Uri(url)).ConfigureAwait(false);

				string responseText = await response.Content.ReadAsStringAsync();
				if (response.IsSuccessStatusCode)
				{
					return JsonConvert.DeserializeObject<TOutput>(responseText);
				}
				else {
					throw new Exception(string.Format("Response Statuscode for {0}: {1}", url, response.StatusCode));
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}





	}
}
