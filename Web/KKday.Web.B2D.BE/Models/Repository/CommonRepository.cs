using System;
using System.Collections.Generic;
using KKday.Web.B2D.BE.Commons;
using KKday.Web.B2D.BE.Models.Model.Common;
using Microsoft.Extensions.Caching.Memory;

namespace KKday.Web.B2D.BE.Models.Repository
{
    public class CommonRepository
    {
        private readonly IMemoryCache _memoryCache;
        private string COUNTRY_AERAS_KEY = "COUNTRY_AREAS_LOCALE_";
        private string CLUTURE_LOCALES_KEY = "CLUTURE_LOCALES";
        private string CURRENCY_LOCALES_KEY = "CURRENCY_LOCALES";

        public CommonRepository(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public List<CountryArea> GetCountryAreas(string locale)
        { 
            var countryAreas = _memoryCache.Get<List<CountryArea>>(COUNTRY_AERAS_KEY + locale);
            if(countryAreas == null) {
                countryAreas = CommonProxy.GetCountryAreas(locale);
                // 保留本機 24HR
                _memoryCache.Set(COUNTRY_AERAS_KEY + locale, countryAreas, new TimeSpan(24,0,0));
            }

            return countryAreas;
        }

        public List<CountryLocale> GetCountryLocales()
        {
            var locales = _memoryCache.Get<List<CountryLocale>>(CLUTURE_LOCALES_KEY);
            if (locales == null)
            {
                locales = CommonProxy.GetCountryLocales();
                // 保留本機 24HR
                _memoryCache.Set(CLUTURE_LOCALES_KEY, locales, new TimeSpan(24, 0, 0));
            }

            return locales;
        }

        public Dictionary<string, string> GetCurrencies(string locale)
        {
            var currency_dict = _memoryCache.Get<Dictionary<string, string>>(CURRENCY_LOCALES_KEY + locale);
            if (currency_dict == null)
            {
                currency_dict = CommonProxy.GetCurrencies(locale);
                // 保留本機 24HR
                _memoryCache.Set(CURRENCY_LOCALES_KEY + locale, currency_dict, new TimeSpan(24, 0, 0));
            }

            return currency_dict;
        }

    }
}
