using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KKday.Web.B2D.BE.Pages.Errors
{
    public class IndexModel : PageModel
    {
        public int Code { get; set; }

        //
        public void OnGet(int? status)
        {
            this.Code = (status.HasValue == true) ? status.Value : -1;

            if(HttpContext.User != null && HttpContext.User.Identity != null) 
            {
                if (HttpContext.User.Claims.Where(c => c.Type == "UserType").Count() > 0)
                {
                    var UseType = ((ClaimsIdentity)HttpContext.User.Identity).FindFirst("UserType").Value;
                    ViewData["HOME_PAGE"] = Url.Content("~/" + (UseType.Equals("KKDAY") ? "KKday" : "User"));
                }
            }
        }
    }
}
