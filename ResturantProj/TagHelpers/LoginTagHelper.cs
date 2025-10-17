using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text;

namespace ResturantProj.TagHelpers
{
    [HtmlTargetElement("Login")]
    public class LoginTagHelper : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "form";
            output.TagMode = TagMode.StartTagAndEndTag;

            var tagsfac = new StringBuilder();

            tagsfac.AppendLine($"<form asp-action=\"Login\">");
            tagsfac.AppendLine($" <div class=\"mb-3\">");
            tagsfac.AppendLine($"<label for=\"exampleInputEmail1\" class=\"form-label\">UserName</label>");
            tagsfac.AppendLine($"<input type=\"text\" class=\"form-control\" id=\"exampleInputEmail1\" aria-describedby=\"emailHelp\">");
            tagsfac.AppendLine($"</div>");
            tagsfac.AppendLine($"<div class=\"mb-3\">");
            tagsfac.AppendLine($" <label for=\"exampleInputPassword1\" class=\"form-label\">Password</label>");
            tagsfac.AppendLine($"<input type=\"password\" class=\"form-control\" id=\"exampleInputPassword1\">");
            tagsfac.AppendLine($"</div>");
            tagsfac.AppendLine($"<button type=\"submit\" class=\"btn btn-primary\">Submit</button>");
            tagsfac.AppendLine($"</form>");

            output.Content.SetHtmlContent(tagsfac.ToString());
        }
    }
}
