#pragma checksum "/Users/eric/MyProjects/KKday.Web.B2D.BE/Areas/User/Views/Account/UserList.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "d7cba9b9c26860ba9defdb7bf9bfead7ba49698c"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Areas_User_Views_Account_UserList), @"mvc.1.0.view", @"/Areas/User/Views/Account/UserList.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Areas/User/Views/Account/UserList.cshtml", typeof(AspNetCore.Areas_User_Views_Account_UserList))]
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
#line 1 "/Users/eric/MyProjects/KKday.Web.B2D.BE/Areas/User/Views/_ViewImports.cshtml"
using KKday.Web.B2D.BE;

#line default
#line hidden
#line 2 "/Users/eric/MyProjects/KKday.Web.B2D.BE/Areas/User/Views/_ViewImports.cshtml"
using KKday.Web.B2D.BE.Areas.User.Models;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"d7cba9b9c26860ba9defdb7bf9bfead7ba49698c", @"/Areas/User/Views/Account/UserList.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"bc7bac046ff07d4ef2ecf2ecf6161a63e44be55d", @"/Areas/User/Views/_ViewImports.cshtml")]
    public class Areas_User_Views_Account_UserList : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(7, 4929, true);
            WriteLiteral(@"<table id=""mytable"" class=""table table-bordred table-striped"">
    <thead>
        <tr>
            <th><input type=""checkbox"" id=""checkall"" /></th>
            <th>First Name</th>
            <th>Last Name</th>
            <th>Address</th>
            <th>Email</th>
            <th>Contact</th>
            <th>Edit</th>
            <th>Delete</th>
        </tr>
    </thead>
    <tbody>

        <tr>
            <td><input type=""checkbox"" class=""checkthis"" aria-label=""Select all items in this table"" title=""Select all items in this table"" /></td>
            <td>Mohsin</td>
            <td>Irshad</td>
            <td>CB 106/107 Street # 11 Wah Cantt Islamabad Pakistan</td>
            <td>isometric.mohsin@gmail.com</td>
            <td>+923335586757</td>
            <td><p data-placement=""top"" data-toggle=""tooltip"" title=""Edit""><button class=""btn btn-primary btn-xs"" data-title=""Edit"" data-toggle=""modal"" data-target=""#edit""><span class=""glyphicon glyphicon-pencil""></span></button></p></td>");
            WriteLiteral(@"
            <td><p data-placement=""top"" data-toggle=""tooltip"" title=""Delete""><button class=""btn btn-danger btn-xs"" data-title=""Delete"" data-toggle=""modal"" data-target=""#delete""><span class=""glyphicon glyphicon-trash""></span></button></p></td>
        </tr>
        <tr>
            <td><input type=""checkbox"" class=""checkthis"" /></td>
            <td>Mohsin</td>
            <td>Irshad</td>
            <td>CB 106/107 Street # 11 Wah Cantt Islamabad Pakistan</td>
            <td>isometric.mohsin@gmail.com</td>
            <td>+923335586757</td>
            <td><p data-placement=""top"" data-toggle=""tooltip"" title=""Edit""><button class=""btn btn-primary btn-xs"" data-title=""Edit"" data-toggle=""modal"" data-target=""#edit""><span class=""glyphicon glyphicon-pencil""></span></button></p></td>
            <td><p data-placement=""top"" data-toggle=""tooltip"" title=""Delete""><button class=""btn btn-danger btn-xs"" data-title=""Delete"" data-toggle=""modal"" data-target=""#delete""><span class=""glyphicon glyphicon-trash""></span></");
            WriteLiteral(@"button></p></td>
        </tr>
        <tr>
            <td><input type=""checkbox"" class=""checkthis"" /></td>
            <td>Mohsin</td>
            <td>Irshad</td>
            <td>CB 106/107 Street # 11 Wah Cantt Islamabad Pakistan</td>
            <td>isometric.mohsin@gmail.com</td>
            <td>+923335586757</td>
            <td><p data-placement=""top"" data-toggle=""tooltip"" title=""Edit""><button class=""btn btn-primary btn-xs"" data-title=""Edit"" data-toggle=""modal"" data-target=""#edit""><span class=""glyphicon glyphicon-pencil""></span></button></p></td>
            <td><p data-placement=""top"" data-toggle=""tooltip"" title=""Delete""><button class=""btn btn-danger btn-xs"" data-title=""Delete"" data-toggle=""modal"" data-target=""#delete""><span class=""glyphicon glyphicon-trash""></span></button></p></td>
        </tr>
        <tr>
            <td><input type=""checkbox"" class=""checkthis"" /></td>
            <td>Mohsin</td>
            <td>Irshad</td>
            <td>CB 106/107 Street # 11 Wah Cantt Islamaba");
            WriteLiteral(@"d Pakistan</td>
            <td>isometric.mohsin@gmail.com</td>
            <td>+923335586757</td>
            <td><p data-placement=""top"" data-toggle=""tooltip"" title=""Edit""><button class=""btn btn-primary btn-xs"" data-title=""Edit"" data-toggle=""modal"" data-target=""#edit""><span class=""glyphicon glyphicon-pencil""></span></button></p></td>
            <td><p data-placement=""top"" data-toggle=""tooltip"" title=""Delete""><button class=""btn btn-danger btn-xs"" data-title=""Delete"" data-toggle=""modal"" data-target=""#delete""><span class=""glyphicon glyphicon-trash""></span></button></p></td>
        </tr>
        <tr>
            <td><input type=""checkbox"" class=""checkthis"" /></td>
            <td>Mohsin</td>
            <td>Irshad</td>
            <td>CB 106/107 Street # 11 Wah Cantt Islamabad Pakistan</td>
            <td>isometric.mohsin@gmail.com</td>
            <td>+923335586757</td>
            <td><p data-placement=""top"" data-toggle=""tooltip"" title=""Edit""><button class=""btn btn-primary btn-xs"" data-title=""");
            WriteLiteral(@"Edit"" data-toggle=""modal"" data-target=""#edit""><span class=""glyphicon glyphicon-pencil""></span></button></p></td>
            <td><p data-placement=""top"" data-toggle=""tooltip"" title=""Delete""><button class=""btn btn-danger btn-xs"" data-title=""Delete"" data-toggle=""modal"" data-target=""#delete""><span class=""glyphicon glyphicon-trash""></span></button></p></td>
        </tr>

    </tbody>
</table>
<div class=""clearfix""></div>
<ul class=""pagination pull-right"">
    <li class=""disabled""><a href=""#""><span class=""glyphicon glyphicon-chevron-left""></span></a></li>
    <li class=""active""><a href=""#"">1</a></li>
    <li><a href=""#"">2</a></li>
    <li><a href=""#"">3</a></li>
    <li><a href=""#"">4</a></li>
    <li><a href=""#"">5</a></li>
    <li><a href=""#""><span class=""glyphicon glyphicon-chevron-right""></span></a></li>
</ul>");
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
