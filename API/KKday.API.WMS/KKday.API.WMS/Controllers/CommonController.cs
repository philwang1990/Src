using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using KKday.API.WMS.Models.Repository.Common;
using KKday.API.WMS.Models.DataModel.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;//IsoDateTimeConverter

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace KKday.API.WMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommonController : Controller
    {
        /// <summary>
        /// Currency the specified locale.
        /// </summary>
        /// <returns>The currency.</returns>
        /// <param name="locale">Locale.</param>
        [HttpGet("Currency")]
        public CurrencyModel Currency(string locale)
        {
            CurrencyModel currency = new CurrencyModel();

            try
            {
                Website.Instance.logger.Info($"WMS Currency Start! B2D locale:{locale}");

                currency = CommonRepository.GetCurrency(locale);


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return currency;
        }

        [HttpGet("GuideLanguage")]
        public GuideLanguageModel GuideLanguage()
        {
            GuideLanguageModel lang = new GuideLanguageModel();
            try
            {
                Website.Instance.logger.Info("WMS GuideLanguageModel Start!");
                lang = CommonRepository.GetGuideLanguage();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lang;
        }
    }
}
