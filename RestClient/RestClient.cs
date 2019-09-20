using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Plugin.RestClient
{
    /// <summary>
    /// RestClient implements methods for calling CRUD operations
    /// using HTTP.
    /// </summary>
    public class RestClient<T>
    {
        private string WebServiceUrl { get; set; }

        public string WebServiceUrlProperty
        {
            get { return WebServiceUrl; }
            set { WebServiceUrl = value; }
        }


        public async Task<List<T>> GetAsync()
        {
            var httpClient = new HttpClient();

            var json = await httpClient.GetStringAsync(WebServiceUrl);

            var ListOfItems = JsonConvert.DeserializeObject<List<T>>(json);

            return ListOfItems;
        }

        public async Task<bool> PostAsync(T t)
        {
            var httpClient = new HttpClient();

            var json = JsonConvert.SerializeObject(t);

            HttpContent httpContent = new StringContent(json);

            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var result = await httpClient.PostAsync(WebServiceUrl, httpContent);

            return result.IsSuccessStatusCode;
        }

        public async Task<bool> PutAsync( T t)
        {
            var httpClient = new HttpClient();

            var json = JsonConvert.SerializeObject(t);

            HttpContent httpContent = new StringContent(json);

            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var result = await httpClient.PutAsync(WebServiceUrl, httpContent);

            return result.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync()
        {
            var httpClient = new HttpClient();

            var response = await httpClient.DeleteAsync(WebServiceUrl);

            return response.IsSuccessStatusCode;
        }

        public async Task<List<T>> GetBykeywordAsync(string keyword)
        {
            var httpClient = new HttpClient();

            var json = await httpClient.GetStringAsync(WebServiceUrl + "Search/" + keyword);

            List<T> ListOfItems = JsonConvert.DeserializeObject<List<T>>(json);

            return ListOfItems;
        }
    }
}
