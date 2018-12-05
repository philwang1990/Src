using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Resources;
using System.Globalization;
using System.Text.RegularExpressions;

namespace KKday.Web.B2D.BE.Filters
{
    public class CultureFilter : IResourceFilter
    {
        private readonly ILocalizer _localizer;

        public CultureFilter(ILocalizer localizer)
        {
            _localizer = localizer;
        }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            string culture = context.HttpContext.Request.Query["lang"];

            // 取得 Cookie 內的 Culture
            string _cultureInCookie = "";
            if (string.IsNullOrEmpty(culture))
            {
                if (!context.HttpContext.Request.Cookies.TryGetValue("b2d.culture", out _cultureInCookie))
                {
                    _cultureInCookie = "zh-TW";
                }
                culture = _cultureInCookie;
            }

            var hasCultureFromUrl = Regex.IsMatch(culture, @"^[A-Za-z]{2}-[A-Za-z]{2}$");
            _localizer.Culture = hasCultureFromUrl ? culture : CultureInfo.CurrentCulture.Name;

            // 更新 Cookie 內的 Culture
            context.HttpContext.Response.Cookies.Append("b2d.culture", culture);
        }

        public void OnResourceExecuted(ResourceExecutedContext context)
        {
        }
    }
}