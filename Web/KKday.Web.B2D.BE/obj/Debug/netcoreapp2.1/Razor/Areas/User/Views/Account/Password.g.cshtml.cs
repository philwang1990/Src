#pragma checksum "/Users/cengshenghao/Documents/GitHub/Src/Web/KKday.Web.B2D.BE/Areas/User/Views/Account/Password.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "517d2dd02bc5d96defe7b58c83d5515ac9a10639"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Areas_User_Views_Account_Password), @"mvc.1.0.view", @"/Areas/User/Views/Account/Password.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Areas/User/Views/Account/Password.cshtml", typeof(AspNetCore.Areas_User_Views_Account_Password))]
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
#line 1 "/Users/cengshenghao/Documents/GitHub/Src/Web/KKday.Web.B2D.BE/Areas/User/Views/_ViewImports.cshtml"
using KKday.Web.B2D.BE;

#line default
#line hidden
#line 2 "/Users/cengshenghao/Documents/GitHub/Src/Web/KKday.Web.B2D.BE/Areas/User/Views/_ViewImports.cshtml"
using KKday.Web.B2D.BE.Areas.User.Models;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"517d2dd02bc5d96defe7b58c83d5515ac9a10639", @"/Areas/User/Views/Account/Password.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"bc7bac046ff07d4ef2ecf2ecf6161a63e44be55d", @"/Areas/User/Views/_ViewImports.cshtml")]
    public class Areas_User_Views_Account_Password : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("method", "post", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("id", new global::Microsoft.AspNetCore.Html.HtmlString("form1"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("onsubmit", new global::Microsoft.AspNetCore.Html.HtmlString("return CheckPassword();"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
            BeginContext(0, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 2 "/Users/cengshenghao/Documents/GitHub/Src/Web/KKday.Web.B2D.BE/Areas/User/Views/Account/Password.cshtml"
  
    Layout = "_LayoutMain";

#line default
#line hidden
            BeginContext(38, 98, true);
            WriteLiteral("<nav aria-label=\"breadcrumb\">\r\n    <ol class=\"breadcrumb\">\r\n        <li class=\"breadcrumb-item\"><a");
            EndContext();
            BeginWriteAttribute("href", " href=\"", 136, "\"", 161, 1);
#line 7 "/Users/cengshenghao/Documents/GitHub/Src/Web/KKday.Web.B2D.BE/Areas/User/Views/Account/Password.cshtml"
WriteAttributeValue("", 143, Url.Content("~/"), 143, 18, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(162, 435, true);
            WriteLiteral(@">Home</a></li>
        <li class=""breadcrumb-item active"" aria-current=""page"">更改密碼</li>
    </ol>
</nav>
<div class=""container"">
    <div class=""row"">
        <div class=""col-xs-12"">
            <h4>更改密碼</h4>
        </div>

        <div class=""row"">
            <div class=""col-xs-12 col-sm-3 col-sm-offset-3"">
                <p class=""text-center"">Your password cannot be the same as your username.</p>
                ");
            EndContext();
            BeginContext(597, 651, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "09dd8077882841ed955437856341fdd2", async() => {
                BeginContext(663, 64, true);
                WriteLiteral("\r\n                    <input type=\"hidden\" id=\"uuid\" name=\"uuid\"");
                EndContext();
                BeginWriteAttribute("value", " value=\"", 727, "\"", 752, 1);
#line 21 "/Users/cengshenghao/Documents/GitHub/Src/Web/KKday.Web.B2D.BE/Areas/User/Views/Account/Password.cshtml"
WriteAttributeValue("", 735, ViewData["UUID"], 735, 17, false);

#line default
#line hidden
                EndWriteAttribute();
                BeginContext(753, 488, true);
                WriteLiteral(@" />
                    <input type=""password"" class=""form-control"" style=""margin-bottom:8px;"" name=""password"" id=""password1"" placeholder=""New Password"" autocomplete=""off"">
                    <input type=""password"" class=""form-control"" style=""margin-bottom:8px;"" id=""password2"" placeholder=""Repeat Password"" autocomplete=""off"">
                    <input type=""submit"" class=""col-xs-12 btn btn-primary btn-load"" data-loading-text=""Changing Password..."" value=""變更密碼"">
                ");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Method = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(1248, 759, true);
            WriteLiteral(@"
            </div><!--/col-sm-6-->
        </div><!--/row-->
    </div>
</div>

<script type=""text/javascript""> 
    function CheckPassword() { 

      $.ajax({
            url: _root_path + 'Account/UpdatePassword',
            type: 'POST',
            data: {
              uuid: $('#uuid').val(), password: $('#password1').val()
            },
            error: function(xhr) {
              alert('Ajax request 發生錯誤');
            },
            success: function(result) {
                if(result.status == ""OK"") {
                    alert(""密碼已變更成功!""); 
                }
                else {
                    alert(result.msg);
                }
            }
          });
          return false;
    }
</script>");
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
