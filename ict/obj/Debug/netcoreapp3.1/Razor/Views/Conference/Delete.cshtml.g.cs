#pragma checksum "F:\webICT\ict\ict\Views\Conference\Delete.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "0bd7cd4b14b9d72b8b0876f39172892b78eebac7"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Conference_Delete), @"mvc.1.0.view", @"/Views/Conference/Delete.cshtml")]
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
#line 1 "F:\webICT\ict\ict\Views\_ViewImports.cshtml"
using ict;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "F:\webICT\ict\ict\Views\_ViewImports.cshtml"
using ict.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"0bd7cd4b14b9d72b8b0876f39172892b78eebac7", @"/Views/Conference/Delete.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"e944af819c70f04043eed4fceab3701b91397aaa", @"/Views/_ViewImports.cshtml")]
    public class Views_Conference_Delete : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<ict.Models.ConferenceModel>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "Delete", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
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
            WriteLiteral("\r\n\r\n<div class=\"main-content container-fluid\">\r\n    <div class=\"page-title\">\r\n        <div class=\"row\">\r\n            <div class=\"col-12 col-md-6 order-md-1 order-last\">\r\n                <h3>ลบการจอง Tele Conference</h3>\r\n");
            WriteLiteral("            </div>\r\n            <div class=\"col-12 col-md-6 order-md-2 order-first\">\r\n                <nav aria-label=\"breadcrumb\" class=\'breadcrumb-header\'>\r\n                    <ol class=\"breadcrumb\">\r\n");
            WriteLiteral("                    </ol>\r\n                </nav>\r\n            </div>\r\n        </div>\r\n\r\n    </div>\r\n    <hr />\r\n    <section class=\"section\">\r\n        <div class=\"card\">\r\n            <div class=\"card-header\">\r\n                <h4 class=\"card-title\">");
#nullable restore
#line 26 "F:\webICT\ict\ict\Views\Conference\Delete.cshtml"
                                  Write(Html.DisplayFor(model => model.DtTitle));

#line default
#line hidden
#nullable disable
            WriteLiteral("</h4>\r\n            </div>\r\n            <div class=\"card-body\">\r\n                <dl class=\"row\">\r\n");
            WriteLiteral("\r\n                    <dt class=\"col-sm-2\">\r\n                        ");
#nullable restore
#line 38 "F:\webICT\ict\ict\Views\Conference\Delete.cshtml"
                   Write(Html.DisplayNameFor(model => model.DtDate));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                    </dt>\r\n                    <dd class=\"col-sm-10\">\r\n                        ");
#nullable restore
#line 41 "F:\webICT\ict\ict\Views\Conference\Delete.cshtml"
                   Write(Html.DisplayFor(model => model.DtDate));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                    </dd>\r\n\r\n                    <dt class=\"col-sm-2\">\r\n                        ");
#nullable restore
#line 45 "F:\webICT\ict\ict\Views\Conference\Delete.cshtml"
                   Write(Html.DisplayNameFor(model => model.DtDate2));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                    </dt>\r\n                    <dd class=\"col-sm-10\">\r\n                        ");
#nullable restore
#line 48 "F:\webICT\ict\ict\Views\Conference\Delete.cshtml"
                   Write(Html.DisplayFor(model => model.DtDate2));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                    </dd>\r\n\r\n                </dl>\r\n\r\n                ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "0bd7cd4b14b9d72b8b0876f39172892b78eebac76025", async() => {
                WriteLiteral("\r\n                    <input type=\"submit\" value=\"ลบ\" class=\"btn btn-danger\" />\r\n\r\n                ");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Action = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n                <div>\r\n                   <p></p> <a onclick=\"window.history.back()\" href=\"#\"><< ย้อนกลับ</a>\r\n                </div>\r\n            </div>\r\n\r\n        </div>\r\n    </section>\r\n</div>");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<ict.Models.ConferenceModel> Html { get; private set; }
    }
}
#pragma warning restore 1591