using System;
using System.Collections.Generic;
using System.Text;

namespace IT.Netic.DispatchIt.Web.Backend.Domain.Entities
{
    public class Location
    {
        public string house_number { get; set; }
        public string road { get; set; }
        public string neighbourhood { get; set; }
        public string town { get; set; }
        public string city { get; set; }
        public string county { get; set; }
        public string state { get; set; }
        public string region { get; set; }
        public string postcode { get; set; }
        public string country { get; set; }
        public string country_code { get; set; }
    }



    public class Geojson
    {
        public string type { get; set; }
        //public List<List<List<Double>>> coordinates { get; set; }
    }

    public class Coordinates
    {
        public int place_id { get; set; }
        public string licence { get; set; }
        public string osm_type { get; set; }
        public long osm_id { get; set; }
        public List<string> boundingbox { get; set; }
        public string lat { get; set; }
        public string lon { get; set; }
        public string display_name { get; set; }
        public int place_rank { get; set; }
        public string category { get; set; }
        public string type { get; set; }
        public double importance { get; set; }
        public Location address { get; set; }
        public Geojson geojson { get; set; }
    }
}
