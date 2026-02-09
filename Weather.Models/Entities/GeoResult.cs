

namespace Weather.Models.Entities
{
    public class GeoResponse
    {
        public string name { get; set; }
        public string country { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public string country_code { get; set; }
    }

    //public class GeoResponse
    //{
    //    public List<GeoResponse> results { get; set; }
    //}
}
