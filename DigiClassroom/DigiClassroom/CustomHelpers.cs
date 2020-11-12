using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNetCore.Html;
using System.Text.RegularExpressions;
namespace DigiClassroom
{
    public static class CustomHelpers
    {
        /*private const string urlRegEx = @"((http|ftp|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?)";

        public static MvcHtmlString DisplayWithLinksFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            string content = GetContent<TModel, TProperty>(htmlHelper, expression);
            string result = ReplaceUrlsWithLinks(content);
            return MvcHtmlString.Create(result);
        }

        private static string ReplaceUrlsWithLinks(string input)
        {
            Regex rx = new Regex(urlRegEx);
            string result = rx.Replace(input, delegate (Match match)
            {
                string url = match.ToString();
                return String.Format("<a href=\"{0}\">{0}</a>", url);
            });
            return result;
        }

        private static string GetContent<TModel, TProperty>(HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            Func<TModel, TProperty> func = expression.Compile();
            return func(htmlHelper.ViewData.Model).ToString();
        }*/
        
    }
}
