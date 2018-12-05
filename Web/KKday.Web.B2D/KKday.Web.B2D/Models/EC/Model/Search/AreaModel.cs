using System;
using System.Collections.Generic;

namespace KKday.SearchProd.Models.Model
{
    public class TravelLine
    {
        public string TlName { get; set; }
        public string TlCode { get; set; }
        public List<CountryInfo> Countries { get; set; }
    }

    public class CountryInfo
    {
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
        public List<CityInfo> Cities { get; set; }
    }
    public class CityInfo
    {
        public string CityName { get; set; }
        public string CityCode { get; set; }
        public bool IsSelceted { get; set; }
    }

}
