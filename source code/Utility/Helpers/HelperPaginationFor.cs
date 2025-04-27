//using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Utility.Helpers
{
    public class HelperPaginationFor : TagHelper
    {
        public PagedList<object> Model { get; set; }
        public string? PageUrl { get; set; }

        public HelperPaginationFor()
        {
            this.Model = new PagedList<object>(new List<object>(), 1, 30, 0);
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;

            var ul = new TagBuilder("ul");
            ul.AddCssClass("pagination d-flex justify-content-end align-items-center");


            // << First
            ul.InnerHtml.AppendHtml(CreatePageLink("<<", 1, isActive: false, disabled: Model.PageNumber == 1));

            // < Previous
            ul.InnerHtml.AppendHtml(CreatePageLink("<", Model.PageNumber - 1, isActive: false, disabled: Model.PageNumber == 1));

            // Smart Pagination Logic
            if (Model.TotalPages <= 7)
            {
                // Show all pages if total pages are small
                for (int i = 1; i <= Model.TotalPages; i++)
                {
                    ul.InnerHtml.AppendHtml(CreatePageLink(i.ToString(), i, isActive: Model.PageNumber == i));
                }
            }
            else
            {
                if (Model.PageNumber <= 4)
                {
                    // Near the start
                    for (int i = 1; i <= 5; i++)
                    {
                        ul.InnerHtml.AppendHtml(CreatePageLink(i.ToString(), i, isActive: Model.PageNumber == i));
                    }
                    ul.InnerHtml.AppendHtml(CreateDots());
                    ul.InnerHtml.AppendHtml(CreatePageLink(Model.TotalPages.ToString(), Model.TotalPages, isActive: false));
                }
                else if (Model.PageNumber >= Model.TotalPages - 3)
                {
                    // Near the end
                    ul.InnerHtml.AppendHtml(CreatePageLink("1", 1, isActive: false));
                    ul.InnerHtml.AppendHtml(CreateDots());
                    for (int i = Model.TotalPages - 4; i <= Model.TotalPages; i++)
                    {
                        ul.InnerHtml.AppendHtml(CreatePageLink(i.ToString(), i, isActive: Model.PageNumber == i));
                    }
                }
                else
                {
                    // Somewhere in the middle
                    ul.InnerHtml.AppendHtml(CreatePageLink("1", 1, isActive: false));
                    ul.InnerHtml.AppendHtml(CreateDots());
                    for (int i = Model.PageNumber - 1; i <= Model.PageNumber + 1; i++)
                    {
                        ul.InnerHtml.AppendHtml(CreatePageLink(i.ToString(), i, isActive: Model.PageNumber == i));
                    }
                    ul.InnerHtml.AppendHtml(CreateDots());
                    ul.InnerHtml.AppendHtml(CreatePageLink(Model.TotalPages.ToString(), Model.TotalPages, isActive: false));
                }
            }
            // > Next
            ul.InnerHtml.AppendHtml(CreatePageLink(">", Model.PageNumber + 1, isActive: false, disabled: Model.PageNumber == Model.TotalPages));

            // >> Last
            ul.InnerHtml.AppendHtml(CreatePageLink(">>", Model.TotalPages, isActive: false, disabled: Model.PageNumber == Model.TotalPages));

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
