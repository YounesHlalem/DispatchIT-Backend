using IT.Netic.DispatchIt.Web.Backend.Domain.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IT.Netic.DispatchIt.Web.Backend.Services
{
    public class CoordinatesService
    {
        private HttpClient ApiClient;
        public CoordinatesService()
        {
            ApiClient = new HttpClient();
        }

        public async Task<string[]> locationToLonLatAsync(string location)
        {
            HttpResponseMessage compa = await ApiClient.GetAsync("https://nominatim.openstreetmap.org/?addressdetails=1&q=" + location + "&polygon_geojson=1&format=jsonv2&email=-");
            string co = await compa.Content.ReadAsStringAsync();
            Coordinates[] obj = JsonConvert.DeserializeObject<Coordinates[]>(co);

            string[] cor = { obj[0].lon, obj[0].lat };
            return cor;
        }
    }
}
