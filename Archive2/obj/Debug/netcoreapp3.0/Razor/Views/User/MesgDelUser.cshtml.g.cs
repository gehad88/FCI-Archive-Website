#pragma checksum "D:\SW Engineering\Sixth term\Software Development\Project\Project\Software-Development-Project\Archive2\Views\User\MesgDelUser.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "a57ed5a65f9822e0f7235d2bdebe6553feba7629"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_User_MesgDelUser), @"mvc.1.0.view", @"/Views/User/MesgDelUser.cshtml")]
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
#nullable restore
#line 1 "D:\SW Engineering\Sixth term\Software Development\Project\Project\Software-Development-Project\Archive2\Views\_ViewImports.cshtml"
using Archive2;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\SW Engineering\Sixth term\Software Development\Project\Project\Software-Development-Project\Archive2\Views\_ViewImports.cshtml"
using Archive2.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"a57ed5a65f9822e0f7235d2bdebe6553feba7629", @"/Views/User/MesgDelUser.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"e686de7ce6e2b4c2b71065f22d651816412f73e2", @"/Views/_ViewImports.cshtml")]
    #nullable restore
    public class Views_User_MesgDelUser : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    #nullable disable
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 1 "D:\SW Engineering\Sixth term\Software Development\Project\Project\Software-Development-Project\Archive2\Views\User\MesgDelUser.cshtml"
  
    Layout = "_AdmainLayout";

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
<div class=""container text-center mb-5 login-btn"">
    <div>
        <h1 style=""color:black""> Removed User Messages </h1>
        <hr class="" w/75"" />
    </div>
</div>
<style>
    .input-group {
        width: 50%;
        display: flex;
        justify-content: center; /* Added to center search button */
        margin: 0 auto; /* Added to center input group */
    }

    select {
        width: 150px;
    }

        select:focus {
            min-width: 150px;
            width: auto;
        }
</style>


<style>
    body {
        /*        background-image: url('../login/img/uby-yanes-lx7g_nojyla-unsplash.jpg');
                */
        background-image: url('../login/img/untitled.jpeg');
        background-repeat: repeat;
        background-size: cover;
        background-position: center center;
    }
</style>


<div class=""container rounded mt-5 mb-5"">


    <div id=""datatable""></div>

    <div dir=""ltr"">
        <table class=""table table-striped table-ho");
            WriteLiteral(@"ver"">
            <thead class=""table-light"">
                <tr>
                    <th scope=""col"">ID</th>
                    <th scope=""col"">Message Title</th>
                    <th scope=""col"">Date of send</th>
                    <th scope=""col"">Receivers</th>
                </tr>
            </thead>
            <tbody>
");
#nullable restore
#line 58 "D:\SW Engineering\Sixth term\Software Development\Project\Project\Software-Development-Project\Archive2\Views\User\MesgDelUser.cshtml"
                 foreach (var item in ViewBag.Messages)
                {

#line default
#line hidden
#nullable disable
            WriteLiteral("                    <tr>\r\n                        <td>");
#nullable restore
#line 61 "D:\SW Engineering\Sixth term\Software Development\Project\Project\Software-Development-Project\Archive2\Views\User\MesgDelUser.cshtml"
                       Write(item.MessagesId);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                        <td>");
#nullable restore
#line 62 "D:\SW Engineering\Sixth term\Software Development\Project\Project\Software-Development-Project\Archive2\Views\User\MesgDelUser.cshtml"
                       Write(item.Title);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                        <th scope=\"row\">");
#nullable restore
#line 63 "D:\SW Engineering\Sixth term\Software Development\Project\Project\Software-Development-Project\Archive2\Views\User\MesgDelUser.cshtml"
                                   Write(item.SendDate);

#line default
#line hidden
#nullable disable
            WriteLiteral("</th>\r\n                        <th scope=\"row\">\r\n");
#nullable restore
#line 65 "D:\SW Engineering\Sixth term\Software Development\Project\Project\Software-Development-Project\Archive2\Views\User\MesgDelUser.cshtml"
                             foreach (var kvp in ViewBag.Recipients)
                            {
                                

#line default
#line hidden
#nullable disable
#nullable restore
#line 67 "D:\SW Engineering\Sixth term\Software Development\Project\Project\Software-Development-Project\Archive2\Views\User\MesgDelUser.cshtml"
                                 if (kvp.Key == item.MessagesId)
                                {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                    <ul>\r\n");
#nullable restore
#line 70 "D:\SW Engineering\Sixth term\Software Development\Project\Project\Software-Development-Project\Archive2\Views\User\MesgDelUser.cshtml"
                                         foreach (var email in (dynamic)kvp.Value)
                                        {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                            <li>");
#nullable restore
#line 72 "D:\SW Engineering\Sixth term\Software Development\Project\Project\Software-Development-Project\Archive2\Views\User\MesgDelUser.cshtml"
                                           Write(email);

#line default
#line hidden
#nullable disable
            WriteLiteral("</li>\r\n");
#nullable restore
#line 73 "D:\SW Engineering\Sixth term\Software Development\Project\Project\Software-Development-Project\Archive2\Views\User\MesgDelUser.cshtml"
                                        }

#line default
#line hidden
#nullable disable
            WriteLiteral("                                    </ul>\r\n");
#nullable restore
#line 75 "D:\SW Engineering\Sixth term\Software Development\Project\Project\Software-Development-Project\Archive2\Views\User\MesgDelUser.cshtml"
                                }

#line default
#line hidden
#nullable disable
#nullable restore
#line 75 "D:\SW Engineering\Sixth term\Software Development\Project\Project\Software-Development-Project\Archive2\Views\User\MesgDelUser.cshtml"
                                 

                            }

#line default
#line hidden
#nullable disable
            WriteLiteral("                        </th>\r\n                    </tr>\r\n");
#nullable restore
#line 80 "D:\SW Engineering\Sixth term\Software Development\Project\Project\Software-Development-Project\Archive2\Views\User\MesgDelUser.cshtml"
                }

#line default
#line hidden
#nullable disable
            WriteLiteral("            </tbody>\r\n        </table>\r\n    </div>\r\n</div>");
        }
        #pragma warning restore 1998
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; } = default!;
        #nullable disable
    }
}
#pragma warning restore 1591
