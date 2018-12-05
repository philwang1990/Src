using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KKday.Web.B2D.BE.Pages.Shared
{
    public class IndexModel : PageModel
    {
        public int Code { get; set; }

        //
        public void OnGet(int? code)
        {
            this.Code = (code.HasValue == true) ? code.Value : -1;
        }
    }
}
