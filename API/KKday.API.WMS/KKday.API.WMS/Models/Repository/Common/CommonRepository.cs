using System;
using System.Data;
using KKday.API.WMS.AppCode.DAL;
using Newtonsoft.Json.Linq;
using KKday.API.WMS.Models.DataModel.Common;
using KKday.API.WMS.AppCode.Proxy;
using System.Collections.Generic;

namespace KKday.API.WMS.Models.Repository.Common
{
    public class CommonRepository
    {

        /// <summary>
        /// Gets the currency.
        /// </summary>
        /// <returns>The currency.</returns>
        /// <param name="locale">Locale.</param>
        public static CurrencyModel GetCurrency(string locale)
        {
            CurrencyModel currency = new CurrencyModel();
            //JObject result = new JObject();
            JObject obj = null;
            try
            {

                //obj = CommonDAL.GetCurrency(locale);
                obj = CurrencyProxy.GetCurrency(locale);


                //if (obj != null && obj.Count > 0)
                if (obj["content"]["result"].ToString() == "0000")
                {
                    //result.Add("result","200");
                    //result.Add("result_msg", "OK");
                    //result.Add("currencyList", (JArray)obj["Table"]);

                    //JArray currencyList = (JArray)obj["Table"];



                    //Json ms = new Json()
                    //{
                    //    currency.result = obj["content"]["result"].ToString(),
                    //    msg = obj["content"]["msg"].ToString()
                    //};

                    //currency.content = ms;
                    //currency.content.codeList = new List<Json2>();

                    //JArray codelst = (JArray)obj["content"]["codeList"];
                    //Json2 ms2 = new Json2();
                    //Json3 ms3 = new Json3();

                    //for (int i = 0; i < codelst.Count ; i++ ){
                    //    ms2 = new Json2();
                    //    ms3 = new Json3();
                    //    ms3.dataCd = (string)codelst[i]["code"]["dataCd"];
                    //    ms3.dataName = (string)codelst[i]["code"]["dataName"];
                    //    ms3.param1 = (string)codelst[i]["code"]["param1"];
                    //    ms2.code = ms3;
                    //    currency.content.codeList.Add(ms2);


                    //}

                    currency.currencyList = new List<Json>();
                    Json Jlist = new Json();
                    currency.result = obj["content"]["result"].ToString();
                    currency.result_msg = obj["content"]["msg"].ToString();

                    JArray codelst = (JArray)obj["content"]["codeList"];
                    foreach (var i in codelst)
                    {
                        Jlist = new Json()
                        {
                            currency = (string)i["code"]["dataCd"],
                            name = (string)i["code"]["dataName"],
                            param1 = (string)i["code"]["param1"]
                        };

                        currency.currencyList.Add(Jlist);
                    }


                }
                else
                {
                    //若找不到該語系的幣別 則回傳找不到的訊息 
                    //result.Add("result", "404");
                    //result.Add("result_msg", "currencyList not found!");

                    currency.result = obj["content"]["result"].ToString();
                    currency.result_msg = $"kkday package api response msg is not correct! {obj["content"]["msg"].ToString()}";
                    throw new Exception($"kkday currency api response msg is not correct! {obj["content"]["msg"].ToString()}");
                }

            }
            catch (Exception ex)
            {

                Website.Instance.logger.FatalFormat($"GetCurrency  Error :{ex.Message},{ex.StackTrace}");

                throw ex;

            }

            return currency;

        }

        public static ProductCountryCityModel GetProductCountryCity(KKdayApiCurrencyRQModel queryRQ)
        {
            ProductCountryCityModel countryCity = new ProductCountryCityModel();

            JObject obj = null;
            countryCity.content = new ProductCountryCity();

            try
            {

                obj = CurrencyProxy.GetProductCountryCity(queryRQ);

                if (obj["content"]["result"].ToString() == "0000")
                {
                    countryCity.content.countryList = new List<Country>();

                    Country country = new Country();
                    country.cityList = new List<City>();
                    City city = new City();

                    countryCity.content.result = obj["content"]["result"].ToString();
                    countryCity.content.msg = obj["content"]["msg"].ToString();

                    JArray countryList = null;
                    JArray cityList = null;

                    if (obj["content"]["countryList"] != null)
                    {
                        countryList = (JArray)obj["content"]["countryList"];
                        foreach (var i in countryList)
                        {
                            country = new Country()
                            {
                                countryCd = (string)i["countryCd"],
                                countryName = (string)i["countryName"]
                            };

                            if (i["cityList"] != null)
                            {
                                country.cityList = new List<City>();
                                cityList = (JArray)i["cityList"];
                                foreach (var j in cityList)
                                {
                                    city = new City()
                                    {
                                        cityCd = (string)j["cityCd"],
                                        cityName = (string)j["cityName"]
                                    };

                                    country.cityList.Add(city);
                                }
                            }


                            countryCity.content.countryList.Add(country);
                        }
                    }


                }
                else
                {
                    countryCity.content.result = obj["content"]["result"].ToString();
                    countryCity.content.msg = $"kkday package api response msg is not correct! {obj["content"]["msg"].ToString()}";
                    throw new Exception($"kkday currency api response msg is not correct! {obj["content"]["msg"].ToString()}");
                }

                }
            catch (Exception ex)
            {

                Website.Instance.logger.FatalFormat($"GetProductCountryCity  Error :{ex.Message},{ex.StackTrace}");

                throw ex;

            }


            return countryCity;
        }
    }
}
