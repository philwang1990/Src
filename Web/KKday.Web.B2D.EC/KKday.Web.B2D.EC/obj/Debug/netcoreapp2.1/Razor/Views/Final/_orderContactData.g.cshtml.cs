#pragma checksum "/Users/phil/Desktop/Git/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/Final/_orderContactData.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "33724f5ad14cf9edb45326ab8e72fe6cac061e24"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Final__orderContactData), @"mvc.1.0.view", @"/Views/Final/_orderContactData.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Final/_orderContactData.cshtml", typeof(AspNetCore.Views_Final__orderContactData))]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#line 1 "/Users/phil/Desktop/Git/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/_ViewImports.cshtml"
using KKday.Web.B2D.EC;

#line default
#line hidden
#line 2 "/Users/phil/Desktop/Git/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/_ViewImports.cshtml"
using KKday.Web.B2D.EC.Models;

#line default
#line hidden
#line 1 "/Users/phil/Desktop/Git/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/Final/_orderContactData.cshtml"
using KKday.Web.B2D.EC.Models.Model.Product;

#line default
#line hidden
#line 2 "/Users/phil/Desktop/Git/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/Final/_orderContactData.cshtml"
using KKday.Web.B2D.EC.Models.Model.Booking;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"33724f5ad14cf9edb45326ab8e72fe6cac061e24", @"/Views/Final/_orderContactData.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"a1decf7227a59ebd0c9a1b3569a3e4065089e3cb", @"/Views/_ViewImports.cshtml")]
    public class Views_Final__orderContactData : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(92, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 4 "/Users/phil/Desktop/Git/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/Final/_orderContactData.cshtml"
  
    var prodTitle = (ProdTitleModel)ViewData["prodTitle"];
    var orderData = (DataModel)ViewData["orderData"];
    var chkSuccess = (Boolean)ViewData["chkSuccess"];

#line default
#line hidden
            BeginContext(271, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 10 "/Users/phil/Desktop/Git/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/Final/_orderContactData.cshtml"
 if (orderData != null)
{

#line default
#line hidden
            BeginContext(301, 33, true);
            WriteLiteral("    <div class=\"title\">\r\n        ");
            EndContext();
            BeginContext(335, 31, false);
#line 13 "/Users/phil/Desktop/Git/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/Final/_orderContactData.cshtml"
   Write(prodTitle.booking_step1_contact);

#line default
#line hidden
            EndContext();
            BeginContext(366, 317, true);
            WriteLiteral(@"
        <a href=""javascript:;"" class=""title-tool expand-info"">
            <i class=""fa fa-angle-down""></i>
        </a>
    </div>
    <div class=""con"">
        <div class=""row"">
            <div class=""col-md-4 col-sm-6 col-xs-12"">
                <div class=""form-group must"">
                    <label>");
            EndContext();
            BeginContext(684, 41, false);
#line 22 "/Users/phil/Desktop/Git/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/Final/_orderContactData.cshtml"
                      Write(prodTitle.booking_step1_contact_firstname);

#line default
#line hidden
            EndContext();
            BeginContext(725, 30, true);
            WriteLiteral("</label>\r\n                    ");
            EndContext();
            BeginContext(756, 26, false);
#line 23 "/Users/phil/Desktop/Git/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/Final/_orderContactData.cshtml"
               Write(orderData.contactFirstname);

#line default
#line hidden
            EndContext();
            BeginContext(782, 175, true);
            WriteLiteral("\r\n                </div>\r\n            </div>\r\n            <div class=\"col-md-4 col-sm-6 col-xs-12\">\r\n                <div class=\"form-group must\">\r\n                    <label>");
            EndContext();
            BeginContext(958, 40, false);
#line 28 "/Users/phil/Desktop/Git/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/Final/_orderContactData.cshtml"
                      Write(prodTitle.booking_step1_contact_lastname);

#line default
#line hidden
            EndContext();
            BeginContext(998, 30, true);
            WriteLiteral("</label>\r\n                    ");
            EndContext();
            BeginContext(1029, 25, false);
#line 29 "/Users/phil/Desktop/Git/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/Final/_orderContactData.cshtml"
               Write(orderData.contactLastname);

#line default
#line hidden
            EndContext();
            BeginContext(1054, 218, true);
            WriteLiteral("\r\n                </div>\r\n            </div>\r\n        </div>\r\n        <div class=\"row\">\r\n            <div class=\"col-md-4 col-sm-6 col-xs-12\">\r\n                <div class=\"form-group must\">\r\n                    <label>");
            EndContext();
            BeginContext(1273, 28, false);
#line 36 "/Users/phil/Desktop/Git/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/Final/_orderContactData.cshtml"
                      Write(prodTitle.common_nationality);

#line default
#line hidden
            EndContext();
            BeginContext(1301, 30, true);
            WriteLiteral("</label>\r\n                    ");
            EndContext();
            BeginContext(1332, 26, false);
#line 37 "/Users/phil/Desktop/Git/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/Final/_orderContactData.cshtml"
               Write(orderData.contactCountryCd);

#line default
#line hidden
            EndContext();
            BeginContext(1358, 175, true);
            WriteLiteral("\r\n                </div>\r\n            </div>\r\n            <div class=\"col-md-4 col-sm-6 col-xs-12\">\r\n                <div class=\"form-group must\">\r\n                    <label>");
            EndContext();
            BeginContext(1534, 35, false);
#line 42 "/Users/phil/Desktop/Git/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/Final/_orderContactData.cshtml"
                      Write(prodTitle.booking_step1_contact_tel);

#line default
#line hidden
            EndContext();
            BeginContext(1569, 30, true);
            WriteLiteral("</label>\r\n                    ");
            EndContext();
            BeginContext(1600, 20, false);
#line 43 "/Users/phil/Desktop/Git/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/Final/_orderContactData.cshtml"
               Write(orderData.contactTel);

#line default
#line hidden
            EndContext();
            BeginContext(1620, 209, true);
            WriteLiteral("\r\n                </div>\r\n            </div>\r\n        </div>\r\n        <div class=\"row\">\r\n            <div class=\"col-md-8 col-xs-12\">\r\n                <div class=\"form-group must\">\r\n                    <label>");
            EndContext();
            BeginContext(1830, 37, false);
#line 50 "/Users/phil/Desktop/Git/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/Final/_orderContactData.cshtml"
                      Write(prodTitle.booking_step1_contact_email);

#line default
#line hidden
            EndContext();
            BeginContext(1867, 30, true);
            WriteLiteral("</label>\r\n                    ");
            EndContext();
            BeginContext(1898, 22, false);
#line 51 "/Users/phil/Desktop/Git/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/Final/_orderContactData.cshtml"
               Write(orderData.contactEmail);

#line default
#line hidden
            EndContext();
            BeginContext(1920, 62, true);
            WriteLiteral("\r\n                </div>\r\n            </div>\r\n        </div>\r\n");
            EndContext();
#line 55 "/Users/phil/Desktop/Git/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/Final/_orderContactData.cshtml"
         if (chkSuccess ==true)
        {

#line default
#line hidden
            BeginContext(2025, 97, true);
            WriteLiteral("            <div class=\"mt-15\">\r\n                <a :href=\"orderShowUrl\" class=\"btn btn-primary\">");
            EndContext();
            BeginContext(2123, 36, false);
#line 58 "/Users/phil/Desktop/Git/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/Final/_orderContactData.cshtml"
                                                           Write(prodTitle.booking_step3_order_detail);

#line default
#line hidden
            EndContext();
            BeginContext(2159, 26, true);
            WriteLiteral("</a>\r\n            </div>\r\n");
            EndContext();
#line 60 "/Users/phil/Desktop/Git/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/Final/_orderContactData.cshtml"
        }

#line default
#line hidden
            BeginContext(2196, 12, true);
            WriteLiteral("    </div>\r\n");
            EndContext();
#line 62 "/Users/phil/Desktop/Git/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/Final/_orderContactData.cshtml"
}

#line default
#line hidden
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
