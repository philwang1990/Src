using System;
using System.Collections.Generic;
namespace KKday.API.WMS.Models.DataModel.Common

{

    public class ProductCountryCityModel
    {
        public ProductCountryCity content { get; set; }

    }

    public class ProductCountryCity
    {
        public string result { get; set; }
        public string msg { get; set; }
        public List<Country> countryList { get; set; }

    }

    public class Country
    {
        public string countryCd { get; set; }
        public string countryName { get; set; }
        public List<City> cityList { get; set; }
    }

    public class City
    {
        public string cityCd { get; set; }
        public string cityName { get; set; }
    }



}