using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace MultiTenantEcommerce.Infrastructure.EmailService;
public static class HtmlToText
{
    public static string Convert(string html)
    {
        if (string.IsNullOrWhiteSpace(html)) return string.Empty;
        var doc = new HtmlDocument();
        doc.LoadHtml(html);
        var text = HtmlEntity.DeEntitize(doc.DocumentNode.InnerText);
        return Regex.Replace(text, @"\s{2,}", " ").Trim();
    }
}
