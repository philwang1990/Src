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
            culture = string.IsNullOrEmpty(culture) ? "zh-TW" : culture;
            var hasCultureFromUrl = Regex.IsMatch(culture, @"^[A-Za-z]{2}-[A-Za-z]{2}$");
            _localizer.Culture = hasCultureFromUrl ? culture : CultureInfo.CurrentCulture.Name;
        }

        public void OnResourceExecuted(ResourceExecutedContext context)
        {
        }
    }
}