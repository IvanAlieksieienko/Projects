using Newtonsoft.Json;
using SERP.Core.IServices;
using SERP.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SERP.Core.Services
{
    public class SerpService : ISerpService
    {
        private static string BaseAddress = @"https://api.dataforseo.com/";
        private static string Login = @"challenger26@rankactive.info";
        private static string Password = @"q2Q8faRdK";

        public async Task<int> GetPosition(TaskModel model)
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(BaseAddress),
                DefaultRequestHeaders = { Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes(Login + ":" + Password))) }
            };
            var postData = new List<object>();

            postData.Add(new
            {
                language_code = "en",
                location_code = model.LocationCode,
                keyword = model.Keywords
            });

            var taskPostResponse = await httpClient.PostAsync(@"/v3/serp/" + model.SearchEngine + @"/organic/live/regular",
                                                              new StringContent(JsonConvert.SerializeObject(postData)));
            var result = JsonConvert.DeserializeObject<dynamic>(await taskPostResponse.Content.ReadAsStringAsync());

            if (result.status_code == 20000)
            {
                var tasks = result.tasks;
                foreach (var task in tasks)
                {
                    var resultsOfTask = task.result;
                    foreach (var serpResult in resultsOfTask)
                    {
                        if (model.Keywords == serpResult.keyword.ToString())
                        {
                            IEnumerable<dynamic> items = serpResult.items;
                            var searchedItem = items.FirstOrDefault(i => i.domain == model.Domain);
                            var position = Convert.ToInt32(searchedItem?.rank_absolute);
                            return position;
                        }
                    }
                }
            }

            return 0;
        }

        public async Task<List<LocationModel>> GetLocations(string searchEngine)
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(BaseAddress),
                DefaultRequestHeaders = { Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes(Login + ":" + Password))) }
            };

            var response = await httpClient.GetAsync(@"/v3/serp/" + searchEngine + @"/locations");
            var result = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());
            var locationsResults = new List<LocationModel>();
            if (result.status_code == 20000)
            {
                var tasks = result.tasks;
                foreach (var task in tasks)
                {
                    var resultsOfTask = task.result;
                    foreach (var locationResult in resultsOfTask)
                    {
                        if (locationResult.location_type.ToString() == "Country")
                        {
                            locationsResults.Add(new LocationModel() { LocationCode = Convert.ToInt32(locationResult.location_code), LocationName = locationResult.location_name.ToString() });
                        }
                        
                    }
                }
            }

            return locationsResults;
        }
    }
}
