#pragma checksum "/Users/phil/Desktop/Git/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/Booking/_guide.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "9870065f8f64daaf2100d138e8d40040e729bbba"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Booking__guide), @"mvc.1.0.view", @"/Views/Booking/_guide.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Booking/_guide.cshtml", typeof(AspNetCore.Views_Booking__guide))]
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
#line 1 "/Users/phil/Desktop/Git/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/Booking/_guide.cshtml"
using KKday.Web.B2D.EC.Models.Model.Product;

#line default
#line hidden
#line 2 "/Users/phil/Desktop/Git/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/Booking/_guide.cshtml"
using KKday.Web.B2D.EC.Models.Model.Booking;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"9870065f8f64daaf2100d138e8d40040e729bbba", @"/Views/Booking/_guide.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"a1decf7227a59ebd0c9a1b3569a3e4065089e3cb", @"/Views/_ViewImports.cshtml")]
    public class Views_Booking__guide : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("value", "null", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("selected", new global::Microsoft.AspNetCore.Html.HtmlString("selected"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(92, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 4 "/Users/phil/Desktop/Git/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/Booking/_guide.cshtml"
  
    var mainCat = (string)ViewData["mainCat"];
    var guide = (List<GuideLanguage>)ViewData["guide"];
    var prodTitle = (ProdTitleModel)ViewData["prodTitle"];

#line default
#line hidden
            BeginContext(266, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 10 "/Users/phil/Desktop/Git/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/Booking/_guide.cshtml"
 if (guide != null)
{
    

#line default
#line hidden
#line 12 "/Users/phil/Desktop/Git/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/Booking/_guide.cshtml"
     if (mainCat != "03")
    {


#line default
#line hidden
            BeginContext(326, 64, true);
            WriteLiteral("     <div class=\"traveler-con\">\r\n        <div class=\"sub-title\">");
            EndContext();
            BeginContext(391, 27, false);
#line 16 "/Users/phil/Desktop/Git/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/Booking/_guide.cshtml"
                          Write(prodTitle.common_guide_lang);

#line default
#line hidden
            EndContext();
            BeginContext(418, 159, true);
            WriteLiteral("</div>\r\n        <div class=\"row\">\r\n            <div class=\"col-md-4 col-sm-6 col-xs-12\">\r\n                <div class=\"form-group\">\r\n                    <label>");
            EndContext();
            BeginContext(578, 42, false);
#line 20 "/Users/phil/Desktop/Git/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/Booking/_guide.cshtml"
                      Write(prodTitle.booking_step1_product_guide_lang);

#line default
#line hidden
            EndContext();
            BeginContext(620, 115, true);
            WriteLiteral("</label>\r\n                    <select class=\"form-control\" id=\"selGuide\" name=\"selGuide\">\r\n                        ");
            EndContext();
            BeginContext(735, 141, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("option", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "bb453e206749421ea211a5571ff1e460", async() => {
                BeginContext(783, 30, true);
                WriteLiteral("\r\n                            ");
                EndContext();
                BeginContext(814, 27, false);
#line 23 "/Users/phil/Desktop/Git/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/Booking/_guide.cshtml"
                       Write(prodTitle.common_select_set);

#line default
#line hidden
                EndContext();
                BeginContext(841, 26, true);
                WriteLiteral("\r\n                        ");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper.Value = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            BeginWriteTagHelperAttribute();
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __tagHelperExecutionContext.AddHtmlAttribute("disabled", Html.Raw(__tagHelperStringValueBuffer), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.Minimized);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(876, 4, true);
            WriteLiteral("\r\n\r\n");
            EndContext();
#line 26 "/Users/phil/Desktop/Git/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/Booking/_guide.cshtml"
                         foreach (GuideLanguage g in guide)
                        {

#line default
#line hidden
            BeginContext(968, 25, true);
            WriteLiteral("                         ");
            EndContext();
            BeginContext(993, 107, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("option", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "4685d6ed56894c369078886cd4976b55", async() => {
                BeginContext(1022, 30, true);
                WriteLiteral("\r\n                            ");
                EndContext();
                BeginContext(1053, 11, false);
#line 29 "/Users/phil/Desktop/Git/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/Booking/_guide.cshtml"
                       Write(g.lang_name);

#line default
#line hidden
                EndContext();
                BeginContext(1064, 27, true);
                WriteLiteral("\r\n                         ");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper);
            BeginWriteTagHelperAttribute();
#line 28 "/Users/phil/Desktop/Git/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/Booking/_guide.cshtml"
                            WriteLiteral(g.lang_code);

#line default
#line hidden
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper.Value = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("value", __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper.Value, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(1100, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 31 "/Users/phil/Desktop/Git/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/Booking/_guide.cshtml"
                        }

#line default
#line hidden
            BeginContext(1129, 104, true);
            WriteLiteral("                    </select>\r\n                </div>\r\n            </div>\r\n        </div>\r\n     </div>\r\n");
            EndContext();
#line 37 "/Users/phil/Desktop/Git/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/Booking/_guide.cshtml"


    }

#line default
#line hidden
#line 39 "/Users/phil/Desktop/Git/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/Booking/_guide.cshtml"
     
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
