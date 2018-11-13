using System;
using System.Data;
using KKday.API.WMS.AppCode.DAL;
using Newtonsoft.Json.Linq;
using KKday.API.WMS.Models.DataModel.Common;
using KKday.API.WMS.AppCode.Proxy;
using System.Collections.Generic;
using System.Linq;


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

                obj = CommonProxy.getCurrency(locale);


                //if (obj != null && obj.Count > 0)
                if (obj["content"]["result"].ToString() == "0000")
                {

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

                Website.Instance.logger.FatalFormat($"getCurrency  Error :{ex.Message},{ex.StackTrace}");


                throw ex;

            }

            return currency;

        }


        public static GuideLanguageModel GetGuideLanguage()
        {
            GuideLanguageModel lang = new GuideLanguageModel();

            JObject obj = null;
            try
            {
                obj = CommonProxy.getGuideLang();
                if (obj["content"]["result"].ToString() == "0000")
                {
                    lang.result = obj["content"]["result"].ToString();
                    lang.result_msg = obj["content"]["msg"].ToString();

                    List<langList> list = new List<langList>();
                    langList Jlist = new langList();
                    
                    JArray codelst = (JArray)obj["content"]["countryLangList"];
                    foreach (var items in codelst)
                    {
                        foreach (var x in items["langList"])
                        {
                            Jlist = x.ToObject<langList>();
                            list.Add(Jlist);
                        }
                    }
                    //list 排除重複語法
                    lang.lang_list = list.GroupBy(x => x.langCd).Select(y =>y.First()).ToList();
                    
                }
                else
                {

                    lang.result = obj["content"]["result"].ToString();
                    lang.result_msg = $"kkday guide lang api response msg is not correct! {obj["content"]["msg"].ToString()}";
                    throw new Exception($"kkday guide lang api response msg is not correct! {obj["content"]["msg"].ToString()}");
                }
            }
            catch (Exception ex)
            {
                Website.Instance.logger.FatalFormat($"GetProductCountryCity  Error :{ex.Message},{ex.StackTrace}");
                throw ex;
            }
            return lang;
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

            }
            catch (Exception ex)
            {

                Website.Instance.logger.FatalFormat($"getCurrency  Error :{ex.Message},{ex.StackTrace}");

                    countryCity.content.result = obj["content"]["result"].ToString();
                    countryCity.content.msg = $"kkday package api response msg is not correct! {obj["content"]["msg"].ToString()}";
                    throw new Exception($"kkday currency api response msg is not correct! {obj["content"]["msg"].ToString()}");
            }

            
            return countryCity;

        }
    }
}
