using HtmlAgilityPack;
using System.Net;

namespace Utility.Helpers
{
    public static class HelperStripHtml
    {
        public static string StripHTML(this string text)
        {
            HtmlDocument htmlDoc = new HtmlDocument();

            htmlDoc.LoadHtml(text);

            if (htmlDoc == null) return text;

            string strippedText = htmlDoc.DocumentNode.InnerText;

            return WebUtility.HtmlDecode(strippedText);
        }
    }
}
