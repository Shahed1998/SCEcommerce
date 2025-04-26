//using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Primitives;


namespace Utility.Helpers
{
    public class HelperPaginationFor : TagHelper
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string? PageUrl { get; set; }
        public string? SearchQuery { get; set; } 

        public HelperPaginationFor()
        {
            
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;

            var ul = new TagBuilder("ul");
            ul.AddCssClass("pagination d-flex justify-content-end align-items-center");


            // << First
            ul.InnerHtml.AppendHtml(CreatePageLink("<<", 1, isActive: false, disabled: CurrentPage == 1));

            // < Previous
            ul.InnerHtml.AppendHtml(CreatePageLink("<", CurrentPage - 1, isActive: false, disabled: CurrentPage == 1));

            // Pages 1 2 3 4
            for (int i = CurrentPage; i <= CurrentPage + 4 && i <= TotalPages; i++)
            {
                ul.InnerHtml.AppendHtml(CreatePageLink(i.ToString(), i, isActive: CurrentPage == i));
            }

            // Dots ...
            if (TotalPages > 6)
            {
                ul.InnerHtml.AppendHtml(CreateDots());
            }

            // Last two pages 999 1000
            if (TotalPages > 4)
            {
                for (int i = TotalPages - 1; i <= TotalPages; i++)
                {
                    ul.InnerHtml.AppendHtml(CreatePageLink(i.ToString(), i, isActive: CurrentPage == i));
                }
            }

            // > Next
            ul.InnerHtml.AppendHtml(CreatePageLink(">", CurrentPage + 1, isActive: false, disabled: CurrentPage == TotalPages));

            // >> Last
            ul.InnerHtml.AppendHtml(CreatePageLink(">>", TotalPages, isActive: false, disabled: CurrentPage == TotalPages));

            output.Content.AppendHtml(ul);
        }

        private TagBuilder CreatePageLink(string text, int pageNumber, bool isActive = false, bool disabled = false, string queryString = "")
        {
            var li = new TagBuilder("li");
            li.AddCssClass("page-item");

            if (isActive)
            {
                li.AddCssClass("active");
            }

            if (disabled)
            {
                li.AddCssClass("disabled");
            }

            var a = new TagBuilder("a");
            a.AddCssClass("page-link");

            if (!disabled)
            {
                a.Attributes["href"] = $"{PageUrl}?page={pageNumber}{queryString}";
            }
            else
            {
                a.Attributes["href"] = "#";
                a.Attributes["tabindex"] = "-1";
                a.Attributes["aria-disabled"] = "true";
            }

            a.InnerHtml.Append(text);

            li.InnerHtml.AppendHtml(a);
            return li;
        }

        private TagBuilder CreateDots()
        {
            var li = new TagBuilder("li");
            li.AddCssClass("page-item disabled");

            var span = new TagBuilder("span");
            span.AddCssClass("page-link");
            span.InnerHtml.Append("...");

            li.InnerHtml.AppendHtml(span);
            return li;
        }
    }
}
