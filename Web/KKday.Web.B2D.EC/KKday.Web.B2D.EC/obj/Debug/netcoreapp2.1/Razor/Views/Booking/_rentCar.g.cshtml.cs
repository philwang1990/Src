#pragma checksum "/Users/phil/Desktop/Git/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/Booking/_rentCar.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "21e1c91a2d718cbc34426b70b9570a56e32ba48a"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Booking__rentCar), @"mvc.1.0.view", @"/Views/Booking/_rentCar.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Booking/_rentCar.cshtml", typeof(AspNetCore.Views_Booking__rentCar))]
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
#line 1 "/Users/phil/Desktop/Git/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/Booking/_rentCar.cshtml"
using KKday.Web.B2D.EC.Models.Model.Product;

#line default
#line hidden
#line 2 "/Users/phil/Desktop/Git/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/Booking/_rentCar.cshtml"
using KKday.Web.B2D.EC.Models.Model.Booking;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"21e1c91a2d718cbc34426b70b9570a56e32ba48a", @"/Views/Booking/_rentCar.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"a1decf7227a59ebd0c9a1b3569a3e4065089e3cb", @"/Views/_ViewImports.cshtml")]
    public class Views_Booking__rentCar : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("value", "null", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("selected", new global::Microsoft.AspNetCore.Html.HtmlString("selected"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute(":value", new global::Microsoft.AspNetCore.Html.HtmlString("null"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
#line 4 "/Users/phil/Desktop/Git/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/Booking/_rentCar.cshtml"
  
    var useDate = (string)ViewData["useDate"];
    var rentCar = ( RentCar)ViewData["rentCar"];
    var prodTitle = (ProdTitleModel)ViewData["prodTitle"];

#line default
#line hidden
            BeginContext(259, 4, true);
            WriteLiteral("\r\n\r\n");
            EndContext();
#line 11 "/Users/phil/Desktop/Git/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/Booking/_rentCar.cshtml"
 if (rentCar != null)
{
   

#line default
#line hidden
#line 13 "/Users/phil/Desktop/Git/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/Booking/_rentCar.cshtml"
    if (rentCar.rent_type == "01" || rentCar.rent_type == "02")
   {

#line default
#line hidden
            BeginContext(356, 65, true);
            WriteLiteral("       <!--_.includes([\'01\', \'02\'], this.rentCarType) 連動還沒有做-->\r\n");
            EndContext();
            BeginContext(428, 32, false);
#line 16 "/Users/phil/Desktop/Git/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/Booking/_rentCar.cshtml"
 Write(Html.Hidden("hdnSelPickUpId",""));

#line default
#line hidden
            EndContext();
            BeginContext(469, 33, false);
#line 17 "/Users/phil/Desktop/Git/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/Booking/_rentCar.cshtml"
 Write(Html.Hidden("hdnSelDropOffId",""));

#line default
#line hidden
            EndContext();
#line 18 "/Users/phil/Desktop/Git/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/Booking/_rentCar.cshtml"
     foreach (Office r in rentCar.rent_office.office_list)
    {
        

#line default
#line hidden
            BeginContext(580, 170, false);
#line 20 "/Users/phil/Desktop/Git/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/Booking/_rentCar.cshtml"
   Write(Html.Hidden("hdnPickup_"+r.id, r.business_hour.from.hour+"%"+ r.business_hour.from.minute+"%" +r.business_hour.to.hour+"%"+ r.business_hour.to.minute+"%" + r.address_eng));

#line default
#line hidden
            EndContext();
#line 20 "/Users/phil/Desktop/Git/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/Booking/_rentCar.cshtml"
                                                                                                                                                                                   
    }

#line default
#line hidden
            BeginContext(759, 63, true);
            WriteLiteral("    <div class=\"traveler-con\">\r\n        <div class=\"sub-title\">");
            EndContext();
            BeginContext(823, 32, false);
#line 23 "/Users/phil/Desktop/Git/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/Booking/_rentCar.cshtml"
                          Write(prodTitle.booking_step1_rent_car);

#line default
#line hidden
            EndContext();
            BeginContext(855, 191, true);
            WriteLiteral("</div>\r\n          <div class=\"row\">\r\n              <div class=\"col-md-6 col-sm-12\">\r\n                  <!-- 取車地點 -->\r\n                  <div class=\"form-group\">\r\n                      <label>");
            EndContext();
            BeginContext(1047, 47, false);
#line 28 "/Users/phil/Desktop/Git/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/Booking/_rentCar.cshtml"
                        Write(prodTitle.booking_step1_rent_car_pick_up_office);

#line default
#line hidden
            EndContext();
            BeginContext(1094, 183, true);
            WriteLiteral("</label>\r\n                      <select class=\"form-control \" id=\"selRentCarPickupOfiice\" name=\"selRentCarPickupOfiice\" onchange=\"chgRentCarTip(\'P\',this)\">\r\n                          ");
            EndContext();
            BeginContext(1277, 177, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("option", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "95bc99d49d264196982c9b9d20cd6cff", async() => {
                BeginContext(1325, 32, true);
                WriteLiteral("\r\n                              ");
                EndContext();
                BeginContext(1358, 59, false);
#line 31 "/Users/phil/Desktop/Git/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/Booking/_rentCar.cshtml"
                         Write(prodTitle.booking_step1_rent_car_pick_up_office_placeholder);

#line default
#line hidden
                EndContext();
                BeginContext(1417, 28, true);
                WriteLiteral("\r\n                          ");
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
            BeginContext(1454, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 33 "/Users/phil/Desktop/Git/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/Booking/_rentCar.cshtml"
                           foreach( Office r in rentCar.rent_office.office_list)
                          {

#line default
#line hidden
            BeginContext(1567, 29, true);
            WriteLiteral("                             ");
            EndContext();
            BeginContext(1596, 112, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("option", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "ddfd648b518a4b59ae461f70db03f740", async() => {
                BeginContext(1618, 36, true);
                WriteLiteral("\r\n                                  ");
                EndContext();
                BeginContext(1655, 13, false);
#line 36 "/Users/phil/Desktop/Git/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/Booking/_rentCar.cshtml"
                             Write(r.office_name);

#line default
#line hidden
                EndContext();
                BeginContext(1668, 31, true);
                WriteLiteral("\r\n                             ");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper);
            BeginWriteTagHelperAttribute();
#line 35 "/Users/phil/Desktop/Git/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/Booking/_rentCar.cshtml"
                                WriteLiteral(r.id);

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
            BeginContext(1708, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 38 "/Users/phil/Desktop/Git/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/Booking/_rentCar.cshtml"
                          }

#line default
#line hidden
            BeginContext(1739, 327, true);
            WriteLiteral(@"                      </select>
                      <div class=""tip pickupTip col-md-6 col-sm-12"">
                           
                      </div>
                  </div>
              </div>
          </div>
              <div class=""form-group"">
                  <!-- 取車時間 -->
                  <label> ");
            EndContext();
            BeginContext(2067, 45, false);
#line 48 "/Users/phil/Desktop/Git/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/Booking/_rentCar.cshtml"
                     Write(prodTitle.booking_step1_rent_car_pick_up_date);

#line default
#line hidden
            EndContext();
            BeginContext(2112, 290, true);
            WriteLiteral(@"</label> 
                  <div class=""row"">
                      <div class=""col-md-4 col-sm-6 col-xs-12"">
                          <div class=""form-group"">
                              <div class=""input-group date"" onclick="""">
                                  <input type=""text""");
            EndContext();
            BeginWriteAttribute("value", " value=\"", 2402, "\"", 2418, 1);
#line 53 "/Users/phil/Desktop/Git/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/Booking/_rentCar.cshtml"
WriteAttributeValue("", 2410, useDate, 2410, 8, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(2419, 21, true);
            WriteLiteral(" class=\"form-control\"");
            EndContext();
            BeginWriteAttribute("placeholder", " placeholder=\"", 2440, "\"", 2512, 1);
#line 53 "/Users/phil/Desktop/Git/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/Booking/_rentCar.cshtml"
WriteAttributeValue("", 2454, prodTitle.booking_step1_rent_car_pick_up_date_placeholder, 2454, 58, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(2513, 605, true);
            WriteLiteral(@" id=""txtRendCarPickUpDate"" name=""txtRendCarPickUpDate"" readonly=""readonly"" />
                                  <span class=""input-group-addon""><i class=""glyphicon glyphicon-calendar""></i></span>
                              </div>
                          </div>
                      </div>
                      <div class=""col-sm-6 col-xs-12"">
                          <div class=""form-group form-select"">
                              <select class=""form-control"" id=""selRentCarPickUpHour"" name=""selRentCarPickUpHour"" onchange=""chgRentCarHour('P',this)"">
                                  ");
            EndContext();
            BeginContext(3118, 136, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("option", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "86a4f4d0ae4445bba43d41582e5c7624", async() => {
                BeginContext(3167, 78, true);
                WriteLiteral("\r\n                                      --\r\n                                  ");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
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
            BeginContext(3254, 294, true);
            WriteLiteral(@"
                              </select>
                          </div>
                          <div class=""form-group form-select"">
                              <select class=""form-control"" id=""selRentCarPickUpMinute"" name=""selRentCarPickUpMinute"">
                                  ");
            EndContext();
            BeginContext(3548, 138, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("option", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "7db4ef0cc39f47b4ac4ad66f37cb7622", async() => {
                BeginContext(3599, 78, true);
                WriteLiteral("\r\n                                      --\r\n                                  ");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
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
            BeginContext(3686, 237, true);
            WriteLiteral("\r\n                              </select>\r\n                          </div>\r\n                      </div>\r\n                      <div class=\"form-group\">\r\n                          <!-- 是否需要免費Wi-Fi機 -->\r\n                          <label>");
            EndContext();
            BeginContext(3924, 50, false);
#line 76 "/Users/phil/Desktop/Git/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/Booking/_rentCar.cshtml"
                            Write(prodTitle.booking_step1_rent_car_is_need_free_wifi);

#line default
#line hidden
            EndContext();
            BeginContext(3974, 241, true);
            WriteLiteral("</label>\r\n                          <div class=\"form-inline\">\r\n                              <label>\r\n                                  <input type=\"radio\" name=\"rentCarwifi\" value=\"true\" id=\"rdoWifiTrue\">\r\n                                  ");
            EndContext();
            BeginContext(4216, 21, false);
#line 80 "/Users/phil/Desktop/Git/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/Booking/_rentCar.cshtml"
                             Write(prodTitle.common_need);

#line default
#line hidden
            EndContext();
            BeginContext(4237, 222, true);
            WriteLiteral("\r\n                              </label>\r\n                              <label>\r\n                                  <input type=\"radio\" name=\"rentCarwifi\" vaule=\"false\" id=\"rdoWifiFalse\">\r\n                                  ");
            EndContext();
            BeginContext(4460, 24, false);
#line 84 "/Users/phil/Desktop/Git/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/Booking/_rentCar.cshtml"
                             Write(prodTitle.common_no_need);

#line default
#line hidden
            EndContext();
            BeginContext(4484, 235, true);
            WriteLiteral("\r\n                              </label>\r\n                          </div>\r\n                       </div>\r\n                       <div class=\"form-group\">\r\n                          <!-- 是否需要免費GPS -->\r\n                          <label>");
            EndContext();
            BeginContext(4720, 49, false);
#line 90 "/Users/phil/Desktop/Git/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/Booking/_rentCar.cshtml"
                            Write(prodTitle.booking_step1_rent_car_is_need_free_gps);

#line default
#line hidden
            EndContext();
            BeginContext(4769, 241, true);
            WriteLiteral("</label>\r\n                          <div class=\"form-inline\">\r\n                              <label>\r\n                                  <input type=\"radio\"  name=\"rentCargps\" :value=\"true\" id=\"rdoGpsTrue\">\r\n                                  ");
            EndContext();
            BeginContext(5011, 21, false);
#line 94 "/Users/phil/Desktop/Git/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/Booking/_rentCar.cshtml"
                             Write(prodTitle.common_need);

#line default
#line hidden
            EndContext();
            BeginContext(5032, 223, true);
            WriteLiteral(" \r\n                              </label>\r\n                              <label>\r\n                                  <input type=\"radio\"  name=\"rentCargps\" :value=\"false\" id=\"rdoGpsFalse\">\r\n                                  ");
            EndContext();
            BeginContext(5256, 24, false);
#line 98 "/Users/phil/Desktop/Git/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/Booking/_rentCar.cshtml"
                             Write(prodTitle.common_no_need);

#line default
#line hidden
            EndContext();
            BeginContext(5280, 353, true);
            WriteLiteral(@" 
                              </label>
                          </div>
                     </div>
                  </div>
              </div>
              <div class=""row"">
                  <!-- 還車地點 -->
                  <div class=""col-md-6 col-sm-12"">
                      <div class=""form-group"">
                          <label>");
            EndContext();
            BeginContext(5634, 48, false);
#line 108 "/Users/phil/Desktop/Git/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/Booking/_rentCar.cshtml"
                            Write(prodTitle.booking_step1_rent_car_drop_off_office);

#line default
#line hidden
            EndContext();
            BeginContext(5682, 194, true);
            WriteLiteral("</label>\r\n                          <select class=\"form-control \" id=\"selRentCarDropOffOfiice\" name=\"selRentCarDropOffOfiice\" onchange=\"chgRentCarTip(\'D\',this)\" >\r\n                              ");
            EndContext();
            BeginContext(5876, 186, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("option", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "8cb8736989e245c1911dff4baa02fedd", async() => {
                BeginContext(5924, 36, true);
                WriteLiteral("\r\n                                  ");
                EndContext();
                BeginContext(5961, 60, false);
#line 111 "/Users/phil/Desktop/Git/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/Booking/_rentCar.cshtml"
                             Write(prodTitle.booking_step1_rent_car_drop_off_office_placeholder);

#line default
#line hidden
                EndContext();
                BeginContext(6021, 32, true);
                WriteLiteral("\r\n                              ");
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
            BeginContext(6062, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 113 "/Users/phil/Desktop/Git/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/Booking/_rentCar.cshtml"
                               foreach( Office r in rentCar.rent_office.office_list)
                              {

#line default
#line hidden
            BeginContext(6183, 30, true);
            WriteLiteral("                              ");
            EndContext();
            BeginContext(6213, 113, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("option", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "8b724f2b3d314fb6b0d55fb6262fa7ca", async() => {
                BeginContext(6235, 36, true);
                WriteLiteral("\r\n                                  ");
                EndContext();
                BeginContext(6272, 13, false);
#line 116 "/Users/phil/Desktop/Git/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/Booking/_rentCar.cshtml"
                             Write(r.office_name);

#line default
#line hidden
                EndContext();
                BeginContext(6285, 32, true);
                WriteLiteral("\r\n                              ");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper);
            BeginWriteTagHelperAttribute();
#line 115 "/Users/phil/Desktop/Git/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/Booking/_rentCar.cshtml"
                                 WriteLiteral(r.id);

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
            BeginContext(6326, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 118 "/Users/phil/Desktop/Git/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/Booking/_rentCar.cshtml"
                              }

#line default
#line hidden
            BeginContext(6361, 307, true);
            WriteLiteral(@"                          </select>
                      </div>
                      <div class=""tip dropOfftip col-md-6 col-sm-12""></div>
                  </div>
                  </div>
                  <div class=""form-group"">
                      <!-- 取車時間 -->
                      <label> ");
            EndContext();
            BeginContext(6669, 45, false);
#line 126 "/Users/phil/Desktop/Git/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/Booking/_rentCar.cshtml"
                         Write(prodTitle.booking_step1_rent_car_pick_up_date);

#line default
#line hidden
            EndContext();
            BeginContext(6714, 266, true);
            WriteLiteral(@"</label>
                      <div class=""row"">
                          <div class=""col-md-4 col-sm-6 col-xs-12"">
                              <div class=""input-group date"" onclick="""">
                                  <input type=""text"" class=""form-control""");
            EndContext();
            BeginWriteAttribute("placeholder", " placeholder=\"", 6980, "\"", 7052, 1);
#line 130 "/Users/phil/Desktop/Git/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/Booking/_rentCar.cshtml"
WriteAttributeValue("", 6994, prodTitle.booking_step1_rent_car_pick_up_date_placeholder, 6994, 58, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(7053, 595, true);
            WriteLiteral(@" id=""txtRendCarDropOffDate"" name=""txtRendCarDropOffDate"" readonly=""readonly"" />
                                  <span class=""input-group-addon""><i class=""glyphicon glyphicon-calendar""></i></span>
                              </div>
                          </div>
                          <div class=""col-sm-6 col-xs-12"">
                              <div class=""form-group form-select"">
                                  <select class=""form-control"" id=""selRentCarDropOffHour"" name=""selRentCarDropOffHour"" onchange=""chgRentCarHour('D',this)"">
                                      ");
            EndContext();
            BeginContext(7648, 146, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("option", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "a22bd7cdfc544901884fa1a445b3f067", async() => {
                BeginContext(7699, 86, true);
                WriteLiteral("\r\n                                          --\r\n                                      ");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
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
            BeginContext(7794, 316, true);
            WriteLiteral(@"
                                  </select>
                              </div>
                              <div class=""form-group form-select"">
                                  <select class=""form-control"" id=""selRentCarDropoffMinute"" name=""selRentCarDropoffMinute"">
                                      ");
            EndContext();
            BeginContext(8110, 146, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("option", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "23d0e3948132463a9c3821e1403a94de", async() => {
                BeginContext(8161, 86, true);
                WriteLiteral("\r\n                                          --\r\n                                      ");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
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
            BeginContext(8256, 187, true);
            WriteLiteral("\r\n                                  </select>\r\n                              </div>\r\n                          </div>\r\n                      </div>\r\n                  </div>\r\n    </div>\r\n");
            EndContext();
#line 153 "/Users/phil/Desktop/Git/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/Booking/_rentCar.cshtml"

   }

#line default
#line hidden
#line 154 "/Users/phil/Desktop/Git/Src/Web/KKday.Web.B2D.EC/KKday.Web.B2D.EC/Views/Booking/_rentCar.cshtml"
    
}

#line default
#line hidden
            BeginContext(8454, 6, true);
            WriteLiteral("\r\n\r\n\r\n");
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
