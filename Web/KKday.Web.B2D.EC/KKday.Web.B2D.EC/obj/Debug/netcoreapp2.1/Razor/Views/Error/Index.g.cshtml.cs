#pragma checksum "/Users/phil/Desktop/Git/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/Error/Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "dcabaea11835c6d713c1db99f254b250eb6b866f"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Error_Index), @"mvc.1.0.view", @"/Views/Error/Index.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Error/Index.cshtml", typeof(AspNetCore.Views_Error_Index))]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"dcabaea11835c6d713c1db99f254b250eb6b866f", @"/Views/Error/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"a1decf7227a59ebd0c9a1b3569a3e4065089e3cb", @"/Views/_ViewImports.cshtml")]
    public class Views_Error_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#line 1 "/Users/phil/Desktop/Git/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/Error/Index.cshtml"
  
    string invalid_title = ViewBag.invalid_title as string;
    string invalid_info = ViewBag.invalid_info as string;
    string common_more_experiences = ViewBag.common_more_experiences as string;

#line default
#line hidden
            BeginContext(208, 173, true);
            WriteLiteral("\r\n<section class=\"wrapper\" id=\"invalidProductApp\">\r\n    <div class=\"container\">\r\n        <div class=\"empty-list\">\r\n            <div class=\"none-title\">\r\n                <img");
            EndContext();
            BeginWriteAttribute("src", " src=\"", 381, "\"", 441, 1);
#line 11 "/Users/phil/Desktop/Git/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/Error/Index.cshtml"
WriteAttributeValue("", 387, Url.Content("/images/empty_state/product_detail.svg"), 387, 54, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(442, 24, true);
            WriteLiteral(">\r\n                <h3> ");
            EndContext();
            BeginContext(467, 13, false);
#line 12 "/Users/phil/Desktop/Git/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/Error/Index.cshtml"
                Write(invalid_title);

#line default
#line hidden
            EndContext();
            BeginContext(480, 46, true);
            WriteLiteral(" </h3>\r\n                <p class=\"none-hint\"> ");
            EndContext();
            BeginContext(527, 12, false);
#line 13 "/Users/phil/Desktop/Git/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/Error/Index.cshtml"
                                 Write(invalid_info);

#line default
#line hidden
            EndContext();
            BeginContext(539, 65, true);
            WriteLiteral(" </p>\r\n            </div>\r\n        </div>\r\n    </div>\r\n</section>");
            EndContext();
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
