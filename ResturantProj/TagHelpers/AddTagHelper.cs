using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text;

namespace ResturantProj.TagHelpers
{
    [HtmlTargetElement("Addthing")]
    public class AddTagHelper : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "form";
            output.TagMode = TagMode.StartTagAndEndTag;

            var addfac = new StringBuilder();

            addfac.AppendLine($"<form asp-action=\"AddCat\" method=\"post\">");
            addfac.AppendLine($" <div class=\"mb-3\">");
            addfac.AppendLine($"<label for=\"name\" class=\"form-label\">Category Name</label>");
            addfac.AppendLine($"<input type=\"text\" class=\"form-control\" name=\"name\">");
            addfac.AppendLine($"</div>");
            addfac.AppendLine($"<button type=\"submit\" class=\"btn btn-primary\">Submit</button>");
            addfac.AppendLine($"</form>");
            output.Content.SetHtmlContent(addfac.ToString());
        }
    }
}
