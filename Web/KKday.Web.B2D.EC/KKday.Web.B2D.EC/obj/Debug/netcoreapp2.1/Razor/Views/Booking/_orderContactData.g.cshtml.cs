#pragma checksum "/Users/caizhiliang/Documents/GitHub/erichu62/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/Booking/_orderContactData.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "d140a39c9f27f9fd0257400d03abdfa7241369cb"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Booking__orderContactData), @"mvc.1.0.view", @"/Views/Booking/_orderContactData.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Booking/_orderContactData.cshtml", typeof(AspNetCore.Views_Booking__orderContactData))]
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
#line 1 "/Users/caizhiliang/Documents/GitHub/erichu62/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/_ViewImports.cshtml"
using KKday.Web.B2D.EC;

#line default
#line hidden
#line 2 "/Users/caizhiliang/Documents/GitHub/erichu62/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/_ViewImports.cshtml"
using KKday.Web.B2D.EC.Models;

#line default
#line hidden
#line 1 "/Users/caizhiliang/Documents/GitHub/erichu62/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/Booking/_orderContactData.cshtml"
using KKday.Web.B2D.EC.Models.Model.Product;

#line default
#line hidden
#line 2 "/Users/caizhiliang/Documents/GitHub/erichu62/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/Booking/_orderContactData.cshtml"
using KKday.Web.B2D.EC.Models.Model.Booking;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"d140a39c9f27f9fd0257400d03abdfa7241369cb", @"/Views/Booking/_orderContactData.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"a1decf7227a59ebd0c9a1b3569a3e4065089e3cb", @"/Views/_ViewImports.cshtml")]
    public class Views_Booking__orderContactData : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("id", new global::Microsoft.AspNetCore.Html.HtmlString("form1"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(92, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 4 "/Users/caizhiliang/Documents/GitHub/erichu62/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/Booking/_orderContactData.cshtml"
  
    var prodTitle = (ProdTitleModel)ViewData["prodTitle"];
    var totalCus = (int)ViewData["totalCus"];
    var cusData = (CusData)ViewData["cusData"];
    var contactInfo = (distributorInfo)ViewData["contactInfo"];


#line default
#line hidden
            BeginContext(324, 2, true);
            WriteLiteral("\r\n");
            EndContext();
            BeginContext(326, 2692, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "ecd3756d3eae4cc6a227e88d26b2ea42", async() => {
                BeginContext(343, 121, true);
                WriteLiteral("\r\n    <div class=\"row\">\r\n        <div class=\"col-md-12\">\r\n            <div class=\"col-md-4\">\r\n                <h3><label>");
                EndContext();
                BeginContext(465, 31, false);
#line 16 "/Users/caizhiliang/Documents/GitHub/erichu62/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/Booking/_orderContactData.cshtml"
                      Write(prodTitle.booking_step1_contact);

#line default
#line hidden
                EndContext();
                BeginContext(496, 173, true);
                WriteLiteral("</label>  </h3>\r\n            </div>\r\n        </div>\r\n    </div>\r\n    <div class=\"row\">\r\n        <div class=\"col-md-12\">\r\n            <div class=\"col-md-4\">\r\n                ");
                EndContext();
                BeginContext(670, 41, false);
#line 23 "/Users/caizhiliang/Documents/GitHub/erichu62/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/Booking/_orderContactData.cshtml"
           Write(prodTitle.booking_step1_contact_firstname);

#line default
#line hidden
                EndContext();
                BeginContext(711, 76, true);
                WriteLiteral("\r\n            </div>\r\n            <div class=\"col-md-4\">\r\n\r\n                ");
                EndContext();
                BeginContext(788, 40, false);
#line 27 "/Users/caizhiliang/Documents/GitHub/erichu62/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/Booking/_orderContactData.cshtml"
           Write(prodTitle.booking_step1_contact_lastname);

#line default
#line hidden
                EndContext();
                BeginContext(828, 237, true);
                WriteLiteral("\r\n            </div>\r\n        </div>\r\n    </div>\r\n    <div class=\"row\">\r\n        <div class=\"col-md-12\">\r\n            <div class=\"col-md-4\">\r\n                <input type=\"text\" class=\"form-control\" id=\"txtLocalFname\" name=\"txtLocalFname\"");
                EndContext();
                BeginWriteAttribute("value", "  value=\"", 1065, "\"", 1096, 1);
#line 34 "/Users/caizhiliang/Documents/GitHub/erichu62/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/Booking/_orderContactData.cshtml"
WriteAttributeValue("", 1074, contactInfo.firstName, 1074, 22, false);

#line default
#line hidden
                EndWriteAttribute();
                BeginWriteAttribute("placeholder", " placeholder=\"", 1097, "\"", 1163, 1);
#line 34 "/Users/caizhiliang/Documents/GitHub/erichu62/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/Booking/_orderContactData.cshtml"
WriteAttributeValue("", 1111, prodTitle.booking_step1_local_firstname_placeholder, 1111, 52, false);

#line default
#line hidden
                EndWriteAttribute();
                BeginContext(1164, 163, true);
                WriteLiteral(" readonly>\r\n            </div>\r\n            <div class=\"col-md-4\">\r\n                <input type=\"text\" class=\"form-control\" id=\"txtLocalLname\" name=\"txtLocalLname\"");
                EndContext();
                BeginWriteAttribute("value", " value=\"", 1327, "\"", 1356, 1);
#line 37 "/Users/caizhiliang/Documents/GitHub/erichu62/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/Booking/_orderContactData.cshtml"
WriteAttributeValue("", 1335, contactInfo.lastName, 1335, 21, false);

#line default
#line hidden
                EndWriteAttribute();
                BeginWriteAttribute("placeholder", " placeholder=\"", 1357, "\"", 1422, 1);
#line 37 "/Users/caizhiliang/Documents/GitHub/erichu62/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/Booking/_orderContactData.cshtml"
WriteAttributeValue("", 1371, prodTitle.booking_step1_local_lastname_placeholder, 1371, 51, false);

#line default
#line hidden
                EndWriteAttribute();
                BeginContext(1423, 168, true);
                WriteLiteral(" readonly>\r\n            </div>\r\n        </div>\r\n    </div>\r\n    <div class=\"row\">\r\n        <div class=\"col-md-12\">\r\n            <div class=\"col-md-4\">\r\n                ");
                EndContext();
                BeginContext(1592, 28, false);
#line 44 "/Users/caizhiliang/Documents/GitHub/erichu62/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/Booking/_orderContactData.cshtml"
           Write(prodTitle.common_nationality);

#line default
#line hidden
                EndContext();
                BeginContext(1620, 76, true);
                WriteLiteral("\r\n            </div>\r\n            <div class=\"col-md-4\">\r\n\r\n                ");
                EndContext();
                BeginContext(1697, 35, false);
#line 48 "/Users/caizhiliang/Documents/GitHub/erichu62/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/Booking/_orderContactData.cshtml"
           Write(prodTitle.booking_step1_contact_tel);

#line default
#line hidden
                EndContext();
                BeginContext(1732, 255, true);
                WriteLiteral("\r\n            </div>\r\n        </div>\r\n    </div>\r\n    <div class=\"row\">\r\n        <div class=\"col-md-12\">\r\n            <div class=\"col-md-4\" >\r\n                <input type=\"text\" class=\"form-control\" id=\"txtNationality\" name=\"txtNationality\" placeholder=\"\"");
                EndContext();
                BeginWriteAttribute("value", "  value=\"", 1987, "\"", 2018, 1);
#line 55 "/Users/caizhiliang/Documents/GitHub/erichu62/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/Booking/_orderContactData.cshtml"
WriteAttributeValue("", 1996, contactInfo.countryCd, 1996, 22, false);

#line default
#line hidden
                EndWriteAttribute();
                BeginContext(2019, 158, true);
                WriteLiteral(" readonly >\r\n            </div>\r\n            <div class=\"col-md-4\">\r\n                <input type=\"text\" class=\"form-control\" id=\"txtContactTel\" placeholder=\"\"");
                EndContext();
                BeginWriteAttribute("value", " value=\"", 2177, "\"", 2226, 4);
                WriteAttributeValue("", 2185, "$(", 2185, 2, true);
#line 58 "/Users/caizhiliang/Documents/GitHub/erichu62/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/Booking/_orderContactData.cshtml"
WriteAttributeValue("", 2187, contactInfo.areatel, 2187, 20, false);

#line default
#line hidden
#line 58 "/Users/caizhiliang/Documents/GitHub/erichu62/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/Booking/_orderContactData.cshtml"
WriteAttributeValue("  ", 2207, contactInfo.tel, 2209, 16, false);

#line default
#line hidden
                WriteAttributeValue("", 2225, ")", 2225, 1, true);
                EndWriteAttribute();
                BeginContext(2227, 168, true);
                WriteLiteral(" readonly>\r\n            </div>\r\n        </div>\r\n    </div>\r\n    <div class=\"row\">\r\n        <div class=\"col-md-12\">\r\n            <div class=\"col-md-8\">\r\n                ");
                EndContext();
                BeginContext(2396, 37, false);
#line 65 "/Users/caizhiliang/Documents/GitHub/erichu62/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/Booking/_orderContactData.cshtml"
           Write(prodTitle.booking_step1_contact_email);

#line default
#line hidden
                EndContext();
                BeginContext(2433, 226, true);
                WriteLiteral("\r\n            </div>\r\n        </div>\r\n    </div>\r\n    <div class=\"row\">\r\n        <div class=\"col-md-12\">\r\n            <div class=\"col-md-4\">\r\n                <input type=\"text\" class=\"form-control\" id=\"txtEmail\" placeholder=\"\"");
                EndContext();
                BeginWriteAttribute("value", " value=\"", 2659, "\"", 2685, 1);
#line 72 "/Users/caizhiliang/Documents/GitHub/erichu62/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/Booking/_orderContactData.cshtml"
WriteAttributeValue("", 2667, contactInfo.email, 2667, 18, false);

#line default
#line hidden
                EndWriteAttribute();
                BeginContext(2686, 62, true);
                WriteLiteral(" readonly>\r\n            </div>\r\n\r\n        </div>\r\n    </div>\r\n");
                EndContext();
                BeginContext(3000, 11, true);
                WriteLiteral("\r\n     \r\n  ");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
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
