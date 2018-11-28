using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
//using kkday.EC.Test.Models.Repostory;
//using KKday.API.WMS.Models.DataModel.Product;
using KKday.SearchProd.AppCode;
using KKday.SearchProd.Models.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KKday.SearchProd.Models.Repostory
{
    public class CountryRepostory
    {

        #region WMS-API

        //private static List<CountryInfo> FetchCountries()
        //{
        //    #region Call CountryProxy

        //    List<CountryInfo> country_list = new List<CountryInfo>();

        //    var uri = "https://192.168.2.83:6001/api/Common/ProductCountryCity";

        //    var query = new Dictionary<string, object>
        //    {
        //        ["ipaddress"] = "172.18.0.1",
        //        ["apiKey"] = "kkdayapi",
        //        ["userOid"] = "1",
        //        ["ver"] = "1.0.1",
        //        ["locale"] = "zh-tw"
        //    };

        //    var countryRS = CountryProxy.Post(uri, query);

        //    //將Response轉成Object
        //    var countryObj = JObject.Parse(countryRS);

        //    //查詢產品
        //    var countries = countryObj["content"]["countryList"];

        //    //準備可售商品之國家&城市資料
        //    foreach (var countryVal in countries)
        //    {
        //        CountryInfo countryInfo = new CountryInfo()
        //        {
        //            CountryName = countryVal["countryName"].ToString(),
        //            CountryCode = countryVal["countryCd"].ToString(),
        //            Cities = new List<CityInfo>()
        //        };

        //        //城市資料
        //        foreach (var cityVal in countryVal["cityList"])
        //        {
        //            countryInfo.Cities.Add(new CityInfo()
        //            {
        //                CityName = cityVal["cityName"].ToString(),
        //                CityCode = cityVal["cityCd"].ToString()
        //            });
        //        }

        //        country_list.Add(countryInfo);
        //    }

        //    #endregion

        //    return country_list;
        //}

        ////Index頁面用(第一次)
        //public static List<CountryInfo> GetCountries()
        //{ 
        //    var country_list = FetchCountries(); 
        //    return country_list; 
        //}

        //public static List<CountryInfo> GetCountries(string key1)
        //{
        //    var country_list = FetchCountries();

        //    // Search Country, if it's matched
        //    var _country = country_list.Where(l => l.CountryName.Equals(key1)).ToList();

        //    // Search city, if it's matched get countryInfo
        //    var _city = country_list.Where(s => s.Cities.Where(c => c.CityName.Equals(key1)).Count() > 0).ToList();

        //    return (List<CountryInfo>)((_country.Count() > 0) ? _country : _city);
        //}


        #endregion

        #region KK-API

        private static List<TravelLine> FetchCountries(string locale) 
        {
            List<TravelLine> tl_list = new List<TravelLine>();

            #region Call CountryProxy

            var uri = "https://api.sit.kkday.com/api/areacontinent";

            var query = new Dictionary<string, object>
            {
                ["ipaddress"] = "61.216.90.92",
                ["apiKey"] = "kkdayapi",
                ["userOid"] = "1",
                ["ver"] = "1.0.1",
                ["locale"] = locale //,
                //["json"]= "",
                //["currency"] = "TWD"
            };

            #endregion

            var areaRS = CountryProxy.Post(uri, query);
            //將Response轉換成Object
            var areaObj = JObject.Parse(areaRS);

            //讀取第一層(線別資料)
            var areas = areaObj["content"]["areaList"];
            foreach (var tlToken in areas.AsJEnumerable())
            {
                var tlVal = tlToken["continent"];

                List<CountryInfo> countries = new List<CountryInfo>();

                //讀取第二層(國家資料)
                foreach (var countryToken in tlToken["countrys"])
                {
                    var countryVal = countryToken["country"];
                    string[] countryArray = countryVal.ToString().Split("|||");

                    //建立國家項目
                    CountryInfo countryInfo = new CountryInfo()
                    {
                        CountryName = countryArray[0],
                        CountryCode = countryArray[1],
                        Cities = new List<CityInfo>()
                    };

                    //讀取第三層(城市資料)
                    foreach (var cityVal in countryToken["citys"])
                    {
                        string[] cityArray = cityVal.ToString().Split("|||");

                        //新增城市至國家項目
                        countryInfo.Cities.Add(new CityInfo()
                        {
                            CityName = cityArray[0],
                            CityCode = cityArray[1]
                        });
                    }
                    countries.Add(countryInfo);

                }

                string[] tlArray = tlVal.ToString().Split("|||");
                //新增線別項目
                tl_list.Add(new TravelLine()
                {
                    TlName = tlArray[0],
                    TlCode = tlArray[1],
                    Countries = countries
                });
            }

            return tl_list;
        }

        //Index頁面用(第一次)
        public static List<TravelLine> GetCountries(string locale)
        { 
            var tl_list = FetchCountries(locale); 
            return tl_list; 
        }

        //ProdList頁面用(第N次)
        public static List<CountryInfo> GetCountries(string key1, string citykey, string locale)
        {
            var tl_list = FetchCountries(locale);

            // Search Country, if it's matched
            var new_country = tl_list.SelectMany(l => l.Countries.Where(s => s.CountryName.Equals(key1)).ToList()).ToList();

            // Search city, if it's matched get countryInfo
            var new_country2 = tl_list.SelectMany(l => l.Countries.Where(s => s.Cities.Where(c => c.CityName.Equals(key1)).Count() > 0).ToList()).ToList();

            //Search citykey, if it's matched get countrInfo
            var new_citykey = tl_list.SelectMany(l => l.Countries.Where(s => s.Cities.Where(c => c.CityCode.Equals(citykey)).Count() > 0).ToList()).ToList();

            var final_country = (List<CountryInfo>)((new_country.Count() > 0) ? new_country : ((new_citykey.Count() > 0)? new_citykey: new_country2));

            //判斷國家裡面的城市名稱或代碼條件符合
            final_country.SelectMany(f => f.Cities.Where(c => c.CityCode.Equals(citykey) || c.CityName.Equals(key1)).ToList()).ToList().ForEach(c => {
                c.IsSelceted = true;
            });

            return final_country;
        }

        #endregion


    }
}
