using System;
using System.Collections.Generic;
using KKday.Web.B2D.BE.Commons;
using KKday.Web.B2D.BE.Models.Model.Common;

namespace KKday.Web.B2D.BE.Models.Repository
{
    public class CountryRepository
    {
        public List<CountryArea> GetCountryAreas(string locale)
        {
            return CommonProxy.GetCountryList(locale);
        }

        public List<CountryLocale> GetCountryLocales()
        {
            return CommonProxy.GetCountryLocales(); ;
        }
    }
}
